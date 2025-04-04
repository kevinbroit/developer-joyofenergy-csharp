using System;
namespace JOIEnergy.Domain
{
    public class ElectricityReading
    {
        //Kevin Broit: If only the time is necessary why not using TimeSpan. 
        public DateTime Time { get; set; }

        // Kevin Broit: Use base type decimal
        public Decimal Reading { get; set; }
    }
}
