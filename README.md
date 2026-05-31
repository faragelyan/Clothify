# Clothify E-commerce Platform

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-blue?style=flat-square)
![Entity Framework Core](https://img.shields.io/badge/EF_Core-ORM-68217A?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL_Server-Database-CC292B?style=flat-square)
![Docker](https://img.shields.io/badge/Docker-Containerization-2496ED?style=flat-square&logo=docker&logoColor=white)

Clothify is a robust, scalable, and modern E-commerce backend designed to handle scalable online retail operations. Built on **ASP.NET Core** utilizing strict **Clean Architecture** principles, this API serves as the rock-solid foundation for advanced storefront applications, featuring secure user authentication, product management, seamless cart operations, and integrated payment gateways. The entire platform is fully containerized using **Docker** to ensure rapid, consistent deployments across any environment.

---

## Table of Contents

- [Core Features](#core-features)
- [Architecture Diagram](#architecture)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation & Configuration](#installation--configuration)
- [API Overview](#api-overview)

---

## Core Features

- **Maintainable Codebase:** Enforces Clean Architecture to guarantee absolute separation of concerns.
- **Containerized Ready:** Fully containerized using **Docker** for consistent, environment-agnostic deployments and effortless local setup.
- **Secure Authentication:** Leverages ASP.NET Core Identity with **JWT** (JSON Web Tokens) Bearer token authentication, complete with forgotten password and email verification workflows.
- **Comprehensive E-commerce Workflows:**
  - **Catalogue Management:** Full CRUD operations for Products, Brands, Sizes, and Categories.
  - **Shopping Cart & Checkout:** Persistent and dynamic quantity management leading to a seamless checkout process.
  - **Order Tracking:** Robust lifecycle tracking and history for user orders.
- **Payment Integration:** Out-of-the-box integration with the **Paymob** payment gateway, natively supporting mobile wallets.
- **Consistent Responses:** Implements a strict Result Pattern to standardize all API success and error responses.
- **Request Validation:** Employs **FluentValidation** to ensure all incoming data is clean, consistent, and strictly validated.

---

## Architecture

The project is structured according to Clean Architecture, ensuring that domain rules and business logic remain completely agnostic of UI components, external databases, or third-party frameworks:

### 1. `Clothify.Domain`
The core of the system. Contains fundamental Entities (User, Product, Brand, Order, etc.) and generic Interfaces (e.g., `IUnitOfWork`, `IGenericRepository`).

### 2. `Clothify.Application`
The business logic layer. Holds domain Services, Data Transfer Objects (DTOs), mapping profiles configurations (AutoMapper), validation rules (FluentValidation), and application-specific interfaces.

### 3. `Clothify.Infrastructure`
The implementation layer. Contains the concrete implementations of interfaces defined in Application/Domain layers. Handles Database Contexts (EF Core), configurations, SMTP services (MailKit), and 3rd-party Payment integrations (Paymob).

### 4. `Clothify.API`
The presentation layer. Exposes RESTful API endpoints securely to the public internet, hosts Swagger documentation, and manages high-level dependency injection setup.

---

## Technologies Used

| Category | Technology |
|---|---|
| **Framework** | .NET / ASP.NET Core Web API |
| **Language** | C# |
| **ORM** | Entity Framework Core |
| **Database** | Microsoft SQL Server |
| **Security** | ASP.NET Core Identity, JWT Verification |
| **Email Services** | MailKit |
| **Mappers & Validators** | AutoMapper, FluentValidation |
| **Payment Gateway** | Paymob SDK / REST API |
| **Documentation** | Swagger / OpenAPI |
| **Containerization** | **Docker** |

---

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (latest preview or LTS recommended)
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) 
- IDE of choice (Visual Studio, JetBrains Rider, or VS Code)

### Installation & Configuration

1. **Clone the repository:**
   ```bash
   git clone https://github.com/faragelyan/Clothify.git
   cd Clothify
   ```

2. **Configure App Settings:**
   Open `Clothify.API/appsettings.json` and adjust the variables to fit your local environment:
   - `ConnectionStrings.DefaultConnection`: Insert your SQL Server connection string.
   - `JWT`: Add a cryptographically secure `SigningKey` along with your `Issuer`, and `Audience`.
   - `SmtpSettings`: Supply valid SMTP configurations for email notifications.

3. **Apply EF Core Migrations:**
   Using the .NET CLI from the root directory, navigate to the API layer and update the database:
   ```bash
   cd Clothify.API
   dotnet ef database update --project ../Clothify.Infrastructure --startup-project .
   ```

4. **Boot the Application:**
   ```bash
   dotnet run
   ```
   *Tip: Navigate to `https://localhost:<port>/swagger` in your browser to access the live, interactive Swagger UI portal.*

### Running with Docker

If you prefer to run the application using Docker, a `Dockerfile` is provided for the API layer.

1. **Build the Docker Image:**
   From the root directory of the repository, execute the following command:
   ```bash
   docker build -t clothify-api -f Clothify.API/Dockerfile .
   ```

2. **Run the Docker Container:**
   Once the image is built, start a container. We recommend using an `.env` file to securely pass configurations like `ConnectionStrings__DefaultConnection` and `JWT__SigningKey`:
   ```bash
   docker run -d -p 8080:8080 --env-file .env --name clothify-api clothify-api
   ```
   *Note: If you don't use an `.env` file, you can pass variables directly using multiple `-e` flags.*

---

## API Overview

The backend exposes a wide array of functional endpoints logically grouped by controllers:

- **Authentication & User Management:**
  - `/api/Authentication`: Handling Login, User Registration, Token Refresh, and Password resets.
  - `/api/User`: Complete management of User profile details.
  - `/api/Address`: Managing user delivery and billing addresses.
  - `/api/UserPhone`: Handling associated phone numbers for users.
- **Catalogue & Inventory:**
  - `/api/Category`: Managing product categories.
  - `/api/Brand`: CRUD interface for item manufacturers.
  - `/api/Size`: Global catalogue of sizes.
  - `/api/Product`: Browsing, Filtering, creating and managing individual retail items.
  - `/api/ProductSize`: Managing size availability per product.
- **Shopping Cart:**
  - `/api/ShoppingCart`: Creating and managing user shopping carts.
  - `/api/CartItem`: Adding, updating, and removing specific items within a cart.
- **Orders & Checkout:**
  - `/api/Order`: Placing and tracking active shipment purchases.
  - `/api/OrderItem`: Exploring specific items within placed orders.
  - `/api/Payment`: Communicating Webhook calls from Paymob transactions.
- **Engagement & Feedback:**
  - `/api/Review`: User reviews and ratings for products.
  - `/api/Report`: Operations for reporting issues (e.g., inappropriate reviews or system issues).

*(Detailed schemas and models can be analyzed interactively within the Swagger UI).*

---

*Built utilizing comprehensive Clean Architecture concepts.*
