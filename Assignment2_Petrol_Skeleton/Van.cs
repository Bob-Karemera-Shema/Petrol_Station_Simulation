using System;

namespace Assignment2_Petrol_Skeleton
{
	public class Van : Vehicle
	{
		public static int maxFuel = 80; //total tank capacity of a van

		public Van(string ftp, int ftm) : base(ftp, ftm)
		{
		}
	}
}
