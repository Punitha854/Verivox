using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerivoxTariffComparisor.Model
{
    public class Product
    {
        [Key]
        [Required]
        public string name { get; set; }

        [ForeignKey("ProductType")]
        public int Type { get; set; }  
        public ProductType ProductType { get; set; }

        public double UnitKwhCostInEuro { get; set; }
        public double BaseCostPerMonth { get; set; }
        public double BaseCostPerYear { get; set; }
        public double AddtionalkwhCost { get; set; }
        public double IncludedKwhCost { get; set; }

        //Ctor
        public Product()
        {
            Type = 1;
            BaseCostPerMonth = 0;
            BaseCostPerYear = 0;
            UnitKwhCostInEuro = 0;
            IncludedKwhCost = 0;
        }
    }
}
