using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
using MMABooksEFClasses.MarisModels;
using Microsoft.EntityFrameworkCore;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {

        MMABooksContext dbContext;
        Products? product;
        List<Products>? products;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest()
        {
            var products = dbContext.Products.OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);

        }


        [Test]
        public void GetByPrimaryKeyTest()
        {
            var product = dbContext.Products.Find("A4CS");
            Assert.IsNotNull(product);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", product.Description);
            Console.WriteLine(product);
        }

        [Test]
        public void GetUsingWhere()
        {
            var products = dbContext.Products.Where(p => p.UnitPrice == 56.50m).OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(7, products.Count);
            Assert.AreEqual(56.50m, products[0].UnitPrice);
            PrintAll(products);
        }

        [Test]
        public void GetWithCalculatedFieldTest()
        {
            // get a list of objects that include the productcode, unitprice, quantity and inventoryvalue
            var products = dbContext.Products.Select(
            p => new { p.ProductCode, p.UnitPrice, p.OnHandQuantity, Value = p.UnitPrice * p.OnHandQuantity }).
            OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            foreach (var p in products)
            {
                Console.WriteLine(p);
            }
        }

        [Test]
        public void DeleteTest()
        {
            dbContext.Database.ExecuteSqlRaw("DELETE FROM InvoiceLineItems WHERE ProductCode = 'A4CS'");
            product = dbContext.Products.Find("A4CS");
            dbContext.Products.Remove(product);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Products.Find("A4CS"));

        }

        [Test]
        public void CreateTest()
        {
            var existingProduct = dbContext.Products.Find("A4CS");
            if (existingProduct != null)
            {
                dbContext.Products.Remove(existingProduct);
                dbContext.SaveChanges();
            }
            product = new Products();
            product.ProductCode = "A4CS";
            product.Description = "Murach's ASP.NET 4 Web Programming with C# 2010";
            product.UnitPrice = 56.50m;
            product.OnHandQuantity = 4637;
            dbContext.Products.Add(product);
            dbContext.SaveChanges();
            Assert.IsNotNull(dbContext.Products.Find("A4CS"));
        }

        [Test]
        public void UpdateTest()
        {

        }
        private void PrintAll(List<Products> products)
        {
            foreach (var p in products)
            {
                Console.WriteLine($"Product Code: {p.ProductCode}, Description: {p.Description}," +
                    $" Unit Price: {p.UnitPrice}, On Hand Quantity: {p.OnHandQuantity}");
            }
        }
    }
}