using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using endavaRestApi.Controllers;
using endavaRestApi.Repositories;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//testovi
namespace Testing.Tests.ShopControllerTesting
{
    public class ShopControllerCSVServiceTest
    {
        private readonly IShopRepository _shopRepository;
        private readonly ICSVRepository _csvRepository;
        
        public ShopControllerCSVServiceTest()
        {
            _shopRepository = A.Fake<IShopRepository>();
            _csvRepository = A.Fake<ICSVRepository>();
        }
        [Fact]
        public async Task ImportProducts_WithValidCsvFile_ReturnsOkResult()
        {
            //Arrange
            var file = CreateFakeCsvFile();
            var controller = new ShopController(_shopRepository, _csvRepository);

            //Act
            var result = await controller.ImportProducts(file);

            //Assert
            result.Should().BeOfType<OkResult>();
            A.CallTo(() => _csvRepository.ImportCsv(file)).MustHaveHappened();

        }
        private static IFormFile CreateFakeCsvFile()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.WriteLine("Product 1, Brand 1, Category 1, Price 1, Description 1, Quantity 1, Size 1, Weight 1, Color 1,{0}",Guid.NewGuid());
            writer.WriteLine("Product 2, Brand 2, Category 2, Price 2, Description 2, Quantity 2, Size 2, Weight 2, Color 2,{0}",Guid.NewGuid());
            writer.Flush();
            stream.Position = 0;
            var file = A.Fake<IFormFile>();
            A.CallTo(() => file.OpenReadStream()).Returns(stream);
            A.CallTo(() => file.Length).Returns(stream.Length);
            return file;
        }
    }
}
