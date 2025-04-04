using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JOIEnergy.Domain;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JOIEnergy.Controllers
{
    [Route("readings")]
    public class MeterReadingController : Controller
    {
        private readonly IMeterReadingService _meterReadingService;

        public MeterReadingController(IMeterReadingService meterReadingService)
        {
            _meterReadingService = meterReadingService;
        }

        // POST api/values
        // Kevin Broit: method documentation could be improved: What it does + Input/Output
        [HttpPost ("store")]
        public ObjectResult Post([FromBody]MeterReadings meterReadings) //Kevin Broit: Method name could be more descriptive: e.g.: StoreReadings
        {   
            if (!IsMeterReadingsValid(meterReadings)) {
                // Kevin Broit: Server error start from code range 500, and invalid request start at 400. Incongruence btw. message and response.
                return new BadRequestObjectResult("Internal Server Error");
            }
            
            // Kevin Broit: What if the storage fails? Error control shall be added. e.g.: return a boolean flag.
            _meterReadingService.StoreReadings(meterReadings.SmartMeterId,meterReadings.ElectricityReadings);
            
            //Kevin Broit: Returning and empty object makes no benefit. Return can be Ok() and signature IActionResult
            return new OkObjectResult("{}");
        }

        private bool IsMeterReadingsValid(MeterReadings meterReadings)
        {
            //Kevin Broit: This validation could be done in the model itself using annotations on the fileds: e.g: Required. 
            string smartMeterId = meterReadings.SmartMeterId;
            List<ElectricityReading> electricityReadings = meterReadings.ElectricityReadings;
        
            //Kevin Broit: Semantic could be improved: string.IsNullOrWhiteSpace
            return smartMeterId != null && smartMeterId.Any()
                    && electricityReadings != null && electricityReadings.Any();
        }

        [HttpGet("read/{smartMeterId}")]
        public ObjectResult GetReading(string smartMeterId) {
            return new OkObjectResult(_meterReadingService.GetReadings(smartMeterId));
        }
    }
}
