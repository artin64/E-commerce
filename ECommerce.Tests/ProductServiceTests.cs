using ECommerce.Models;
using ECommerce.Services;
using Xunit;

namespace ECommerce.Tests
{
    /// <summary>
    /// Unit Tests per ProductService — Sprint 2.
    /// Perdorin InMemoryProductRepository: izolim i plote, pa CSV, pa disk.
    /// 15 teste, 5 grupe: Search, FilterByPrice, Statistics, AddProduct, DeleteProduct.
    /// </summary>
    public class ProductServiceTests
    {
        // ── Helper: krijon service me 5 produkte demo ────────────────────────
        private static ProductService CreateServiceWithData()
        {
            var repo    = new InMemoryProductRepository();
            var service = new ProductService(repo);
            service.AddProduct(new Product(0, "Laptop Dell",   999.99m,  10, "tech",    "S1"));
            service.AddProduct(new Product(0, "iPhone 15",    1299.00m,   5, "tech",    "S1"));
            service.AddProduct(new Product(0, "Nike Shoes",    89.99m,  50, "fashion",  "S2"));
            service.AddProduct(new Product(0, "Coffee Maker",  49.99m,  20, "home",     "S2"));
            service.AddProduct(new Product(0, "Yoga Mat",      24.99m, 100, "sports",   "S3"));
            return service;
        }

        // ════════════════════════════════════════════════════════════════
        // GRUP 1: SEARCH (4 teste)
        // ════════════════════════════════════════════════════════════════

        [Fact]
        public void Search_ByName_ReturnsMatchingProduct()
        {
            var service = CreateServiceWithData();

            var results = service.Search("Laptop").ToList();

            Assert.Single(results);
            Assert.Equal("Laptop Dell", results[0].Name);
        }

        [Fact]
        public void Search_ByCategory_ReturnsAllInCategory()
        {
            var service = CreateServiceWithData();

            var results = service.Search("tech").ToList();

            // Laptop Dell + iPhone 15 = 2 produkte
            Assert.Equal(2, results.Count);
        }

        [Fact]
        public void Search_NonExistingKeyword_ReturnsEmptyList()
        {
            var service = CreateServiceWithData();

            var results = service.Search("xyz-nuk-ekziston-999").ToList();

            Assert.Empty(results);
        }

        [Fact]
        public void Search_CaseInsensitive_FindsProduct()
        {
            var service = CreateServiceWithData();

            // Kerko me shkronja te medha — duhet gjeje njelloj
            var results = service.Search("LAPTOP").ToList();

            Assert.Single(results);
            Assert.Equal("Laptop Dell", results[0].Name);
        }

        // ════════════════════════════════════════════════════════════════
        // GRUP 2: FILTER BY PRICE (4 teste)
        // ════════════════════════════════════════════════════════════════

        [Fact]
        public void FilterByPrice_ValidRange_ReturnsCorrectProducts()
        {
            var service = CreateServiceWithData();

            // €20-€100: Nike(89.99) + Coffee(49.99) + Yoga(24.99) = 3
            var results = service.FilterByPrice(20, 100).ToList();

            Assert.Equal(3, results.Count);
            Assert.All(results, p => Assert.InRange(p.Price, 20m, 100m));
        }

        [Fact]
        public void FilterByPrice_NoProductsInRange_ReturnsEmpty()
        {
            var service = CreateServiceWithData();

            var results = service.FilterByPrice(5000, 9999).ToList();

            Assert.Empty(results);
        }

        [Fact]
        public void FilterByPrice_NegativeMin_ThrowsArgumentException()
        {
            var service = CreateServiceWithData();

            var ex = Assert.Throws<ArgumentException>(
                () => service.FilterByPrice(-1, 100));
            Assert.Contains("negativ", ex.Message);
        }

        [Fact]
        public void FilterByPrice_MaxLessThanMin_ThrowsArgumentException()
        {
            var service = CreateServiceWithData();

            Assert.Throws<ArgumentException>(
                () => service.FilterByPrice(500, 100));
        }

        // ════════════════════════════════════════════════════════════════
        // GRUP 3: STATISTICS (4 teste)
        // ════════════════════════════════════════════════════════════════

        [Fact]
        public void GetStatistics_WithProducts_ReturnsCorrectCount()
        {
            var service = CreateServiceWithData();

            var stats = service.GetStatistics();

            Assert.Equal(5, stats.Count);
        }

        [Fact]
        public void GetStatistics_WithProducts_ReturnsCorrectMax()
        {
            var service = CreateServiceWithData();

            var stats = service.GetStatistics();

            // iPhone 15 me 1299.00 eshte maksimali
            Assert.Equal(1299.00m, stats.Max);
        }

        [Fact]
        public void GetStatistics_WithProducts_ReturnsCorrectMin()
        {
            var service = CreateServiceWithData();

            var stats = service.GetStatistics();

            // Yoga Mat me 24.99 eshte minimali
            Assert.Equal(24.99m, stats.Min);
        }

        [Fact]
        public void GetStatistics_EmptyRepository_ReturnsAllZeros()
        {
            // Service bosh, pa asnje produkt
            var service = new ProductService(new InMemoryProductRepository());

            var stats = service.GetStatistics();

            Assert.Equal(0, stats.Count);
            Assert.Equal(0m, stats.Total);
            Assert.Equal(0m, stats.Average);
            Assert.Equal(0m, stats.Max);
            Assert.Equal(0m, stats.Min);
        }

        // ════════════════════════════════════════════════════════════════
        // GRUP 4: ADD PRODUCT — raste kufitare (3 teste)
        // ════════════════════════════════════════════════════════════════

        [Fact]
        public void AddProduct_ValidProduct_IncreasesCountByOne()
        {
            var service = new ProductService(new InMemoryProductRepository());
            int before  = service.GetAll().Count();

            service.AddProduct(new Product(0, "Produkt Test", 9.99m, 5, "test", "S1"));

            Assert.Equal(before + 1, service.GetAll().Count());
        }

        [Fact]
        public void AddProduct_EmptyName_ThrowsArgumentException()
        {
            var service = new ProductService(new InMemoryProductRepository());

            var ex = Assert.Throws<ArgumentException>(
                () => service.AddProduct(new Product(0, "", 10m, 5, "cat", "S1")));
            Assert.Contains("Emri", ex.Message);
        }

        [Fact]
        public void AddProduct_NegativePrice_ThrowsArgumentException()
        {
            var service = new ProductService(new InMemoryProductRepository());

            // Cmim negativ hedh ArgumentException nga Product constructor
            Assert.Throws<ArgumentException>(
                () => service.AddProduct(new Product(0, "Emri Valid", -5m, 10, "cat", "S1")));
        }

        // ════════════════════════════════════════════════════════════════
        // GRUP 5: DELETE PRODUCT (2 teste)
        // ════════════════════════════════════════════════════════════════

        [Fact]
        public void DeleteProduct_ExistingId_RemovesProductFromList()
        {
            var service  = CreateServiceWithData();
            int before   = service.GetAll().Count();
            int firstId  = service.GetAll().First().Id;

            service.DeleteProduct(firstId);

            Assert.Equal(before - 1, service.GetAll().Count());
            Assert.Null(service.GetById(firstId));
        }

        [Fact]
        public void DeleteProduct_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Service bosh
            var service = new ProductService(new InMemoryProductRepository());

            // ID 9999 nuk ekziston → KeyNotFoundException
            Assert.Throws<KeyNotFoundException>(
                () => service.DeleteProduct(9999));
        }
    }
}
