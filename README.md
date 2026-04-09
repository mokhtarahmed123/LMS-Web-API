# 📚 LMS — Learning Management System

A full-featured Learning Management System built with **.NET 8** and **Clean Architecture**, supporting instructors, students, and admins with a complete e-learning experience.

---

## 🚀 Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend | ASP.NET Core 8 |
| Architecture | Clean Architecture + CQRS + MediatR |
| Database | SQL Server + Entity Framework Core |
| Caching | Redis |
| Authentication | ASP.NET Identity + JWT Bearer |
| Payment | Paymob |
| Real-time | SignalR |
| Background Jobs | Hangfire |
| Validation | FluentValidation |
| Logging | Serilog |
| Documentation | Swagger / OpenAPI |

---

## 📁 Project Structure

```
LMS/
├── LMS.PL/               # Presentation Layer (API Controllers)
├── LMS.Core/             # Application Layer (CQRS Handlers, Commands, Queries)
├── LMS.Infrastructure/   # Infrastructure Layer (Services, Caching, Email)
├── LMS.Data/             # Data Layer (Entities, DbContext, Repositories)
└── LMS.Service/          # Service Layer (Business Logic)
```

---

## ✨ Features

### 👨‍🏫 Instructor
- Create and manage courses, lessons, and quizzes
- Upload lesson files and videos
- View student enrollments and stats
- Apply coupons and manage pricing

### 👨‍🎓 Student
- Browse and enroll in courses
- Track lesson progress
- Take quizzes and view results
- Rate and favourite courses
- Manage subscriptions and payments

### 🛡️ Admin
- Approve/reject instructor profiles and courses
- Manage users, roles, and categories
- View payment history and subscriptions
- Full platform oversight

---

## 🔐 Authentication & Security

- JWT Bearer Token authentication
- Refresh Token rotation
- Token blacklisting on logout (Redis)
- Role-based authorization (`Admin`, `Instructor`, `Student`)
- Rate limiting on all endpoints

---

## 💳 Payment

Integrated with **Paymob** for:
- Course purchases
- Subscription plans
- Payment history tracking

---

## 📦 API Modules

| Module | Description |
|--------|-------------|
| `Auth` | Register, login, logout, email confirmation, password reset |
| `Course` | CRUD, approval flow, search, pagination |
| `Lesson` | CRUD, file upload, ordering |
| `Quiz` | Create quizzes, questions, options, submissions |
| `Categories` | Course categorization |
| `Coupons` | Discount management |
| `Subscriptions` | Plan management and renewals |
| `Payment` | Paymob integration and history |
| `InstructorProfile` | Profile requests and approvals |
| `UserCourse` | Enrollments, ratings, favourites |
| `Profile` | User profile management |
| `Roles` | Role assignment and management |

---

## ⚙️ Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server
- Redis
- Docker (optional)

### Run Redis with Docker
```bash
docker run -d -p 6379:6379 --name redis-lms redis
```

### Setup

```bash
# Clone the repo
git clone https://github.com/your-username/LMS.git
cd LMS

# Restore packages
dotnet restore

# Update appsettings.json with your connection strings
# Then run migrations
dotnet ef database update --project LMS.Data --startup-project LMS.PL

# Run the project
dotnet run --project LMS.PL
```

### Configuration — `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=LMS;Trusted_Connection=True;"
  },
  "JwtSettings": {
    "Key": "your-secret-key",
    "Issuer": "http://localhost:5184",
    "Audience": "http://localhost:4200",
    "DurationInDays": 1
  },
  "Redis": {
    "ConnectionString": "localhost:6379"
  },
  "Paymob": {
    "ApiKey": "your-paymob-api-key"
  }
}
```

---

## 📖 API Documentation

Once running, visit:
```
http://localhost:5184/swagger
```

---

## 🗄️ Database Schema

Key entities and their relationships:

```
Users ──── InstructorProfile ──── Courses ──── Lessons
                                     │
                                  Quizzes ──── QuizQuestions ──── QuestionOptions
                                     │
                                QuizSubmissions ──── SubmissionAnswers

Users ──── UserCourse (Enrollments, Ratings, Favourites)
Users ──── Subscriptions ──── Plans
Users ──── Payments
```

---

## 🔄 Rate Limiting

| Policy | Limit | Window |
|--------|-------|--------|
| Global | 10 requests | 1 minute |
| Fixed Window | 5 requests | 10 seconds |
| Token Bucket | 10 tokens | 10 seconds |
| Sliding Window | 4 requests | 10 seconds |

---

## 📝 License

This project is licensed under the MIT License.
