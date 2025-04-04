using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Enums;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JOIEnergy.Controllers
{
    [Route("price-plans")]
    public class PricePlanComparatorController : Controller
    {
        public const string PRICE_PLAN_ID_KEY = "pricePlanId";
        public const string PRICE_PLAN_COMPARISONS_KEY = "pricePlanComparisons";
        private readonly IPricePlanService _pricePlanService;
        private readonly IAccountService _accountService;

        public PricePlanComparatorController(IPricePlanService pricePlanService, IAccountService accountService)
        {
            this._pricePlanService = pricePlanService;
            this._accountService = accountService;
        }

        [HttpGet("compare-all/{smartMeterId}")]
        // Kevin Broit: Use standarized C# nomenclature for methods
        public ObjectResult CalculatedCostForEachPricePlan(string smartMeterId)
        {
            string pricePlanId = _accountService.GetPricePlanIdForSmartMeterId(smartMeterId);
            // Kevin Broit: the null check with the  return statement is forgotten.

            Dictionary<string, decimal> costPerPricePlan = _pricePlanService.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);
            if (!costPerPricePlan.Any())
            {
                //Kevin Broit: returned message is incorrect.
                return new NotFoundObjectResult(string.Format("Smart Meter ID ({0}) not found", smartMeterId));
            }

            //Kevin Broit: The return object is coupled to constants. Instead of that a DTO shall be created.
            return new ObjectResult(new Dictionary<string, object>() {
                {PRICE_PLAN_ID_KEY, pricePlanId},
                {PRICE_PLAN_COMPARISONS_KEY, costPerPricePlan},
            });
        }

        [HttpGet("recommend/{smartMeterId}")]
        // Kevin Broit: Use standarized C# nomenclature for methods
        public ObjectResult RecommendCheapestPricePlans(string smartMeterId, int? limit = null) {
            // Kevin Broit: Parameter check for limit has been forgotten.

            var consumptionForPricePlans = _pricePlanService.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);

            if (!consumptionForPricePlans.Any()) {
                //Kevin Broit: returned message is incorrect.
                return new NotFoundObjectResult(string.Format("Smart Meter ID ({0}) not found", smartMeterId));
            }

            // Kevin Broit: The following code exceeds controller responsability. It shall be moved out to the service.
            var recommendations = consumptionForPricePlans.OrderBy(pricePlanComparison => pricePlanComparison.Value);
            if (limit.HasValue && limit.Value < recommendations.Count())
            {
                return new ObjectResult(recommendations.Take(limit.Value));
            }

            return new ObjectResult(recommendations);
        }
    }
}
