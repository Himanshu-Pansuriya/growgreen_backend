# GrowGreen Backend

GrowGreen Backend is the core API for the GrowGreen platform, connecting farmers, buyers, and admins. It provides functionality for managing crops, pesticides, users, orders, and other essential features for the agriculture-focused application.

---

## Features

- **User Management**: CRUD operations for farmers, buyers, and admins.
- **Crop Management**: APIs for selling crops, crop suitability testing, and buying crops.
- **Pesticide Management**: Manage pesticides, including image uploads to Cloudinary.
- **Order Management**: Handle orders for crops and pesticides.
- **Admin Features**: Admin-specific APIs for managing platform operations.
- **Cloudinary Integration**: Securely upload and manage images for entities like pesticides.

---

## Technologies Used

- **.NET Core 7**: Backend framework.
- **Entity Framework Core**: Database management.
- **SQL Server**: Primary database.
- **Swagger**: API documentation and testing.
- **Cloudinary**: Image hosting and management.
- **JWT Authentication**: Secure API access.
- **Dependency Injection**: For better code maintainability.

---

## Installation

### Prerequisites

- .NET Core SDK 7.0 or later
- SQL Server
- Visual Studio or VS Code
- Cloudinary account (for image uploads)

### Steps

1. **Clone the repository**:
   ```bash
   git clone https://github.com/your-username/growgreen-backend.git
   cd growgreen-backend
