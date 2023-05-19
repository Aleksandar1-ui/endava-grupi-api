using CsvHelper;
using endavaRestApi.Data;
using endavaRestApi.Repositories;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace Testing.Tests.CsvRepositoryTesting
{
    public class CSVServiceTesting
    {
        private readonly ShopContext _fakeShopContext;
        private readonly ILogger<CSVService> _fakeLogger;
        private readonly CSVService _csvService;

        public CSVServiceTesting()
        {
            _fakeShopContext = A.Fake<ShopContext>();
            _fakeLogger = A.Fake<ILogger<CSVService>>();
            _csvService = new CSVService(_fakeShopContext, _fakeLogger);
        }

        [Fact]
        public async Task ImportCsv_ValidFile_ImportsProducts()
        {
            // Arrange
            var formFile = A.Fake<IFormFile>();
            A.CallTo(() => formFile.Length).Returns(10);

            var csvReader = A.Fake<CsvReader>();
            A.CallTo(() => csvReader.GetRecords<ProductCSV>())
                .Returns(new List<ProductCSV>
                {
                  new ProductCSV { ProductGuidId = Guid.NewGuid(), ProductName = "Product 1", ProductBrand = "Brand 1", ProductCategory = "Category 1", ProductDescription = "Description 1", ProductQuantity = 10, ProductSize = "Size 1", Color = "Color 1", Weight = (decimal)20.4, Price = (decimal)40.2 },
                  new ProductCSV { ProductGuidId = Guid.NewGuid(), ProductName = "Product 2", ProductBrand = "Brand 2", ProductCategory = "Category 2", ProductDescription = "Description 2", ProductQuantity = 5, ProductSize = "Size 2", Color = "Color 2", Weight = (decimal)1.2, Price = (decimal)30.8 },
                });

            var productsDbSet = A.Fake<DbSet<Product>>();
            var entityEntry = A.Fake<EntityEntry<Product>>();
            A.CallTo(() => entityEntry.State).Returns(EntityState.Added);
            A.CallTo(() => productsDbSet.Add(A<Product>._)).Returns(entityEntry);
            A.CallTo(() => _fakeShopContext.Products).Returns(productsDbSet);

            // Act
            await _csvService.ImportCsv(formFile);

            // Assert
            A.CallTo(() => productsDbSet.Add(A<Product>._)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => productsDbSet.Update(A<Product>._)).MustNotHaveHappened();
            A.CallTo(() => _fakeShopContext.SaveChangesAsync(A<CancellationToken>._))
                .WithAnyArguments()
                .MustHaveHappenedOnceExactly();
        }

    }
    }
