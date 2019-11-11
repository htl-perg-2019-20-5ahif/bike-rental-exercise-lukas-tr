using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRental
{
    public interface ICostCalculator
    {
        decimal calculateRentalPrice(DateTime begin, DateTime end, decimal bikePriceFirstHour, decimal bikePriceAdditionalHour);
    }
    public class CostCalculator : ICostCalculator
    {
        public decimal calculateRentalPrice(DateTime begin, DateTime end, decimal bikePriceFirstHour, decimal bikePriceAdditionalHour)
        {
            var duration = end - begin;
            if(duration < TimeSpan.Zero)
            {
                throw new InvalidRentalDurationException();
            }
            decimal total = 0;
            if (duration >= TimeSpan.FromMinutes(15))
            {
                total += bikePriceFirstHour;
            }
            var additionalHours = (int) Math.Ceiling((duration - TimeSpan.FromHours(1)).TotalHours);
            if(additionalHours > 0)
            {
                total += bikePriceAdditionalHour * additionalHours;
            }

            return total;
        }
    }
}
