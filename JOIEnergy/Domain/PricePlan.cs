using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Enums;

namespace JOIEnergy.Domain
{
    public class PricePlan
    {
        public string PlanName { get; set; }
        public Supplier EnergySupplier { get; set; }
        public decimal UnitRate { get; set; }
        
        //Kevin Broit: Name of lists shall be plural
        public IList<PeakTimeMultiplier> PeakTimeMultiplier { get; set;}

        // Kevin Broit: If only the day of week is neccesary, the parameter shall be changed to receive an enum.
        public decimal GetPrice(DateTime datetime) {
            //Kevin Broit: implement safeguard null check for PeakTimeMultiplier 
            //Kevin Broit: And what about the time? meaning: are prices the same for day and night?
            var multiplier = PeakTimeMultiplier.FirstOrDefault(m => m.DayOfWeek == datetime.DayOfWeek);
                
            //Kevin Broit: varible name multiplier shall be peakTimeMultiplier. 

            if (multiplier?.Multiplier != null) {
                return multiplier.Multiplier * UnitRate;
            } else {
                return UnitRate;
            }
        }
    }

    public class PeakTimeMultiplier
    {
        // Kevin Broit: Validation for multiplier attribute shall be added
        public DayOfWeek DayOfWeek { get; set; }
        public decimal Multiplier { get; set; }
    }
}
