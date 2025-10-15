# Development Guide

[English](#english) | [Polski](#polski)

---

## English

## Development Setup

This guide covers everything you need to know to develop and contribute to the Crispy Barnacle project.

### Prerequisites

#### Required Software

- **.NET 9.0 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Download](https://git-scm.com/)

#### Recommended IDEs

- **Visual Studio Code** (Recommended)
  - [Download](https://code.visualstudio.com/)
  - Extensions:
    - C# Dev Kit
    - C#
    - .NET Extension Pack
    - GitLens

- **Visual Studio 2022** (Alternative)
  - [Download](https://visualstudio.microsoft.com/)
  - Version 17.8 or later
  - ASP.NET and web development workload

- **JetBrains Rider** (Alternative)
  - [Download](https://www.jetbrains.com/rider/)

### Getting Started

#### 1. Clone the Repository

```bash
git clone https://github.com/wojcikwojtek/crispy-barnacle.git
cd crispy-barnacle
```

#### 2. Verify .NET Installation

```bash
dotnet --version
# Should output: 9.0.xxx or higher
```

#### 3. Restore Dependencies

```bash
cd SampleApp
dotnet restore
```

#### 4. Build the Solution

```bash
dotnet build
```

You should see:
```
Build succeeded in X.Xs
```

### Running the Application

#### Option 1: Using VS Code (Recommended)

1. Open the project in VS Code
2. Press `F5` or click "Run and Debug"
3. Select "Run All" configuration
4. Both backend and frontend will start

#### Option 2: Manual Terminal Commands

**Terminal 1 - Backend:**
```bash
cd SampleApp/BackEnd
dotnet run
```

Output:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

**Terminal 2 - Frontend:**
```bash
cd SampleApp/FrontEnd
dotnet run
```

Output:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5002
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

#### Option 3: Using Watch Mode (Hot Reload)

For automatic rebuilding on file changes:

**Backend:**
```bash
cd SampleApp/BackEnd
dotnet watch run
```

**Frontend:**
```bash
cd SampleApp/FrontEnd
dotnet watch run
```

### Project Structure

```
crispy-barnacle/
├── .devcontainer/          # Dev container configuration
├── .github/                # GitHub workflows and settings
├── .vscode/                # VS Code configuration
│   ├── launch.json        # Debug configurations
│   ├── tasks.json         # Build tasks
│   └── settings.json      # Workspace settings
├── docs/                   # Documentation
│   ├── API.md             # API documentation
│   ├── ARCHITECTURE.md    # Architecture guide
│   └── DEVELOPMENT.md     # This file
├── images/                 # Screenshots and images
├── SampleApp/              # Main application
│   ├── BackEnd/           # Backend API project
│   │   ├── Models/        # Domain models
│   │   │   ├── Dto/      # Data Transfer Objects
│   │   │   └── Validation/ # Validation logic
│   │   ├── Properties/    # Assembly info
│   │   ├── Program.cs     # Application entry point
│   │   ├── BackEnd.csproj # Project file
│   │   ├── appsettings.json
│   │   └── appsettings.Development.json
│   ├── FrontEnd/          # Frontend Blazor project
│   │   ├── Data/          # HTTP clients and models
│   │   ├── Pages/         # Razor components
│   │   ├── Shared/        # Shared components
│   │   ├── wwwroot/       # Static files
│   │   ├── Properties/    # Assembly info
│   │   ├── Program.cs     # Application entry point
│   │   ├── FrontEnd.csproj # Project file
│   │   ├── App.razor      # App component
│   │   ├── _Imports.razor # Global imports
│   │   ├── appsettings.json
│   │   └── appsettings.Development.json
│   └── SampleApp.sln      # Solution file
├── readme.md               # Main README
└── .gitignore             # Git ignore rules
```

### Development Workflow

#### Making Changes

1. **Create a Feature Branch**
   ```bash
   git checkout -b feature/my-new-feature
   ```

2. **Make Your Changes**
   - Edit code in your IDE
   - Save files
   - Hot reload will apply changes automatically

3. **Test Your Changes**
   - Access the frontend at `http://localhost:5002`
   - Test API at `http://localhost:5001/scalar`
   - Verify functionality

4. **Build and Verify**
   ```bash
   dotnet build
   ```

5. **Commit Your Changes**
   ```bash
   git add .
   git commit -m "Add: Description of changes"
   ```

6. **Push to GitHub**
   ```bash
   git push origin feature/my-new-feature
   ```

7. **Create Pull Request**
   - Go to GitHub repository
   - Click "New Pull Request"
   - Select your branch
   - Add description
   - Submit

### Adding New Features

#### Adding a New API Endpoint

1. **Open `BackEnd/Program.cs`**

2. **Add Your Endpoint**
   ```csharp
   app.MapGet("/api/myendpoint", () =>
   {
       return Results.Ok(new { message = "Hello World" });
   })
   .WithName("GetMyEndpoint")
   .WithDescription("Description of what this endpoint does")
   .WithOpenApi()
   .WithTags("MyTag");
   ```

3. **Test in Scalar**
   - Navigate to `/scalar`
   - Find your new endpoint
   - Click "Test Request"

#### Adding a New Model

1. **Create Model File**
   ```bash
   # In SampleApp/BackEnd/Models/
   touch MyModel.cs
   ```

2. **Define Your Model**
   ```csharp
   namespace BackEnd.Models;

   /// <summary>
   /// Description of your model
   /// </summary>
   public class MyModel
   {
       /// <summary>
       /// Property description
       /// </summary>
       public int Id { get; set; }
       
       public string Name { get; set; } = string.Empty;
   }
   ```

3. **Create DTO (if needed)**
   ```bash
   # In SampleApp/BackEnd/Models/Dto/
   touch MyModelDto.cs
   ```

4. **Add Conversion Method**
   ```csharp
   public MyModelDto ToDto()
   {
       return new MyModelDto
       {
           Id = Id,
           Name = Name
       };
   }
   ```

#### Adding a New Blazor Page

1. **Create Razor File**
   ```bash
   # In SampleApp/FrontEnd/Pages/
   touch MyPage.razor
   ```

2. **Define Your Page**
   ```razor
   @page "/mypage"
   @using FrontEnd.Data

   <PageTitle>My Page</PageTitle>

   <h3>My New Page</h3>

   <p>Content goes here</p>

   @code {
       protected override async Task OnInitializedAsync()
       {
           // Initialization code
       }
   }
   ```

3. **Add to Navigation (Optional)**
   - Edit `Shared/NavMenu.razor`
   - Add menu item

#### Adding a New HTTP Client

1. **Create Client Class**
   ```bash
   # In SampleApp/FrontEnd/Data/
   touch MyClient.cs
   ```

2. **Implement Client**
   ```csharp
   using System.Net.Http.Json;

   namespace FrontEnd.Data;

   public class MyClient
   {
       private readonly HttpClient _httpClient;

       public MyClient(HttpClient httpClient)
       {
           _httpClient = httpClient;
       }

       public async Task<MyModel[]> GetDataAsync()
       {
           return await _httpClient.GetFromJsonAsync<MyModel[]>("api/myendpoint") 
               ?? Array.Empty<MyModel>();
       }
   }
   ```

3. **Register in Program.cs**
   ```csharp
   builder.Services.AddHttpClient<MyClient>(c =>
   {
       var apiUrl = builder.Configuration["API_URL"] 
           ?? throw new InvalidOperationException("API_URL is not set");
       c.BaseAddress = new Uri(apiUrl);
   });
   ```

### Configuration

#### Backend Configuration

**appsettings.json** (Checked into source control):
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**appsettings.Development.json** (Checked into source control):
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  }
}
```

#### Frontend Configuration

**appsettings.json**:
```json
{
  "WEATHER_URL": "http://localhost:5001",
  "API_URL": "http://localhost:5001"
}
```

**For GitHub Codespaces:**
The URLs are automatically adjusted for forwarded ports.

### Debugging

#### VS Code Debugging

1. **Set Breakpoints**
   - Click in the gutter next to line numbers
   - Red dot indicates breakpoint

2. **Start Debugging**
   - Press `F5`
   - Or click "Run and Debug"

3. **Debug Tools**
   - Step Over: `F10`
   - Step Into: `F11`
   - Step Out: `Shift+F11`
   - Continue: `F5`

#### Browser DevTools

For frontend debugging:
1. Open browser DevTools (`F12`)
2. Check Console for errors
3. Use Network tab for API calls
4. Use Application tab for storage

### Common Development Tasks

#### Clean Build

```bash
cd SampleApp
dotnet clean
dotnet build
```

#### Clear All Build Artifacts

```bash
cd SampleApp
# Remove bin and obj directories
find . -type d -name "bin" -o -name "obj" | xargs rm -rf
dotnet restore
dotnet build
```

#### Update NuGet Packages

```bash
cd SampleApp
dotnet list package --outdated
dotnet add package PackageName --version X.Y.Z
```

#### Check for Code Issues

```bash
cd SampleApp
dotnet format --verify-no-changes
```

#### Run with Different Port

**Backend:**
```bash
cd SampleApp/BackEnd
dotnet run --urls="http://localhost:6001"
```

**Frontend:**
```bash
cd SampleApp/FrontEnd
dotnet run --urls="http://localhost:6002"
```

### Coding Standards

#### C# Style Guidelines

Follow the project's C# guidelines in `.github/csharp-guidelines.md`:

- Use nullable reference types
- Use implicit usings
- Follow async/await best practices
- Use meaningful variable names
- Add XML documentation comments
- Use record types for DTOs where appropriate

#### Blazor Guidelines

Follow the project's Blazor guidelines in `.github/blazor-guidelines.md`:

- Component naming conventions
- Parameter validation
- State management
- Event handling

#### Git Commit Messages

Follow conventional commits format:

```
Add: New feature or capability
Fix: Bug fix
Update: Changes to existing functionality
Docs: Documentation changes
Refactor: Code refactoring
Test: Adding or updating tests
```

### Testing

#### Manual Testing

1. **Test Backend APIs**
   - Use Scalar UI at `/scalar`
   - Test all CRUD operations
   - Verify error handling

2. **Test Frontend**
   - Navigate all pages
   - Submit forms
   - Check responsive design

#### API Testing with cURL

```bash
# Get weather forecast
curl http://localhost:5001/weatherforecast

# Create product
curl -X POST http://localhost:5001/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"Test","description":"Test","price":10.0,"stockQuantity":5}'

# Get all products
curl http://localhost:5001/api/products
```

### Troubleshooting

#### Port Already in Use

```bash
# Find process using port 5001
lsof -i :5001
# or on Windows
netstat -ano | findstr :5001

# Kill the process
kill -9 <PID>
```

#### Build Errors

```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

#### Frontend Not Connecting to Backend

1. Check backend is running
2. Verify `API_URL` in `appsettings.json`
3. Check browser console for CORS errors
4. Verify ports match configuration

#### Hot Reload Not Working

```bash
# Restart with watch
dotnet watch run
```

### GitHub Codespaces

#### Opening in Codespaces

1. Click "Code" button on GitHub
2. Select "Codespaces" tab
3. Click "Create codespace on main"
4. Wait for environment to build

#### Port Forwarding

Codespaces automatically forwards ports:
- Backend API
- Frontend App
- Any additional ports

Access via the "Ports" tab in VS Code.

### Contributing Guidelines

1. **Fork the Repository**
2. **Create Feature Branch**
3. **Make Changes**
4. **Test Thoroughly**
5. **Submit Pull Request**
6. **Respond to Review Feedback**

### Resources

- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)
- [Blazor Documentation](https://docs.microsoft.com/aspnet/core/blazor/)
- [C# Language Reference](https://docs.microsoft.com/dotnet/csharp/)

---

## Polski

## Konfiguracja Środowiska Deweloperskiego

Ten przewodnik obejmuje wszystko, co musisz wiedzieć, aby rozwijać i współtworzyć projekt Crispy Barnacle.

### Wymagania Wstępne

#### Wymagane Oprogramowanie

- **.NET 9.0 SDK** - [Pobierz](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Pobierz](https://git-scm.com/)

#### Zalecane IDE

- **Visual Studio Code** (Zalecane)
  - [Pobierz](https://code.visualstudio.com/)
  - Rozszerzenia:
    - C# Dev Kit
    - C#
    - .NET Extension Pack
    - GitLens

- **Visual Studio 2022** (Alternatywa)
  - [Pobierz](https://visualstudio.microsoft.com/)
  - Wersja 17.8 lub nowsza
  - Obciążenie: ASP.NET i tworzenie aplikacji webowych

### Rozpoczęcie Pracy

#### 1. Sklonuj Repozytorium

```bash
git clone https://github.com/wojcikwojtek/crispy-barnacle.git
cd crispy-barnacle
```

#### 2. Zweryfikuj Instalację .NET

```bash
dotnet --version
# Powinno wyświetlić: 9.0.xxx lub wyżej
```

#### 3. Przywróć Zależności

```bash
cd SampleApp
dotnet restore
```

#### 4. Zbuduj Rozwiązanie

```bash
dotnet build
```

Powinno się pojawić:
```
Build succeeded in X.Xs
```

### Uruchamianie Aplikacji

Szczegółowe instrukcje znajdują się w sekcji angielskiej powyżej.

### Struktura Projektu

Szczegółowa struktura znajduje się w sekcji angielskiej powyżej.

### Przepływ Pracy Deweloperskiej

#### Wprowadzanie Zmian

1. **Utwórz Gałąź Funkcjonalności**
   ```bash
   git checkout -b feature/moja-nowa-funkcjonalnosc
   ```

2. **Wprowadź Zmiany**
   - Edytuj kod w swoim IDE
   - Zapisz pliki
   - Hot reload automatycznie zastosuje zmiany

3. **Przetestuj Zmiany**
   - Dostęp do frontendu: `http://localhost:5002`
   - Testuj API: `http://localhost:5001/scalar`
   - Zweryfikuj funkcjonalność

4. **Zbuduj i Zweryfikuj**
   ```bash
   dotnet build
   ```

5. **Zatwierdź Zmiany**
   ```bash
   git add .
   git commit -m "Dodaj: Opis zmian"
   ```

### Dodawanie Nowych Funkcji

Szczegółowe instrukcje dotyczące dodawania:
- Nowych endpointów API
- Nowych modeli
- Nowych stron Blazor
- Nowych klientów HTTP

znajdują się w sekcji angielskiej powyżej.

### Konfiguracja

Szczegółowe informacje o konfiguracji znajdują się w sekcji angielskiej powyżej.

### Debugowanie

#### Debugowanie w VS Code

1. **Ustaw Punkty Przerwania**
   - Kliknij w odstęp obok numerów linii
   - Czerwona kropka wskazuje punkt przerwania

2. **Rozpocznij Debugowanie**
   - Naciśnij `F5`
   - Lub kliknij "Run and Debug"

### Standardy Kodowania

#### Wytyczne Stylu C#

Przestrzegaj wytycznych C# projektu w `.github/csharp-guidelines.md`:

- Używaj nullable reference types
- Używaj implicit usings
- Przestrzegaj najlepszych praktyk async/await
- Używaj znaczących nazw zmiennych
- Dodawaj komentarze dokumentacji XML

#### Komunikaty Commitów Git

Przestrzegaj formatu conventional commits:

```
Add: Nowa funkcjonalność lub możliwość
Fix: Naprawa błędu
Update: Zmiany w istniejącej funkcjonalności
Docs: Zmiany w dokumentacji
Refactor: Refaktoryzacja kodu
Test: Dodawanie lub aktualizacja testów
```

### Testowanie

#### Testowanie Ręczne

1. **Testuj API Backendu**
   - Użyj interfejsu Scalar pod `/scalar`
   - Testuj wszystkie operacje CRUD
   - Zweryfikuj obsługę błędów

2. **Testuj Frontend**
   - Nawiguj po wszystkich stronach
   - Wysyłaj formularze
   - Sprawdź responsywny design

### Rozwiązywanie Problemów

#### Port Już Używany

```bash
# Znajdź proces używający portu 5001
lsof -i :5001
# lub w Windows
netstat -ano | findstr :5001

# Zabij proces
kill -9 <PID>
```

#### Błędy Budowania

```bash
# Wyczyść i przebuduj
dotnet clean
dotnet restore
dotnet build
```

### Wytyczne Współtworzenia

1. **Fork Repozytorium**
2. **Utwórz Gałąź Funkcjonalności**
3. **Wprowadź Zmiany**
4. **Przetestuj Dokładnie**
5. **Wyślij Pull Request**
6. **Odpowiadaj na Feedback z Przeglądu**

### Zasoby

- [Dokumentacja .NET](https://docs.microsoft.com/dotnet/)
- [Dokumentacja ASP.NET Core](https://docs.microsoft.com/aspnet/core/)
- [Dokumentacja Blazor](https://docs.microsoft.com/aspnet/core/blazor/)
- [Dokumentacja Języka C#](https://docs.microsoft.com/dotnet/csharp/)
