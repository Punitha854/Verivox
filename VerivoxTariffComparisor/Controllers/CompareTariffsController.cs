using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VerivoxTariffComparisor.Data;
using VerivoxTariffComparisor.Model;

namespace VerivoxTariffComparisor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompareTariffsController : ControllerBase
    {

        private readonly TariffDbContext _context;

        public CompareTariffsController(TariffDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// This method list the claculation result between predefined product A and Product B
        /// </summary>
        /// <param name="consumption">Total electrcity consumed in Kwh</param>
        /// <returns>List of annual costs for tariffs</returns>

        [Route("/CompareProductAProductBTariffs")]
        [HttpGet()]

        public List<TariffComparismResult> CompareProductAProductBTariffs(double consumption)
        {
            var products = _context.TariffProvider.Where(x => new string[2] { "Product A", "Product B" }.Contains(x.name)).Include(x => x.ProductType).ToList();
            return _context.GetCalculationResults(products, consumption);
        }

        /// <summary>
        /// This Method list the calculation results between any number of tariffproducts identified by their name
        /// </summary>
        /// <param name="tariffProducts"></param>
        /// <param name="consumption">Total electrcity consumed in Kwh</param>
        /// <returns>List of annual costs for tariffs</returns>
        [Route("/CompareGivenProductTariffs")]
        [HttpGet]
        public List<TariffComparismResult> CompareTariffs(string tariffProducts, double consumption)
        {
            var item =  tariffProducts.Split(",",StringSplitOptions.TrimEntries).ToList();
            var products = _context.TariffProvider.Where(x => item.Contains(x.name)).Include(y => y.ProductType).ToList();
            return _context.GetCalculationResults(products, consumption);            
        }

        /// <summary>
        /// return the calculation results fof all the Tariffs
        /// </summary>
        /// <param name="consumption">Total electrcity consumed in Kwh</param>
        /// <returns>List of annual costs for tariffs</returns>

        [Route("/CompareAllProductTariffs")]
        [HttpGet()]
        public List<TariffComparismResult> CompareTariffs(double consumption)
        {
            var products = _context.TariffProvider.Include(x =>x.ProductType).ToList();
            return _context.GetCalculationResults(products, consumption);
        }
    }

    public class  TariffComparismResult
    {
        public TariffComparismResult()
        {
            AnnualCost = 0;
            TariffName = "";
            Message = "AnnualCost calculated sucessfully";
        }
        /// <summary>
        ///  Name of the Product which has the tariff
        /// </summary>
        public string TariffName { get; set; }
        /// <summary>
        /// Total electricity cost per year to be paid
        /// </summary>
        public double AnnualCost { get; set; }
        /// <summary>
        /// Result of the calculation process
        /// </summary>
        public string Message { get; set; }
       
    }
}
