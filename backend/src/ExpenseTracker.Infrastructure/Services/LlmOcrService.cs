using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ExpenseTracker.Core.DTOs;
using ExpenseTracker.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ExpenseTracker.Infrastructure.Services;

public class LlmOcrService : IOcrService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private const string GeminiApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

    public LlmOcrService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<ReceiptDto> ScanReceiptAsync(Stream imageStream, string contentType, IEnumerable<CategoryDto> availableCategories)
    {
        var apiKey = _configuration["Gemini:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new Exception("Gemini API key not configured. Please add 'Gemini:ApiKey' to appsettings.json");
        }

        // Convert image stream to base64
        using var memoryStream = new MemoryStream();
        await imageStream.CopyToAsync(memoryStream);
        var imageBytes = memoryStream.ToArray();
        var base64Image = Convert.ToBase64String(imageBytes);

        // Determine MIME type
        var mimeType = contentType switch
        {
            "image/jpeg" => "image/jpeg",
            "image/jpg" => "image/jpeg",
            "image/png" => "image/png",
            "image/webp" => "image/webp",
            _ => "image/jpeg"
        };

        // Build category list for LLM
        var categoryNames = availableCategories.Select(c => c.Name).ToList();
        var categoryListText = string.Join(", ", categoryNames);

        // Create request payload with structured output including category prediction
        var requestPayload = new
        {
            contents = new[]
            {
                new
                {
                    parts = new object[]
                    {
                        new
                        {
                            inline_data = new
                            {
                                mime_type = mimeType,
                                data = base64Image
                            }
                        },
                        new
                        {
                            text = $@"Analyze this receipt image and extract the following information in JSON format:
- merchantName: The name of the store/merchant
- totalAmount: The total amount paid (as a decimal number)
- transactionDate: The date of the transaction (in ISO 8601 format: YYYY-MM-DD)
- suggestedCategory: Based on the merchant name and items purchased, suggest the MOST APPROPRIATE category from this list: [{categoryListText}]
  Choose the category that best matches the type of purchase. For example:
  * Restaurants, cafes, food stores -> Food & Dining
  * Gas stations, uber, taxi -> Transportation
  * Clothing stores, electronics -> Shopping
  * Movies, games, entertainment venues -> Entertainment
  * Electricity, water, internet bills -> Bills & Utilities
  * Pharmacies, hospitals, clinics -> Healthcare
  * Books, courses, schools -> Education
  * If unsure -> Other

If any field is not clearly visible, use null for that field.
Return ONLY valid JSON, no additional text."
                        }
                    }
                }
            },
            generationConfig = new
            {
                responseMimeType = "application/json",
                responseSchema = new
                {
                    type = "object",
                    properties = new
                    {
                        merchantName = new { type = "string" },
                        totalAmount = new { type = "number" },
                        transactionDate = new { type = "string" },
                        suggestedCategory = new { type = "string" }
                    },
                    required = new[] { "merchantName", "totalAmount", "transactionDate", "suggestedCategory" }
                }
            }
        };

        var jsonPayload = JsonSerializer.Serialize(requestPayload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        // Make API request
        var request = new HttpRequestMessage(HttpMethod.Post, $"{GeminiApiUrl}?key={apiKey}")
        {
            Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Gemini API error: {response.StatusCode} - {responseContent}");
        }

        // Parse response
        var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseContent);
        var extractedText = geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

        if (string.IsNullOrEmpty(extractedText))
        {
            throw new Exception("No response from Gemini API");
        }

        // Parse the structured JSON response
        var receiptData = JsonSerializer.Deserialize<ReceiptData>(extractedText, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (receiptData == null)
        {
            throw new Exception("Failed to parse receipt data from Gemini response");
        }

        // Validate suggested category against available categories
        var suggestedCategory = receiptData.SuggestedCategory;
        if (!string.IsNullOrEmpty(suggestedCategory) &&
            !categoryNames.Any(c => c.Equals(suggestedCategory, StringComparison.OrdinalIgnoreCase)))
        {
            // If LLM suggested a category not in our list, default to "Other"
            suggestedCategory = categoryNames.FirstOrDefault(c => c.Equals("Other", StringComparison.OrdinalIgnoreCase))
                              ?? categoryNames.FirstOrDefault();
        }

        // Convert to ReceiptDto
        return new ReceiptDto
        {
            MerchantName = receiptData.MerchantName ?? "Unknown Merchant",
            TotalAmount = receiptData.TotalAmount,
            TransactionDate = ParseTransactionDate(receiptData.TransactionDate),
            SuggestedCategoryName = suggestedCategory,
            RawText = extractedText
        };
    }

    private DateTime? ParseTransactionDate(string? dateString)
    {
        if (string.IsNullOrEmpty(dateString))
            return null;

        if (DateTime.TryParse(dateString, out var date))
            return date;

        return null;
    }

    // Response models for Gemini API
    private class GeminiResponse
    {
        [JsonPropertyName("candidates")]
        public List<Candidate>? Candidates { get; set; }
    }

    private class Candidate
    {
        [JsonPropertyName("content")]
        public Content? Content { get; set; }
    }

    private class Content
    {
        [JsonPropertyName("parts")]
        public List<Part>? Parts { get; set; }
    }

    private class Part
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }

    // Receipt data model
    private class ReceiptData
    {
        [JsonPropertyName("merchantName")]
        public string? MerchantName { get; set; }

        [JsonPropertyName("totalAmount")]
        public decimal? TotalAmount { get; set; }

        [JsonPropertyName("transactionDate")]
        public string? TransactionDate { get; set; }

        [JsonPropertyName("suggestedCategory")]
        public string? SuggestedCategory { get; set; }

        [JsonPropertyName("items")]
        public List<ReceiptItem>? Items { get; set; }
    }

    private class ReceiptItem
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
    }
}
