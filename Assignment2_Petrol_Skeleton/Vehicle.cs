using System;

namespace Assignment2_Petrol_Skeleton
{
    public class Vehicle
    {
        public string fuelType;
        public double fuelTime;
		public static int nextCarID = 0;
		public int carID;
        public int pumpNumber=0;
        public Vehicle(string ftp, double ftm)
        {
            fuelType = ftp;
            fuelTime = ftm;
			carID = nextCarID++;
        }

        public Vehicle()
        {
        }
    }
}
