# API Documentation

[English](#english) | [Polski](#polski)

---

## English

This document provides detailed information about all API endpoints available in the Backend service.

### Base URL

```
http://localhost:5001
```

In production or Codespaces, use the appropriate forwarded URL.

---

## Weather Forecast API

### Get Weather Forecast

Returns a 5-day weather forecast with temperature and summary.

**Endpoint:** `GET /weatherforecast`

**Response:**
```json
[
  {
    "date": "2024-10-16",
    "temperatureC": 15,
    "temperatureF": 58,
    "summary": "Mild"
  },
  {
    "date": "2024-10-17",
    "temperatureC": 22,
    "temperatureF": 71,
    "summary": "Warm"
  }
]
```

**Status Codes:**
- `200 OK` - Successfully retrieved forecast

---

## Products API

### Get All Products

Retrieves all products in the catalog.

**Endpoint:** `GET /api/products`

**Response:**
```json
[
  {
    "id": 1,
    "name": "Laptop",
    "description": "High-performance laptop",
    "price": 1299.99,
    "stockQuantity": 10
  },
  {
    "id": 2,
    "name": "Smartphone",
    "description": "Latest smartphone model",
    "price": 799.99,
    "stockQuantity": 15
  }
]
```

**Status Codes:**
- `200 OK` - Successfully retrieved products

---

### Get Product by ID

Retrieves a specific product by its ID.

**Endpoint:** `GET /api/products/{id}`

**Parameters:**
- `id` (path parameter) - Product ID (integer)

**Response:**
```json
{
  "id": 1,
  "name": "Laptop",
  "description": "High-performance laptop",
  "price": 1299.99,
  "stockQuantity": 10
}
```

**Status Codes:**
- `200 OK` - Product found
- `404 Not Found` - Product with specified ID doesn't exist

---

### Create Product

Creates a new product in the catalog.

**Endpoint:** `POST /api/products`

**Request Body:**
```json
{
  "name": "Tablet",
  "description": "10-inch tablet",
  "price": 499.99,
  "stockQuantity": 20
}
```

**Response:**
```json
{
  "id": 3,
  "name": "Tablet",
  "description": "10-inch tablet",
  "price": 499.99,
  "stockQuantity": 20
}
```

**Status Codes:**
- `201 Created` - Product successfully created
- `400 Bad Request` - Invalid product data

---

### Update Product

Updates an existing product.

**Endpoint:** `PUT /api/products/{id}`

**Parameters:**
- `id` (path parameter) - Product ID (integer)

**Request Body:**
```json
{
  "name": "Laptop Pro",
  "description": "Updated high-performance laptop",
  "price": 1499.99,
  "stockQuantity": 8
}
```

**Response:**
```json
{
  "id": 1,
  "name": "Laptop Pro",
  "description": "Updated high-performance laptop",
  "price": 1499.99,
  "stockQuantity": 8
}
```

**Status Codes:**
- `200 OK` - Product successfully updated
- `404 Not Found` - Product with specified ID doesn't exist

---

### Delete Product

Deletes a product from the catalog.

**Endpoint:** `DELETE /api/products/{id}`

**Parameters:**
- `id` (path parameter) - Product ID (integer)

**Status Codes:**
- `204 No Content` - Product successfully deleted
- `404 Not Found` - Product with specified ID doesn't exist

---

## Users API

### Get All Users

Retrieves all users with sensitive data (password hash) excluded.

**Endpoint:** `GET /api/users`

**Response:**
```json
[
  {
    "id": 1,
    "username": "john.doe",
    "email": "john.doe@example.com",
    "createdAt": "2024-10-15T12:00:00Z",
    "updatedAt": "2024-10-15T12:00:00Z"
  }
]
```

**Status Codes:**
- `200 OK` - Successfully retrieved users

---

### Get User by ID

Retrieves a specific user by ID (without password hash).

**Endpoint:** `GET /api/users/{id}`

**Parameters:**
- `id` (path parameter) - User ID (integer)

**Response:**
```json
{
  "id": 1,
  "username": "john.doe",
  "email": "john.doe@example.com",
  "createdAt": "2024-10-15T12:00:00Z",
  "updatedAt": "2024-10-15T12:00:00Z"
}
```

**Status Codes:**
- `200 OK` - User found
- `404 Not Found` - User with specified ID doesn't exist

---

### Create User

Creates a new user account.

**Endpoint:** `POST /api/users`

**Request Body:**
```json
{
  "username": "jane.smith",
  "email": "jane.smith@example.com",
  "passwordHash": "hashed_password_here"
}
```

**Response:**
```json
{
  "id": 2,
  "username": "jane.smith",
  "email": "jane.smith@example.com",
  "createdAt": "2024-10-15T14:30:00Z",
  "updatedAt": "2024-10-15T14:30:00Z"
}
```

**Status Codes:**
- `201 Created` - User successfully created
- `400 Bad Request` - Email or username already exists

**Validation:**
- Email must be unique
- Username must be unique

---

### Update User

Updates an existing user's information.

**Endpoint:** `PUT /api/users/{id}`

**Parameters:**
- `id` (path parameter) - User ID (integer)

**Request Body:**
```json
{
  "username": "jane.smith.updated",
  "email": "jane.updated@example.com",
  "passwordHash": "new_hashed_password"
}
```

**Response:**
```json
{
  "id": 2,
  "username": "jane.smith.updated",
  "email": "jane.updated@example.com",
  "createdAt": "2024-10-15T14:30:00Z",
  "updatedAt": "2024-10-15T15:45:00Z"
}
```

**Status Codes:**
- `200 OK` - User successfully updated
- `400 Bad Request` - Email or username already exists
- `404 Not Found` - User with specified ID doesn't exist

---

### Delete User

Deletes a user account.

**Endpoint:** `DELETE /api/users/{id}`

**Parameters:**
- `id` (path parameter) - User ID (integer)

**Status Codes:**
- `204 No Content` - User successfully deleted
- `404 Not Found` - User with specified ID doesn't exist

---

## Orders API

### Get All Orders

Retrieves all orders.

**Endpoint:** `GET /api/orders`

**Response:**
```json
[
  {
    "id": 1,
    "customerName": "John Doe",
    "customerEmail": "john@example.com",
    "orderDate": "2024-10-15T10:00:00Z",
    "items": [
      {
        "id": 1,
        "productId": 1,
        "productName": "Laptop",
        "quantity": 1,
        "unitPrice": 1299.99
      }
    ],
    "totalAmount": 1299.99,
    "status": "Pending"
  }
]
```

**Status Codes:**
- `200 OK` - Successfully retrieved orders

---

### Get Order by ID

Retrieves a specific order by ID.

**Endpoint:** `GET /api/orders/{id}`

**Parameters:**
- `id` (path parameter) - Order ID (integer)

**Response:**
```json
{
  "id": 1,
  "customerName": "John Doe",
  "customerEmail": "john@example.com",
  "orderDate": "2024-10-15T10:00:00Z",
  "items": [
    {
      "id": 1,
      "productId": 1,
      "productName": "Laptop",
      "quantity": 1,
      "unitPrice": 1299.99
    }
  ],
  "totalAmount": 1299.99,
  "status": "Pending"
}
```

**Status Codes:**
- `200 OK` - Order found
- `404 Not Found` - Order with specified ID doesn't exist

---

### Create Order

Creates a new order with validation.

**Endpoint:** `POST /api/orders`

**Request Body:**
```json
{
  "customerName": "Jane Smith",
  "customerEmail": "jane@example.com",
  "items": [
    {
      "productId": 2,
      "productName": "Smartphone",
      "quantity": 2,
      "unitPrice": 799.99
    }
  ]
}
```

**Response:**
```json
{
  "id": 2,
  "customerName": "Jane Smith",
  "customerEmail": "jane@example.com",
  "orderDate": "2024-10-15T15:00:00Z",
  "items": [
    {
      "id": 1,
      "productId": 2,
      "productName": "Smartphone",
      "quantity": 2,
      "unitPrice": 799.99
    }
  ],
  "totalAmount": 1599.98,
  "status": "Pending"
}
```

**Status Codes:**
- `201 Created` - Order successfully created
- `400 Bad Request` - Invalid order data or validation errors

**Validation Rules:**
- Customer name is required
- Customer email must be valid
- At least one item is required
- Item quantity must be greater than 0
- Unit price must be greater than 0

---

### Update Order Status

Updates the status of an existing order with validation of status transitions.

**Endpoint:** `PUT /api/orders/{id}/status`

**Parameters:**
- `id` (path parameter) - Order ID (integer)

**Request Body:**
```json
"Confirmed"
```

**Response:**
```json
{
  "id": 1,
  "customerName": "John Doe",
  "customerEmail": "john@example.com",
  "orderDate": "2024-10-15T10:00:00Z",
  "items": [...],
  "totalAmount": 1299.99,
  "status": "Confirmed"
}
```

**Status Codes:**
- `200 OK` - Status successfully updated
- `400 Bad Request` - Invalid status transition
- `404 Not Found` - Order with specified ID doesn't exist

**Valid Status Transitions:**
- `Pending` → `Confirmed` or `Cancelled`
- `Confirmed` → `Shipped` or `Cancelled`
- `Shipped` → `Delivered`
- `Cancelled` → (no further transitions allowed)
- `Delivered` → (no further transitions allowed)

**Order Status Enum:**
- `Pending` - Order created but not confirmed
- `Confirmed` - Order confirmed and being processed
- `Shipped` - Order has been shipped
- `Delivered` - Order has been delivered
- `Cancelled` - Order has been cancelled

---

## Contact API

### Submit Contact Message

Submits a contact form message.

**Endpoint:** `POST /api/contact`

**Request Body:**
```json
{
  "name": "John Doe",
  "email": "john@example.com",
  "message": "I have a question about your products."
}
```

**Response:**
```json
{
  "id": 1,
  "name": "John Doe",
  "email": "john@example.com",
  "message": "I have a question about your products.",
  "createdAt": "2024-10-15T16:00:00Z"
}
```

**Status Codes:**
- `201 Created` - Message successfully submitted
- `400 Bad Request` - Invalid message data

---

## Error Responses

All endpoints may return the following error responses:

### 400 Bad Request
```json
{
  "error": "Invalid data provided",
  "errors": ["Field 'name' is required", "Email format is invalid"]
}
```

### 404 Not Found
```json
{
  "error": "Resource not found"
}
```

### 500 Internal Server Error
```json
{
  "error": "An internal server error occurred"
}
```

---

## Testing the API

### Using Scalar UI

1. Run the backend application
2. Navigate to `http://localhost:5001/scalar`
3. Browse available endpoints
4. Click "Test Request" on any endpoint
5. Fill in parameters and request body
6. Click "Send" to test the endpoint
7. View the response

### Using cURL

**Get Weather Forecast:**
```bash
curl -X GET http://localhost:5001/weatherforecast
```

**Create Product:**
```bash
curl -X POST http://localhost:5001/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Tablet",
    "description": "10-inch tablet",
    "price": 499.99,
    "stockQuantity": 20
  }'
```

**Update Order Status:**
```bash
curl -X PUT http://localhost:5001/api/orders/1/status \
  -H "Content-Type: application/json" \
  -d '"Confirmed"'
```

---

## Polski

## Dokumentacja API

Ten dokument zawiera szczegółowe informacje o wszystkich endpointach API dostępnych w serwisie Backend.

### Podstawowy URL

```
http://localhost:5001
```

W środowisku produkcyjnym lub Codespaces użyj odpowiednio przekierowanego URL.

---

## API Prognozy Pogody

### Pobierz Prognozę Pogody

Zwraca 5-dniową prognozę pogody z temperaturą i opisem.

**Endpoint:** `GET /weatherforecast`

**Odpowiedź:**
```json
[
  {
    "date": "2024-10-16",
    "temperatureC": 15,
    "temperatureF": 58,
    "summary": "Mild"
  }
]
```

**Kody Statusu:**
- `200 OK` - Pomyślnie pobrano prognozę

---

## API Produktów

Szczegółowe informacje o wszystkich operacjach CRUD dla produktów znajdują się w sekcji angielskiej powyżej.

## API Użytkowników

Szczegółowe informacje o zarządzaniu użytkownikami znajdują się w sekcji angielskiej powyżej.

## API Zamówień

Szczegółowe informacje o przetwarzaniu zamówień znajdują się w sekcji angielskiej powyżej.

**Dozwolone Przejścia Statusu:**
- `Pending` (Oczekujące) → `Confirmed` (Potwierdzone) lub `Cancelled` (Anulowane)
- `Confirmed` (Potwierdzone) → `Shipped` (Wysłane) lub `Cancelled` (Anulowane)
- `Shipped` (Wysłane) → `Delivered` (Dostarczone)
- `Cancelled` (Anulowane) → (brak dalszych przejść)
- `Delivered` (Dostarczone) → (brak dalszych przejść)

## API Kontaktu

Szczegółowe informacje o formularzu kontaktowym znajdują się w sekcji angielskiej powyżej.

---

## Testowanie API

### Używanie Interfejsu Scalar

1. Uruchom aplikację backendu
2. Przejdź do `http://localhost:5001/scalar`
3. Przeglądaj dostępne endpointy
4. Kliknij "Test Request" na dowolnym endpoincie
5. Wypełnij parametry i treść żądania
6. Kliknij "Send" aby przetestować endpoint
7. Zobacz odpowiedź

### Używanie cURL

**Pobierz Prognozę Pogody:**
```bash
curl -X GET http://localhost:5001/weatherforecast
```

**Utwórz Produkt:**
```bash
curl -X POST http://localhost:5001/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Tablet",
    "description": "Tablet 10-calowy",
    "price": 499.99,
    "stockQuantity": 20
  }'
```
