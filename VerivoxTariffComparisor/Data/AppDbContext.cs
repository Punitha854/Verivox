using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection.Emit;
using VerivoxTariffComparisor.Controllers;
using VerivoxTariffComparisor.Model;

namespace VerivoxTariffComparisor.Data
{
    public class TariffDbContext(DbContextOptions<TariffDbContext> options) : DbContext(options)   
    {
        public DbSet<Product> TariffProvider { get; set; }
        public DbSet<ProductType> TariffProviderType { get; set; }


        public List<TariffComparismResult> GetCalculationResults(List<Product> tariffProducts, double consumption)
        {
            var result = new List<TariffComparismResult>();
            foreach (var product in tariffProducts)
            {
                var tariffResult = new TariffComparismResult();
                try
                {
                    double consumedUnits = consumption; double basecost = 0;
                    
                    if (product != null)
                    {                        
                        if (product.IncludedKwhCost > 0)
                            consumedUnits = product.IncludedKwhCost < consumption ? consumption - product.IncludedKwhCost : 0;

                        basecost = product.BaseCostPerMonth > 0 ? product.BaseCostPerMonth * 12 : product.BaseCostPerYear;

                        tariffResult.AnnualCost = (consumedUnits * product.UnitKwhCostInEuro) + basecost;
                        tariffResult.TariffName = product.ProductType.TypeName;
                    }
                    else
                    {
                        tariffResult.Message = "Invalid Product " + product.name;
                    }
                    
                }
                catch (Exception Ex )
                {
                    tariffResult.Message = Ex.Message;
                }
                result.Add(tariffResult);
            }
            result = result.OrderBy(x => x.AnnualCost).ToList();
            return result;

        }

    }
}
