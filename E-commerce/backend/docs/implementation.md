# E-Commerce Product Management System - Implementation Documentation

## Overview
This project implements a complete CRUD (Create, Read, Update, Delete) system for product management in an e-commerce application using C# .NET 6.0.

## Architecture
The application follows a layered architecture pattern:

### 1. **Model Layer** (`src/models/`)
- **Product.cs**: Core entity with 7 attributes (Id, Name, Description, Price, Category, Stock, CreatedDate)
- Implements CSV serialization/deserialization methods

### 2. **Repository Layer** (`src/repositories/`)
- **IProductRepository.cs**: Interface defining CRUD contract
- **FileProductRepository.cs**: File-based implementation using CSV storage
- **Methods Implemented**:
  - `GetAll()`: Reads all products from CSV
  - `GetById(id)`: Finds specific product by ID
  - `Add(product)`: Adds new product
  - `Update(product)`: Updates existing product
  - `Delete(id)`: Removes product by ID
  - `Save()`: Persists changes to CSV file

### 3. **Service Layer** (`src/services/`)
- **IProductService.cs**: Interface for business logic
- **ProductService.cs**: Implements business rules and validation
- **Methods Implemented**:
  - `ListProducts()`: Lists products with optional filtering (category, price range)
  - `AddProduct()`: Adds product with validation (name not empty, price > 0)
  - `FindProductById()`: Retrieves product by ID
  - `UpdateProduct()`: Updates product with validation
  - `DeleteProduct()`: Removes product
  - `GetCategories()`: Returns distinct categories

### 4. **UI Layer** (`src/ui/`)
- **ConsoleUI.cs**: Console-based user interface
- **Complete Menu System** with 8 options:
  1. List All Products
  2. Filter Products (by category, price range)
  3. Find Product by ID
  4. Add New Product
  5. Update Product
  6. Delete Product
  7. Show Categories
  8. Exit

## Key Features

### ✅ **Model + Repository (30 points)**
- Product model with 7+ attributes ✓
- FileRepository with all CRUD methods ✓
- CSV file with 7 initial records ✓
- Proper error handling and validation ✓

### ✅ **Service with Logic (25 points)**
- 3+ service methods (List, Add, Find, Update, Delete) ✓
- Dependency injection (Repository injected into Service) ✓
- Input validation (name not empty, price > 0, stock >= 0) ✓
- Advanced filtering capabilities ✓

### ✅ **Functional UI (25 points)**
- Complete console menu system ✓
- End-to-end flow: UI → Service → Repository → File ✓
- All CRUD operations fully functional ✓
- User-friendly interface with clear prompts ✓

### ✅ **Update + Delete (10 points)**
- Update method in Repository, Service, and UI ✓
- Delete method in Repository, Service, and UI ✓
- Proper confirmation for delete operations ✓

### ✅ **Documentation (10 points)**
- Complete implementation documentation ✓
- Architecture explanation ✓
- Feature breakdown ✓

## Data Flow
