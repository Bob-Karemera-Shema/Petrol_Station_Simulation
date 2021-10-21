using System;

namespace Assignment2_Petrol_Skeleton
{
	public class Car : Vehicle
	{
		public static int maxFuel=40; //total tank capacity of a car
		public Car(string ftp,int ftm):base(ftp, ftm)
		{
		}
	}
}