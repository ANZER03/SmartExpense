# Gemini OCR Service - Implementation Guide

## 🎯 Overview

The Gemini OCR service has been successfully implemented using Google's Gemini 2.0 Flash model with structured JSON output for receipt scanning.

## 🔧 Implementation Details

### Service Architecture
- **Service**: `LlmOcrService.cs`
- **API Model**: Gemini 2.0 Flash Experimental
- **Output Format**: Structured JSON with schema validation
- **Endpoint**: `POST /api/receipts/scan`

### Features
- ✅ Base64 image encoding
- ✅ Multiple image format support (JPEG, PNG, WebP)
- ✅ Structured JSON response schema
- ✅ Automatic data extraction:
  - Merchant name
  - Total amount
  - Transaction date
  - Line items (optional)

### Configuration Required

**Add your Gemini API key to `appsettings.json`:**

```json
{
  "Gemini": {
    "ApiKey": "YOUR_GEMINI_API_KEY_HERE"
  }
}
```

**Get your API key from:** https://aistudio.google.com/app/apikey

## 📝 API Request Format

### Endpoint
```
POST /api/receipts/scan
Authorization: Required (Cookie-based session)
Content-Type: multipart/form-data
```

### Request Body
```
file: <image file> (JPEG, PNG, or WebP)
```

### Response Format
```json
{
  "merchantName": "Store Name",
  "totalAmount": 123.45,
  "transactionDate": "2025-11-20T00:00:00Z",
  "rawText": "{...full JSON response from Gemini...}"
}
```

## 🧪 Testing the OCR Service

### Method 1: Using Postman or Similar Tools

1. **Login first** to get authentication cookie
   ```
   POST http://localhost:5273/api/auth/login
   Content-Type: application/json
   
   {
     "email": "test@example.com",
     "password": "Test123!"
   }
   ```

2. **Upload receipt image**
   ```
   POST http://localhost:5273/api/receipts/scan
   Content-Type: multipart/form-data
   
   file: [select your receipt image]
   ```

### Method 2: Using cURL (PowerShell)

```powershell
# Login first
$loginBody = @{email='test@example.com';password='Test123!'} | ConvertTo-Json
$session = New-Object Microsoft.PowerShell.Commands.WebRequestSession
Invoke-WebRequest -Uri 'http://localhost:5273/api/auth/login' -Method POST -Body $loginBody -ContentType 'application/json' -SessionVariable session

# Upload receipt
$form = @{
    file = Get-Item -Path 'C:\path\to\receipt.jpg'
}
Invoke-WebRequest -Uri 'http://localhost:5273/api/receipts/scan' -Method POST -Form $form -WebSession $session
```

### Method 3: Using C# Test Code

```csharp
using var client = new HttpClient();
using var form = new MultipartFormDataContent();
using var fileStream = File.OpenRead("receipt.jpg");
using var fileContent = new StreamContent(fileStream);

fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
form.Add(fileContent, "file", "receipt.jpg");

var response = await client.PostAsync("http://localhost:5273/api/receipts/scan", form);
var result = await response.Content.ReadAsStringAsync();
```

## 🔍 How It Works

1. **Image Upload**: Client sends receipt image via multipart form
2. **Base64 Encoding**: Service converts image to base64 string
3. **Gemini API Call**: Sends image + prompt to Gemini with structured schema
4. **JSON Parsing**: Extracts structured data from Gemini response
5. **DTO Mapping**: Maps to `ReceiptDto` and returns to client

### Gemini Prompt
The service uses this prompt for extraction:
```
Analyze this receipt image and extract the following information in JSON format:
- merchantName: The name of the store/merchant
- totalAmount: The total amount paid (as a decimal number)
- transactionDate: The date of the transaction (in ISO 8601 format: YYYY-MM-DD)
- items: Array of items purchased (optional, if visible)

If any field is not clearly visible, use null for that field.
Return ONLY valid JSON, no additional text.
```

### Response Schema
The service enforces this JSON schema:
```json
{
  "type": "object",
  "properties": {
    "merchantName": { "type": "string" },
    "totalAmount": { "type": "number" },
    "transactionDate": { "type": "string" },
    "items": {
      "type": "array",
      "items": {
        "type": "object",
        "properties": {
          "name": { "type": "string" },
          "price": { "type": "number" }
        }
      }
    }
  },
  "required": ["merchantName", "totalAmount", "transactionDate"]
}
```

## ⚙️ Code Structure

### Main Service Class
```csharp
public class LlmOcrService : IOcrService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    
    public async Task<ReceiptDto> ScanReceiptAsync(Stream imageStream, string contentType)
    {
        // 1. Get API key from configuration
        // 2. Convert image to base64
        // 3. Build Gemini API request with structured schema
        // 4. Send request to Gemini
        // 5. Parse and validate response
        // 6. Return ReceiptDto
    }
}
```

### DI Registration
```csharp
builder.Services.AddScoped<IOcrService, LlmOcrService>();
builder.Services.AddHttpClient<LlmOcrService>();
```

## 🚨 Error Handling

The service handles these error cases:
- ❌ Missing API key → Exception with configuration message
- ❌ Invalid image format → Defaults to JPEG
- ❌ Gemini API error → Exception with status code and message
- ❌ Empty response → Exception
- ❌ Invalid JSON → Exception with parsing error

## 📊 Supported Image Formats

- ✅ JPEG/JPG (`image/jpeg`)
- ✅ PNG (`image/png`)
- ✅ WebP (`image/webp`)

## 🔐 Security Notes

- API key stored in `appsettings.json` (use User Secrets in production)
- Endpoint requires authentication (`[Authorize]` attribute)
- Only authenticated users can scan receipts
- Images are not stored on server (processed in memory)

## 🎯 Next Steps

1. **Add your Gemini API key** to `appsettings.json`
2. **Test with sample receipt images**
3. **Integrate with expense creation workflow**
4. **Add receipt image storage** (optional)
5. **Implement retry logic** for API failures
6. **Add rate limiting** for API calls

## 📚 Resources

- [Gemini API Documentation](https://ai.google.dev/docs)
- [Get API Key](https://aistudio.google.com/app/apikey)
- [Structured Output Guide](https://ai.google.dev/gemini-api/docs/structured-output)
