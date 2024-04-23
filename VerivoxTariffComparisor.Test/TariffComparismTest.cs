using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using VerivoxTariffComparisor.Data;
using VerivoxTariffComparisor.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VerivoxTariffComparisor.Test
{
    [TestClass]
    public class AnnualCostCalculationTest
    {
        private TariffDbContext _context;
        [TestInitialize]
        public void Initialize()
        {            
            var builder = new DbContextOptionsBuilder<TariffDbContext>();
            builder.UseInMemoryDatabase("TestDB");
            var options = builder.Options;
            _context = new TariffDbContext(options);

            if (!_context.TariffProvider.Any())
            {
                _context.TariffProvider.AddRange(
                      new Product
                      {
                          name = "Product A",
                          Type = 1,
                          ProductType = new ProductType
                          {
                              Id = 1,
                              TypeName = "Basic Electricity Tariff"
                          },
                          IncludedKwhCost = 0,
                          BaseCostPerMonth = 5,
                          UnitKwhCostInEuro = 0.22
                      },
                      new Product
                      {
                          name = "Product B",
                          Type = 2,
                          ProductType = new ProductType
                          {
                              Id = 2,
                              TypeName = "Packaged Tariff"

                          },
                          BaseCostPerYear = 800,
                          IncludedKwhCost = 4000,
                          UnitKwhCostInEuro = 0.30

                      });
                _context.SaveChanges();
            }
        }
        
        [TestMethod]
        public void TestComparism_WithAllValues_WorksExpected()
        {
            //Act
            var result = _context.GetCalculationResults(_context.TariffProvider.Include( x=> x.ProductType).ToList(),3500);

            //Asset
            Assert.AreEqual(result[0].AnnualCost, 800);
            Assert.AreEqual(result[1].TariffName, "Basic Electricity Tariff");

        }

        [TestMethod]
        public void TestComparism_AddNewProductwithNewProductType_WorksExpected()
        {
            //Arrange
            _context.TariffProvider.Add(new Product
            {
                name = "Product C",
                Type = 3,
                ProductType = new ProductType { Id = 3, TypeName = "TestTariffName" },
                IncludedKwhCost = 3000,
                BaseCostPerYear = 700,
                UnitKwhCostInEuro = 0.25
            });
            _context.SaveChanges(); 

            //Act
            var result = _context.GetCalculationResults(_context.TariffProvider.Include(x => x.ProductType).ToList(), 3500);

            //Asset
            Assert.AreEqual(result.Count, 3);
            Assert.AreNotEqual(result[2].AnnualCost, 800);
            Assert.AreEqual(result[1].TariffName, "TestTariffName");

        }


        //[TestMethod]
        //public void TestComparism_InvalidInputs_WorksNotExpected()
        //{
        //    //Arrange
        //    var item =_context.TariffProvider.FirstOrDefault(x => x.name == "Product B");
        //    item.BaseCostPerYear = 0;                      
        //    item.UnitKwhCostInEuro = 0;                  
        //    _context.SaveChanges();

        //    //Act
        //    var result = _context.GetCalculationResults(_context.TariffProvider.Include(x => x.ProductType).ToList(), 3500);

        //    //Asset
        //    Asset.Equals(result.Count, 2);
        //    Asset.Equals(result[0].AnnualCost, 0);
        //    Asset.Equals(result[1].AnnualCost, 830);

        //}
    }
}