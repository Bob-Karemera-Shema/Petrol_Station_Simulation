using System;
using System.Timers;

namespace Assignment2_Petrol_Skeleton
{
    class Pump
    {
        public Vehicle currentVehicle = null;
        public string[] fuelType = new string[3]; //types of fuel a pump is capable of dispensing

        public Pump(string[] ftp)
        {
            fuelType = ftp;
        }

        /// <summary>
        /// checks the availability of a pump
        /// </summary>
        /// <returns>TRUE if currentVehicle is NULL, meaning it's available</returns>
        /// <returns>FALSE if currentVehicle is NOT NULL, meaning it's not available</returns>
        public bool IsAvailable()
        {
            return currentVehicle == null;
        }

        /// <summary>
        /// assignes a vehicle to a pump and uses a timer to make sure it stays at the pump
        /// for an amount of time which equals to its fuel time
        /// </summary>
        /// <param name="v">vehicle assigned to the pump</param>
        public void AssignVehicle(Vehicle v)
        {
            currentVehicle = v;

            Timer timer = new Timer();
            timer.Interval = v.fuelTime;
            timer.AutoReset = false; // the vehicle is only assigned to a pump once
            timer.Elapsed += ReleaseVehicle;
            timer.Enabled = true;
            timer.Start();
        }

        /// <summary>
        /// removes the vehicle from the pump and updates counters 1 to 4 and 6
        /// </summary>
        /// <param name="sender">contains information of the timer object calling the event handler when the timer is elapsed</param>
        /// <param name="e">contains information about the timer elapsed event</param>
        public void ReleaseVehicle(object sender, ElapsedEventArgs e)
        {
            float dispensed_litres;
            //the variable will hold the amount dispensed in a vehicle
            //it will help when updating the counters

            //as the vehicle leaves the pump it is registered on the list of serviced vehicles
            //and the transaction is recorded
            Data.servicedVehicles.Add(currentVehicle);

            /*casting will be used because the values used to update the counters are different from the counters in
            terms of datatypes*/

            dispensed_litres = (float)((currentVehicle.fuelTime * Data.dispensingRate)/1000);
            // the pump dispenses at a rate of 1.5 litres/second

            if (currentVehicle.fuelType=="Unleaded")
            {                
                Data.totalUnleadedDispensed += dispensed_litres;                
                Data.totalRevenue += (float)(dispensed_litres * Data.priceLitre[0]);
                // the price of unleaded is £2.0/litre
            }
            if (currentVehicle.fuelType == "Diesel")
            {
                Data.totalDieselDispensed += dispensed_litres;
                Data.totalRevenue += (float)(dispensed_litres * Data.priceLitre[1]);
                // the price of diesel is £2.5/litre
            }
            if (currentVehicle.fuelType == "LPG")
            {
                Data.totalLPGDispensed += dispensed_litres;
                Data.totalRevenue += (float)(dispensed_litres * Data.priceLitre[2]);
                // the price of LPG is £3.0/litre
            }

            Data.commision = (float)(Data.totalRevenue * 0.01); // commision is 1% of total revenue
            Data.fuelAttendantWage = (float)(Data.commision + 2.49) * 8;

            currentVehicle = null;
        }
    }
}
