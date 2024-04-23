using System.ComponentModel.DataAnnotations;

namespace VerivoxTariffComparisor.Model
{
    public class ProductType
    {

            [Key]
            public int Id { get; set; }
            public string TypeName { get; set; }
           
    }
}
