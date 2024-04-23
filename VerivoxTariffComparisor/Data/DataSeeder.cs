using Microsoft.EntityFrameworkCore;
using VerivoxTariffComparisor.Model;

namespace VerivoxTariffComparisor.Data
{
    public class DataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
           using (var context = new TariffDbContext(serviceProvider.GetRequiredService<DbContextOptions<TariffDbContext>>()))
            {
                if (context.TariffProvider.Any())                
                    return; 

               context.TariffProvider.AddRange(
                      new Product
                      {
                          name = "Product A",
                          Type = 1,
                          IncludedKwhCost = 0,
                          BaseCostPerMonth = 5,
                          UnitKwhCostInEuro = 0.22
                      },
                      new Product
                      {
                          name = "Product B",
                          Type = 2,
                          BaseCostPerYear = 800,
                          IncludedKwhCost = 4000,
                          UnitKwhCostInEuro = 0.30

                      });

                context.TariffProviderType.AddRange(
                      new ProductType
                      {
                          Id = 1,
                          TypeName = "Basic Electricity Tariff"
                      },
                      new ProductType
                      {
                          Id = 2,
                          TypeName = "Packaged Tariff"

                      });
                context.SaveChanges();
            }

        }
    }
}
