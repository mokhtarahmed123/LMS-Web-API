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



<img width="1275" height="443" alt="Screenshot 2026-04-09 013708" src="https://github.com/user-attachments/assets/9aadeb0f-c8bb-4deb-a9e7-e76d8793a223" />
<img width="1526" height="361" alt="Screenshot 2026-04-09 013806" src="https://github.com/user-attachments/assets/dbe6534c-bd07-4e76-ab8f-dea3a0fa714b" />
<img width="1294" height="715" alt="Screenshot 2026-04-09 013916" src="https://github.com/user-attachments/assets/c8f226cc-bfc3-4a03-97d1-ef3c038500c2" />

<img width="1421" height="717" alt="Screenshot 2026-04-09 013949" src="https://github.com/user-attachments/assets/5794cd7c-8349-4f24-ae35-8f88c44d8868" />

<img width="1313" height="722" alt="Screenshot 2026-04-09 014112" src="https://github.com/user-attachments/assets/e32de1c3-b46b-45dd-b057-5732d25db489" />

<img width="1512" height="643" alt="Screenshot 2026-04-09 014154" src="https://github.com/user-attachments/assets/c1178d86-c99a-4166-bc6f-83a3c1c421ad" />


<img width="1251" height="848" alt="Screenshot 2026-04-09 014016" src="https://github.com/user-attachments/assets/b4986c38-9fba-4aac-bace-0aec51e545f3" />

<img width="1637" height="463" alt="Screenshot 2026-04-09 014045" src="https://github.com/user-attachments/assets/3fc457d7-2de3-49ec-b926-bbcb9d72bbdd" />


<img width="1524" height="719" alt="Screenshot 2026-04-09 014225" src="https://github.com/user-attachments/assets/5572cdf1-2799-42cd-bd86-7e20ad94d01d" />

<img width="1600" height="355" alt="Screenshot 2026-04-09 014250" src="https://github.com/user-attachments/assets/1e3f50dc-256f-4fb3-bba9-20b0f397840d" />


<img width="1497" height="761" alt="Screenshot 2026-04-09 014315" src="https://github.com/user-attachments/assets/8a985d7b-92b5-4364-a3b9-8cd2f21289c4" />








<img width="1058" height="868" alt="Screenshot 2026-04-09 014729" src="https://github.com/user-attachments/assets/43ba9552-6e11-4bb1-9351-708f6e3b439d" />

