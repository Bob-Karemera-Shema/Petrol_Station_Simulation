using System;

namespace Assignment2_Petrol_Skeleton
{
	public class HGV : Vehicle
	{
		public static int maxFuel = 150; //total tank capacity of a HGV

		public HGV(string ftp, int ftm) : base(ftp, ftm)
		{
		}
	}
}
