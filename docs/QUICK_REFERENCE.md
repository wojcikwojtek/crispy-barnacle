# Quick Reference Guide

Quick reference for common tasks and commands in the Crispy Barnacle project.

## Table of Contents

- [Getting Started](#getting-started)
- [Building & Running](#building--running)
- [API Testing](#api-testing)
- [Common Commands](#common-commands)
- [Troubleshooting](#troubleshooting)

---

## Getting Started

### Clone and Setup

```bash
git clone https://github.com/wojcikwojtek/crispy-barnacle.git
cd crispy-barnacle/SampleApp
dotnet restore
dotnet build
```

### Run Application

```bash
# Terminal 1 - Backend
cd SampleApp/BackEnd
dotnet run

# Terminal 2 - Frontend  
cd SampleApp/FrontEnd
dotnet run
```

### Access Points

- **Frontend UI**: http://localhost:5002
- **API Documentation**: http://localhost:5001/scalar
- **Backend API**: http://localhost:5001

---

## Building & Running

### Build Commands

```bash
# Build entire solution
cd SampleApp
dotnet build

# Build specific project
dotnet build BackEnd/BackEnd.csproj
dotnet build FrontEnd/FrontEnd.csproj

# Clean build
dotnet clean
dotnet build --no-incremental
```

### Run Commands

```bash
# Run with hot reload
dotnet watch run

# Run on specific port
dotnet run --urls="http://localhost:6001"

# Run in production mode
dotnet run --environment Production
```

---

## API Testing

### Using Scalar UI

1. Navigate to http://localhost:5001/scalar
2. Select endpoint from list
3. Click "Test Request"
4. Fill parameters
5. Click "Send"

### Using cURL

#### Weather Forecast

```bash
curl http://localhost:5001/weatherforecast
```

#### Products

```bash
# Get all products
curl http://localhost:5001/api/products

# Get product by ID
curl http://localhost:5001/api/products/1

# Create product
curl -X POST http://localhost:5001/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Tablet",
    "description": "10-inch tablet",
    "price": 499.99,
    "stockQuantity": 20
  }'

# Update product
curl -X PUT http://localhost:5001/api/products/1 \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Updated Laptop",
    "description": "Updated description",
    "price": 1399.99,
    "stockQuantity": 5
  }'

# Delete product
curl -X DELETE http://localhost:5001/api/products/1
```

#### Users

```bash
# Get all users
curl http://localhost:5001/api/users

# Create user
curl -X POST http://localhost:5001/api/users \
  -H "Content-Type: application/json" \
  -d '{
    "username": "newuser",
    "email": "user@example.com",
    "passwordHash": "hashed_password"
  }'
```

#### Orders

```bash
# Get all orders
curl http://localhost:5001/api/orders

# Create order
curl -X POST http://localhost:5001/api/orders \
  -H "Content-Type: application/json" \
  -d '{
    "customerName": "John Doe",
    "customerEmail": "john@example.com",
    "items": [
      {
        "productId": 1,
        "productName": "Laptop",
        "quantity": 1,
        "unitPrice": 1299.99
      }
    ]
  }'

# Update order status
curl -X PUT http://localhost:5001/api/orders/1/status \
  -H "Content-Type: application/json" \
  -d '"Confirmed"'
```

#### Contact

```bash
curl -X POST http://localhost:5001/api/contact \
  -H "Content-Type: application/json" \
  -d '{
    "name": "John Doe",
    "email": "john@example.com",
    "message": "This is a test message"
  }'
```

---

## Common Commands

### .NET CLI

```bash
# Check .NET version
dotnet --version

# List installed SDKs
dotnet --list-sdks

# Create new project
dotnet new webapi -n MyApi
dotnet new blazorserver -n MyApp

# Add package
dotnet add package PackageName

# List packages
dotnet list package

# Restore dependencies
dotnet restore

# Clean artifacts
dotnet clean

# Run tests (when available)
dotnet test
```

### Git Commands

```bash
# Create feature branch
git checkout -b feature/my-feature

# Check status
git status

# Stage changes
git add .

# Commit
git commit -m "Add: New feature"

# Push
git push origin feature/my-feature

# Update from main
git checkout main
git pull
git checkout feature/my-feature
git merge main
```

### VS Code

```bash
# Open in VS Code
code .

# Run and Debug
F5

# Stop debugging
Shift+F5

# Build
Ctrl+Shift+B (Windows/Linux)
Cmd+Shift+B (Mac)
```

---

## Troubleshooting

### Port Already in Use

```bash
# Find process on port (Linux/Mac)
lsof -i :5001
kill -9 <PID>

# Find process on port (Windows)
netstat -ano | findstr :5001
taskkill /PID <PID> /F
```

### Build Failures

```bash
# Clean and rebuild
dotnet clean
rm -rf bin/ obj/
dotnet restore
dotnet build
```

### Package Issues

```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore --force
```

### Frontend Not Connecting

1. Check backend is running: http://localhost:5001/weatherforecast
2. Verify `appsettings.json` in Frontend:
   ```json
   {
     "WEATHER_URL": "http://localhost:5001",
     "API_URL": "http://localhost:5001"
   }
   ```
3. Check browser console for errors (F12)
4. Clear browser cache

### Hot Reload Not Working

```bash
# Restart with watch
Ctrl+C
dotnet watch run
```

---

## Quick Links

- **Documentation**
  - [README](readme.md)
  - [API Documentation](docs/API.md)
  - [Architecture Guide](docs/ARCHITECTURE.md)
  - [Development Guide](docs/DEVELOPMENT.md)
  - [Contributing Guide](CONTRIBUTING.md)

- **External Resources**
  - [.NET Documentation](https://docs.microsoft.com/dotnet/)
  - [ASP.NET Core](https://docs.microsoft.com/aspnet/core/)
  - [Blazor](https://docs.microsoft.com/aspnet/core/blazor/)
  - [Scalar Documentation](https://github.com/scalar/scalar)

---

## Development Workflow Cheat Sheet

### Starting Development

```bash
1. git pull                           # Get latest changes
2. git checkout -b feature/name       # Create feature branch
3. cd SampleApp/BackEnd && dotnet watch run   # Start backend
4. cd SampleApp/FrontEnd && dotnet watch run  # Start frontend
5. # Make changes...
6. # Test in browser and Scalar
7. git add .                          # Stage changes
8. git commit -m "Add: Description"   # Commit
9. git push origin feature/name       # Push
10. # Create PR on GitHub
```

### Testing Workflow

```bash
1. Navigate to http://localhost:5001/scalar
2. Test all affected endpoints
3. Navigate to http://localhost:5002
4. Test UI functionality
5. Check browser console (F12) for errors
6. Test on different screen sizes
```

### Before PR Submission

```bash
☐ dotnet build                        # Successful build
☐ Test in Scalar                      # API works
☐ Test in browser                     # UI works
☐ Check console for errors            # No errors
☐ Update documentation                # If needed
☐ Clear commit messages                # Good descriptions
☐ Feature branch up to date            # Merged main
```

---

## Environment Variables

### Backend

```bash
# Development
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://localhost:5001

# Production
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=https://+:443;http://+:80
```

### Frontend

```bash
# Development
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://localhost:5002

# API Configuration (in appsettings.json)
WEATHER_URL=http://localhost:5001
API_URL=http://localhost:5001
```

---

## Need More Help?

- Check [Development Guide](docs/DEVELOPMENT.md)
- Review [API Documentation](docs/API.md)
- See [Architecture Guide](docs/ARCHITECTURE.md)
- Open an issue on GitHub
