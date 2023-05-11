﻿using CsvHelper;
using endavaRestApi.Repositories;
using System.Globalization;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace endavaRestApi.Data
{
    public class CSVService : ICSVRepository
    {
        private readonly ShopContext _shopContext;
        private readonly ILogger<CSVService> _logger;
        public CSVService(ShopContext shopContext, ILogger<CSVService> logger)
        {
            _shopContext = shopContext;
            _logger = logger;
        }
        public async Task VnesiCSV(IFormFile file)
        {

            if (file == null || file.Length == 0)
            {
                _logger.LogError("File is empty or null.");
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
                    .SingleOrDefaultAsync(p => p.ProductName == record.ProductName);

                if (product == null)
                {
                    product = new Product();
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

            await _shopContext.SaveChangesAsync();
        }

    }

}