# Contributing to Crispy Barnacle

[English](#english) | [Polski](#polski)

---

## English

Thank you for your interest in contributing to Crispy Barnacle! This document provides guidelines for contributing to the project.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [How Can I Contribute?](#how-can-i-contribute)
- [Development Setup](#development-setup)
- [Pull Request Process](#pull-request-process)
- [Coding Standards](#coding-standards)
- [Commit Message Guidelines](#commit-message-guidelines)

## Code of Conduct

This project adheres to the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). By participating, you are expected to uphold this code.

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check existing issues to avoid duplicates. When creating a bug report, include:

- **Clear title and description**
- **Steps to reproduce** the issue
- **Expected behavior** vs **actual behavior**
- **Environment details** (.NET version, OS, browser if applicable)
- **Screenshots** if applicable
- **Error messages** or stack traces

### Suggesting Enhancements

Enhancement suggestions are welcome! Please:

- Use a clear and descriptive title
- Provide detailed description of the proposed functionality
- Explain why this enhancement would be useful
- Include examples if possible

### Pull Requests

We actively welcome your pull requests:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Make your changes
4. Commit with clear messages
5. Push to your fork
6. Open a Pull Request

## Development Setup

See the [Development Guide](docs/DEVELOPMENT.md) for detailed setup instructions.

Quick start:

```bash
# Clone repository
git clone https://github.com/wojcikwojtek/crispy-barnacle.git
cd crispy-barnacle

# Build
cd SampleApp
dotnet restore
dotnet build

# Run
cd BackEnd && dotnet run  # Terminal 1
cd FrontEnd && dotnet run # Terminal 2
```

## Pull Request Process

### Before Submitting

1. **Build successfully**: Ensure `dotnet build` completes without errors
2. **Test your changes**: Manually verify functionality works as expected
3. **Follow coding standards**: Check `.github/csharp-guidelines.md` and `.github/blazor-guidelines.md`
4. **Update documentation**: If you change APIs or add features, update relevant docs

### PR Guidelines

- **One feature per PR**: Keep pull requests focused on a single feature or fix
- **Clear description**: Explain what changes you made and why
- **Link related issues**: Reference any related issues with `Fixes #123` or `Relates to #456`
- **Screenshots**: Include before/after screenshots for UI changes
- **Small commits**: Make logical, atomic commits with clear messages

### Review Process

1. A maintainer will review your PR
2. Address any requested changes
3. Once approved, your PR will be merged

## Coding Standards

### C# Guidelines

Follow the project's C# coding standards:

```csharp
// ✅ Good: Nullable reference types, XML comments
/// <summary>
/// Retrieves a product by its unique identifier
/// </summary>
/// <param name="id">The product ID</param>
/// <returns>The product if found, null otherwise</returns>
public Product? GetProductById(int id)
{
    return products.FirstOrDefault(p => p.Id == id);
}

// ❌ Bad: No documentation, nullable not handled
public Product GetProductById(int id)
{
    return products.FirstOrDefault(p => p.Id == id);
}
```

### API Endpoint Standards

```csharp
// ✅ Good: Complete endpoint with OpenAPI documentation
app.MapGet("/api/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    return product == null ? Results.NotFound() : Results.Ok(product);
})
.WithName("GetProductById")
.WithDescription("Retrieves a specific product by its ID")
.WithOpenApi()
.WithTags("Products");

// ❌ Bad: No OpenAPI documentation
app.MapGet("/api/products/{id}", (int id) =>
{
    return products.FirstOrDefault(p => p.Id == id);
});
```

### Blazor Component Standards

```razor
@* ✅ Good: Clear component with documentation *@
@page "/products"
@using FrontEnd.Data

<PageTitle>Product Catalog</PageTitle>

<h3>Products</h3>

@if (products == null)
{
    <p><em>Loading...</em></p>
}
else
{
    <div class="row">
        @foreach (var product in products)
        {
            <ProductCard Product="@product" />
        }
    </div>
}

@code {
    private Product[]? products;

    [Inject]
    private ProductClient ProductClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        products = await ProductClient.GetAllAsync();
    }
}
```

## Commit Message Guidelines

Follow the [Conventional Commits](https://www.conventionalcommits.org/) specification:

### Format

```
<type>: <description>

[optional body]

[optional footer]
```

### Types

- `Add:` - New feature or capability
- `Fix:` - Bug fix
- `Update:` - Changes to existing functionality
- `Docs:` - Documentation only changes
- `Refactor:` - Code refactoring (no functional changes)
- `Test:` - Adding or updating tests
- `Chore:` - Maintenance tasks (dependencies, build config)

### Examples

```bash
# Good commit messages
Add: Product search functionality
Fix: Null reference exception in order validation
Update: Improve error messages for user API
Docs: Add API endpoint documentation for orders
Refactor: Extract validation logic to separate class
Test: Add unit tests for OrderValidation

# Bad commit messages
Fixed stuff
Updated code
Changes
WIP
asdf
```

### Detailed Commit Example

```
Add: Order status transition validation

Implement business rules for valid order status transitions.
Orders can only move through specific states in a defined order.

- Add OrderValidation.IsValidStatusTransition method
- Prevent invalid status changes (e.g., Delivered -> Pending)
- Add validation to PUT /api/orders/{id}/status endpoint
- Update API documentation with valid transitions

Fixes #42
```

## Documentation Standards

### Code Comments

```csharp
// ✅ Good: XML documentation for public APIs
/// <summary>
/// Validates order data and business rules
/// </summary>
/// <param name="order">The order to validate</param>
/// <returns>List of validation error messages, empty if valid</returns>
public static List<string> ValidateOrder(Order order)
{
    // Implementation...
}

// ✅ Good: Inline comment for complex logic
// Calculate total with 10% discount for orders over $1000
var total = order.TotalAmount > 1000 
    ? order.TotalAmount * 0.9m 
    : order.TotalAmount;

// ❌ Bad: Obvious comment that adds no value
// Increment i
i++;
```

### README Updates

When adding features, update:

- README.md: Main feature list
- docs/API.md: If adding/changing endpoints
- docs/ARCHITECTURE.md: If changing architecture
- docs/DEVELOPMENT.md: If changing development process

## Testing Guidelines

### Manual Testing Checklist

Before submitting a PR, verify:

- [ ] Backend builds successfully
- [ ] Frontend builds successfully
- [ ] No console errors in browser
- [ ] All API endpoints return expected results (test in Scalar)
- [ ] UI displays correctly on desktop and mobile
- [ ] Forms validate properly
- [ ] Error handling works as expected

### API Testing

Test all endpoints using Scalar UI:

1. Navigate to `http://localhost:5001/scalar`
2. Test each affected endpoint
3. Verify request/response formats
4. Check error responses
5. Test edge cases (empty data, invalid IDs, etc.)

## Questions?

If you have questions about contributing:

- Check the [Development Guide](docs/DEVELOPMENT.md)
- Review existing issues and PRs
- Open a discussion on GitHub
- Ask in your PR comments

Thank you for contributing to Crispy Barnacle! 🐙

---

## Polski

## Współtworzenie Crispy Barnacle

Dziękujemy za zainteresowanie współtworzeniem projektu Crispy Barnacle! Ten dokument zawiera wytyczne dotyczące współpracy przy projekcie.

## Spis Treści

- [Kodeks Postępowania](#kodeks-postępowania)
- [Jak Mogę Pomóc?](#jak-mogę-pomóc)
- [Konfiguracja Środowiska](#konfiguracja-środowiska)
- [Proces Pull Request](#proces-pull-request)
- [Standardy Kodowania](#standardy-kodowania)
- [Wytyczne dla Commitów](#wytyczne-dla-commitów)

## Kodeks Postępowania

Ten projekt przestrzega [Kodeksu Postępowania Microsoft Open Source](https://opensource.microsoft.com/codeofconduct/). Uczestnicząc, oczekuje się przestrzegania tego kodeksu.

## Jak Mogę Pomóc?

### Zgłaszanie Błędów

Przed utworzeniem zgłoszenia błędu, sprawdź istniejące issues, aby uniknąć duplikatów. Tworząc zgłoszenie błędu, uwzględnij:

- **Jasny tytuł i opis**
- **Kroki do odtworzenia** problemu
- **Oczekiwane zachowanie** vs **rzeczywiste zachowanie**
- **Szczegóły środowiska** (wersja .NET, system operacyjny, przeglądarka jeśli dotyczy)
- **Zrzuty ekranu** jeśli dotyczy
- **Komunikaty błędów** lub stack trace

### Sugerowanie Ulepszeń

Sugestie ulepszeń są mile widziane! Proszę:

- Użyj jasnego i opisowego tytułu
- Podaj szczegółowy opis proponowanej funkcjonalności
- Wyjaśnij, dlaczego to ulepszenie byłoby przydatne
- Dodaj przykłady jeśli to możliwe

### Pull Requesty

Aktywnie przyjmujemy Twoje pull requesty:

1. Zrób fork repozytorium
2. Utwórz gałąź funkcjonalności (`git checkout -b feature/NowafunkcjonalnoscPL`)
3. Wprowadź zmiany
4. Commituj z jasnymi komunikatami
5. Wyślij do swojego forka
6. Otwórz Pull Request

## Konfiguracja Środowiska

Zobacz [Przewodnik Deweloperski](docs/DEVELOPMENT.md) dla szczegółowych instrukcji konfiguracji.

Szybki start:

```bash
# Sklonuj repozytorium
git clone https://github.com/wojcikwojtek/crispy-barnacle.git
cd crispy-barnacle

# Zbuduj
cd SampleApp
dotnet restore
dotnet build

# Uruchom
cd BackEnd && dotnet run  # Terminal 1
cd FrontEnd && dotnet run # Terminal 2
```

## Proces Pull Request

### Przed Wysłaniem

1. **Udane budowanie**: Upewnij się, że `dotnet build` kończy się bez błędów
2. **Przetestuj zmiany**: Ręcznie zweryfikuj, że funkcjonalność działa zgodnie z oczekiwaniami
3. **Przestrzegaj standardów kodowania**: Sprawdź `.github/csharp-guidelines.md` i `.github/blazor-guidelines.md`
4. **Zaktualizuj dokumentację**: Jeśli zmieniasz API lub dodajesz funkcje, zaktualizuj odpowiednią dokumentację

### Wytyczne PR

- **Jedna funkcja na PR**: Utrzymuj pull requesty skoncentrowane na pojedynczej funkcji lub poprawce
- **Jasny opis**: Wyjaśnij, jakie zmiany wprowadziłeś i dlaczego
- **Linkuj powiązane issues**: Odwołaj się do powiązanych issues używając `Fixes #123` lub `Relates to #456`
- **Zrzuty ekranu**: Dołącz zrzuty ekranu przed/po dla zmian UI
- **Małe commity**: Twórz logiczne, atomowe commity z jasnymi komunikatami

## Standardy Kodowania

Szczegółowe przykłady standardów kodowania znajdują się w sekcji angielskiej powyżej.

## Wytyczne dla Commitów

Przestrzegaj specyfikacji [Conventional Commits](https://www.conventionalcommits.org/):

### Format

```
<typ>: <opis>

[opcjonalne ciało]

[opcjonalna stopka]
```

### Typy

- `Add:` - Nowa funkcjonalność lub możliwość
- `Fix:` - Naprawa błędu
- `Update:` - Zmiany w istniejącej funkcjonalności
- `Docs:` - Zmiany tylko w dokumentacji
- `Refactor:` - Refaktoryzacja kodu (bez zmian funkcjonalnych)
- `Test:` - Dodawanie lub aktualizacja testów
- `Chore:` - Zadania konserwacyjne (zależności, konfiguracja budowania)

### Przykłady

```bash
# Dobre komunikaty commitów
Add: Funkcjonalność wyszukiwania produktów
Fix: Wyjątek null reference w walidacji zamówienia
Update: Poprawa komunikatów błędów dla API użytkowników
Docs: Dodanie dokumentacji endpointów API dla zamówień
Refactor: Wydzielenie logiki walidacji do osobnej klasy
Test: Dodanie testów jednostkowych dla OrderValidation
```

## Pytania?

Jeśli masz pytania dotyczące współtworzenia:

- Sprawdź [Przewodnik Deweloperski](docs/DEVELOPMENT.md)
- Przejrzyj istniejące issues i PR
- Otwórz dyskusję na GitHub
- Zapytaj w komentarzach do swojego PR

Dziękujemy za współtworzenie Crispy Barnacle! 🐙
