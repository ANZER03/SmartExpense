# Expense Tracker with OCR

A modern, full-stack expense tracking application designed to help users manage their finances efficiently. It features a comprehensive dashboard, detailed expense management, and an innovative OCR-based receipt scanning feature powered by LLMs.

## 🚀 Tech Stack

### Backend
- **Framework:** ASP.NET Core Web API (.NET 8/9)
- **Architecture:** Clean Architecture (3-Tier: API, Core, Infrastructure)
- **Database:** MySQL (via Entity Framework Core)
- **Authentication:** Cookie-based Session Authentication
- **Object Mapping:** AutoMapper
- **Documentation:** OpenAPI / Swagger

### Frontend
- **Framework:** Angular 19+
- **Styling:** Tailwind CSS 3.4
- **Charts:** Chart.js & ng2-charts
- **HTTP Client:** Angular HttpClient

## 🏗️ Architecture

The project follows a strict **Clean Architecture** to ensure separation of concerns and maintainability:

### Backend Structure
- **`ExpenseTracker.Core`**: Contains the domain entities, DTOs, and interfaces. This layer has no dependencies on other layers.
- **`ExpenseTracker.Infrastructure`**: Implements the interfaces defined in Core. Handles database access (EF Core), external services (LLM OCR), and authentication logic.
- **`ExpenseTracker.API`**: The entry point of the application. Contains Controllers that expose endpoints and handle HTTP requests.

### Frontend Structure
- **`app/components`**: Reusable UI components (e.g., Navbar, Sidebar, ExpenseForm).
- **`app/pages`**: Main view components (e.g., Dashboard, Login, Register).
- **`app/services`**: Angular services for API communication.
- **`app/guards`**: Route guards for authentication.

## ✨ Features

- **User Authentication**: Secure login and registration using HTTP-only cookies.
- **Dashboard**: Visual overview of expenses with:
  - Total spending summary.
  - Monthly budget tracking.
  - Bar charts for recent spending trends.
  - Doughnut charts for category-wise breakdown.
- **Expense Management**:
  - Add, Edit, and Delete expenses.
  - Categorize expenses with color-coded tags.
  - Filter and sort expense lists.
- **OCR Receipt Scanning**:
  - Upload receipt images.
  - AI-powered extraction of Date, Amount, and Vendor.
  - Auto-fill expense forms with extracted data.

## 🛠️ Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (8.0 or later)
- [Node.js](https://nodejs.org/) (LTS version)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)

### Installation

1.  **Clone the repository:**
    ```bash
    git clone <repository-url>
    cd budget_tracker
    ```

2.  **Backend Setup:**
    - Navigate to the API directory:
      ```bash
      cd backend/src/ExpenseTracker.API
      ```
    - Update `appsettings.json` with your MySQL connection string.
    - Apply database migrations:
      ```bash
      dotnet ef database update
      ```
    - Run the backend:
      ```bash
      dotnet run
      ```
    - The API will be available at `http://localhost:5054` (or similar).

3.  **Frontend Setup:**
    - Navigate to the frontend directory:
      ```bash
      cd frontend
      ```
    - Install dependencies:
      ```bash
      npm install
      ```
    - Run the application:
      ```bash
      npm start
      ```
    - Open your browser and navigate to `http://localhost:4200`.
