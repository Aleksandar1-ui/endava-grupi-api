using CsvHelper;
using endavaRestApi.Repositories;
using System.Globalization;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using log4net;
namespace endavaRestApi.Data
{
    public class CSVService : ICSVRepository
    {
        private readonly ShopContext _shopContext;
        private readonly ILog _logger = LogManager.GetLogger(typeof(CSVService));
        public CSVService(ShopContext shopContext, ILog logger)
        {
            _shopContext = shopContext;
            _logger = logger;
        }
        public async Task ImportCsv(IFormFile file)
        {

            if (file == null || file.Length == 0)
            {
                _logger.Error("File is empty or null.");
                throw new ArgumentException("File is empty or null.");
            }

            using var streamReader = new StreamReader(file.OpenReadStream());
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            var records = csvReader.GetRecords<ProductCSV>().ToList();

            var avgPrice = await _shopContext.Products.AverageAsync(p => p.Price);
            var newProducts = new List<Product>();
            foreach (var record in records)
            {
                var product = await _shopContext.Products
                    .SingleOrDefaultAsync(p => p.ProductGuidId == record.ProductGuidId);

                if (product == null)
                {
                    product = new Product();
                    product.ProductGuidId = Guid.NewGuid();
                    product.ProductName = record.ProductName;
                    product.ProductBrand = record.ProductBrand;
                    product.ProductCategory = record.ProductCategory;
                    product.ProductDescription = record.ProductDescription;
                    product.ProductQuantity = record.ProductQuantity;
                    product.ProductSize = record.ProductSize;
                    product.Color = record.Color;
                    product.Weight = record.Weight;
                    product.Price = record.Price;
                    product.TotalPrice = record.Price > _shopContext.Products.Average(p => p.Price) ? record.Price - record.ProductQuantity : record.Price + record.ProductQuantity;
                    newProducts.Add(product);
                    _shopContext.Products.Add(product);
                }
                else
                {
                    product.ProductGuidId = record.ProductGuidId;
                    product.ProductName = record.ProductName;
                    product.ProductBrand = record.ProductBrand;
                    product.ProductCategory = record.ProductCategory;
                    product.ProductDescription = record.ProductDescription;
                    product.ProductQuantity = record.ProductQuantity;
                    product.ProductSize = record.ProductSize;
                    product.Color = record.Color;
                    product.Weight = record.Weight;
                    product.Price = record.Price;
                    product.TotalPrice = record.Price > _shopContext.Products.Average(p => p.Price) ? record.Price - record.ProductQuantity : record.Price + record.ProductQuantity;
                    _shopContext.Products.Update(product);
                }
            }
            _logger.Info("Successful");
            await _shopContext.SaveChangesAsync();
        }
        public async Task<object> GetMatchingPaymentDetailsAsync(int userId, string productName)
        {
            try
            {
                var user = await _shopContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    _logger.Error($"User with ID '{userId}' not found.");
                    return "User not found.";
                }

                var product = await _shopContext.Products.FirstOrDefaultAsync(p => p.ProductName == productName);
                if (product == null)
                {
                    _logger.Error($"Product with name '{productName}' not found.");
                    return "Product not found.";
                }

                var payment = await _shopContext.Payments.FirstOrDefaultAsync(p => p.UserId == userId && p.PricePaid == product.TotalPrice);
                if (payment == null)
                {
                    _logger.Error($"Payment for user ID '{userId}' and product '{productName}' not found or the price doesn't match.");
                    return "Payment not found or the price doesn't match.";
                }

                var result = new
                {
                    UserName = user.Name,
                    ProductName = product.ProductName,
                    PricePaid = payment.PricePaid,
                    PaymentMethod = payment.PaymentMethod
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred while retrieving the payment details.", ex);
                return "An error occurred while retrieving the payment details.";
            }
        }
    }

}
