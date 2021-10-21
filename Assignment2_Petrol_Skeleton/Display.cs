using System;
using System.IO;
namespace Assignment2_Petrol_Skeleton
{
    public class Display
    {
		/// <summary>
		/// displays the name of the petrol station on the console
		/// </summary>
		public static void DrawTitle()
		{
			Console.WriteLine("\t\t\t\t\t--------------------------------------------------------------------------------------------");
			Console.WriteLine("\t\t\t\t\t -------------------------------PETROL TRULY UNLIMITED LTD--------------------------------- ");
			Console.WriteLine("\t\t\t\t\t--------------------------------------------------------------------------------------------");
		}

		/// <summary>
		/// draw the pumps on the console
		/// </summary>
		public static void DrawPumps()
		{
			Pump p;

			Console.WriteLine("Pumps Status:");

			for (int i = 0; i < 9; i++)
			{
				p = Data.pumps[i];


				Console.Write("PUMP #{0} ", i + 1);
				if (p.IsAvailable()) { Console.Write("FREE"); }
				else { Console.Write("BUSY"); }
				Console.Write("      |");

                // modulus -> remainder of a division operation
                // 0 % 3 => 0 (0 / 3 = 0 R=0)
                // 1 % 3 => 1 (1 / 3 = 0 R=1)
                // 2 % 3 => 2 (2 / 3 = 0 R=2)
                // 3 % 3 => 0 (3 / 3 = 1 R=0)
                // 4 % 3 => 1 (4 / 3 = 1 R=1)
                // 5 % 3 => 2 (5 / 3 = 1 R=2)
                // 6 % 3 => 0 (6 / 3 = 2 R=0)
                // ...
				if (i % 3 == 2) 
				{ 
					Console.WriteLine();
					CurrentVehicleAtPump(i);
					Console.WriteLine();
					Console.WriteLine();
				}
			}
		}

		/// <summary>
		/// displays information about a vehicle at a certain pump
		/// </summary>
		/// <param name="a"></param>
		private static void CurrentVehicleAtPump(int a)
		{
			Pump p;

			for(int i=a-2;i<=a;i++)
			{
				p = Data.pumps[i];
				if (p.IsAvailable()) 
				{
					Console.Write("                  |");
					continue; 
				}
				else
				{
					Console.Write("Vehicle Type: {0} |", p.currentVehicle.GetType().Name);
				}
			}
			Console.WriteLine();
			for (int i = a - 2; i <= a; i++)
			{
				p = Data.pumps[i];
				if (p.IsAvailable()) 
				{
					Console.Write("                  |");
					continue; 
				}
				else
				{
					if (p.currentVehicle.fuelType == "Unleaded")
					{
						Console.Write("Fuel Type:{0}|", p.currentVehicle.fuelType);
					}
					else if(p.currentVehicle.fuelType == "Diesel")
					{
						Console.Write("Fuel Type: {0} |", p.currentVehicle.fuelType);
					}
					else
					{
						Console.Write("Fuel Type: {0}    |", p.currentVehicle.fuelType);
					}
				}
			}
			Console.WriteLine();
			for (int i = a - 2; i <= a; i++)
			{
				p = Data.pumps[i];
				if (p.IsAvailable()) 
				{
					Console.Write("                  |");
					continue; 
				}
				else
				{
					if (p.currentVehicle.fuelTime >= 10000)
					{
						Console.Write("Fuel Time: {0}  |", p.currentVehicle.fuelTime);
					}
					else
					{
						Console.Write("Fuel Time: {0}   |", p.currentVehicle.fuelTime);
					}
				}
			}
		}

		/// <summary>
		/// displays forecourt information and counters on the console
		/// also writes all transactions in the text file to keep record of the transactions
		/// </summary>
		public static void ShowCounters()
		{
			Console.Write("FORECOURT:\t\t\t\t\t\t\t\t\t\t");
			Console.WriteLine("RECORDS:");

			Console.Write("Vehicles Waiting : {0}\t\t\t\t\t\t\t\t\t", Data.vehicles.Count);
			Console.WriteLine("Unleaded litres dispensed   : {0}", Data.totalUnleadedDispensed);

			Console.Write("Vehicles serviced: {0}\t\t\t\t\t\t\t\t\t", Data.servicedVehicles.Count);
			Console.WriteLine("Diesel litres dispensed     : {0}", Data.totalDieselDispensed);

			Console.Write("Vehicles that left before being serviced: {0}\t\t\t\t\t\t", Data.vehicleLeft);
			Console.WriteLine("LPG litres dispensed        : {0}", Data.totalLPGDispensed);

			Console.Write("                      \t\t\t\t\t\t\t\t\t");
			Console.WriteLine("Earnings on litres dispensed: {0}", Data.totalRevenue);

			Console.Write("                      \t\t\t\t\t\t\t\t\t");
			Console.WriteLine("Commission on earnings      : {0}", Data.commision);

			Console.Write("                      \t\t\t\t\t\t\t\t\t");
			Console.WriteLine("Fuel Attendant Wage         : {0}", Data.fuelAttendantWage);

			Console.WriteLine();


			StreamWriter trans = new StreamWriter(Data.transaction,true);
			Data.servicedVehicles.ForEach(Vehicle =>
			{
				if (Vehicle.fuelType == "Unleaded")
				{
					if (Vehicle.GetType().Name == "Car")
					{
						trans.WriteLine("{0}\t\t{1}\t\t{2}\t\t\t{3}",
					  Vehicle.GetType().Name,
					  Vehicle.fuelType,
					  (Vehicle.fuelTime * Data.dispensingRate / 1000),
					  Vehicle.pumpNumber);
					}
					else
					{
						trans.WriteLine("{0}\t\t{1}\t\t{2}\t\t{3}",
						  Vehicle.GetType().Name,
						  Vehicle.fuelType,
						  (Vehicle.fuelTime * Data.dispensingRate / 1000),
						  Vehicle.pumpNumber);
					}
				}
				else
				{
					trans.WriteLine("{0}\t\t{1}\t\t\t{2}\t\t\t{3}",
					  Vehicle.GetType().Name,
					  Vehicle.fuelType,
					  (Vehicle.fuelTime * Data.dispensingRate / 1000),
					  Vehicle.pumpNumber);
				}
			}
			);

			trans.Close();
		}

		/// <summary>
		/// displays a few transactions on the console to give the user an idea of how a transaction is recorded
		/// and what's happening at the pumps
		/// </summary>
		public static void TransactionDisplay()
		{
			int listLimit = 0;
			Console.WriteLine("");
			Console.WriteLine("\t\t\t\t\t\t\t\t-----PREVIOUS TRANSACTIONS-----");
			Console.WriteLine("");
			Console.WriteLine("\t\t\t\t\t\tVehicle Type\tFuel Type\tNumber of litres\tPump Number");
			Data.servicedVehicles.ForEach(Vehicle =>
			{
				if(listLimit<5)
				{
					if (Vehicle.fuelType == "Unleaded")
					{
						if (Vehicle.GetType().Name == "Car")
						{
							Console.WriteLine("\t\t\t\t\t\t{0}\t\t{1}\t\t{2}\t\t\t{3}",
						  Vehicle.GetType().Name,
						  Vehicle.fuelType,
						  (Vehicle.fuelTime * Data.dispensingRate / 1000),
						  Vehicle.pumpNumber);
						}
						else
						{
							Console.WriteLine("\t\t\t\t\t\t{0}\t\t{1}\t\t{2}\t\t{3}",
							  Vehicle.GetType().Name,
							  Vehicle.fuelType,
							  (Vehicle.fuelTime * Data.dispensingRate / 1000),
							  Vehicle.pumpNumber);
						}
					}
					else
					{
						Console.WriteLine("\t\t\t\t\t\t{0}\t\t{1}\t\t\t{2}\t\t\t{3}",
						  Vehicle.GetType().Name,
						  Vehicle.fuelType,
						  (Vehicle.fuelTime * Data.dispensingRate / 1000),
						  Vehicle.pumpNumber);
					}
				}
				else { return; }
				listLimit++;
			}
			);
		}

		/// <summary>
		/// tells the user how to exit the program
		/// </summary>
		public static void ExitProgram()
		{
			Console.WriteLine("Press ENTER key to exit");
		}
    }
}
