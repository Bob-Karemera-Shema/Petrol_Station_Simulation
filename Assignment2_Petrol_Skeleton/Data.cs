using System.IO;
using System.Collections.Generic;
using System.Timers;
using System;

namespace Assignment2_Petrol_Skeleton
{
    class Data
    {
        private static Timer timer; //timer used in this class
        public static List<Vehicle> vehicles; //list of vehicles in the Forecourt
        public static List<Pump> pumps; //list of pumps at the station
        
        public static List<Vehicle> servicedVehicles= new List<Vehicle>();  //counter4 the list of all vehicles serviced
        public static float totalUnleadedDispensed = 0f;    //counter1a for the amount of unleaded didspensed during the app's lifetime
        public static float totalDieselDispensed = 0f;    //counter1b for the amount of unleaded didspensed during the app's lifetime
        public static float totalLPGDispensed = 0f;    //counter1c for the amount of unleaded didspensed during the app's lifetime
        public static float totalRevenue = 0f; //counter2 for the equivalent amount of money for the dispensed fuel
        public static float commision = 0f;     //counter3 for commission paid to fuel attendants
        public static double[] priceLitre = { 2.0, 2.5, 3.0 };
        //price of unleaded is £2.0/litre
        //price of Diesel is £2.5/litre
        //price of LPG is £3.0/litre

        public static double dispensingRate = 10; //a pump dispenses 10 litres/second
        private static string[] fuelType = { "Unleaded", "Diesel", "LPG" }; //array holding the types of fuel dispensed at the station
        private static string[] vehicleType = { "Car", "Van", "HGV" }; //array holding the types of cars serviced at the station
        public static int vehicleLeft = 0; //counter5 for the number of vehicles that left before being serviced
        public static float fuelAttendantWage = 0f;

        public static Vehicle createdVehicle; 
        //assigned the value of a created vehicles to easily identify it when it leaves the forecourt before service
        
        public static string transaction = "Detailedlisttransaction.txt";
        /*the name of the file where every transaction will be recorded with full details
        this is in line with counter6. I used a text file instead of a list, as a transaction record is useful for
        other managerial purposes outside the scope of the app. The text file is permanent recorda and is found
        in program folder*/


        private static Random random = new Random(); //random used in this class for all action requiring a random

        /// <summary>
        /// calls methods that instantiate objects of class Vehicle and Pump used in the progam
        /// </summary>
        public static void Initialise() {
            InitialisePumps();
            InitialiseVehicles();
            FileTitle();
        }

        /// <summary>
        /// instantiates a list object of class vehicle which will store vehicles at the forecourt
        /// contains timer for the creation of vehicles in the forecourt
        /// </summary>
        private static void InitialiseVehicles()
        {
                vehicles = new List<Vehicle>(); //list of vehicles in the forecourt
                
                //timer for the creation of vehicles in the forecourt during the app's running time
                timer = new Timer();
                timer.Interval = RandomVehicleCreation(); //the RandomVehicleCreation method will provide the interval of the timer
                timer.AutoReset = true;
                timer.Elapsed += CreateVehicle;
                timer.Enabled = true;
                timer.Start();
        }

        /// <summary>
        /// Creates a vehicle in the forecourt
        /// </summary>
        /// <param name="sender">contains information of the timer object calling the event handler when the timer is elapsed</param>
        /// <param name="e">contains information about the timer elapsed event</param>
        private static void CreateVehicle(object sender, ElapsedEventArgs e)
        {        
            if (vehicles.Count < 5) //vehicles in the forecourt can't exceed the number of 5
            {
                float quarterTank;
                //the quarter of the maximum of a tank which is the limit
                //when a new vehicle is created
                string type = VehicleTypeGenerator(); //calling VehicleTypeGenerator method which returns a random type of vehicle to create

                if (type == "Car")
                {
                    quarterTank = (float)Car.maxFuel / 4;
                    Vehicle v = new Car(FuelTypeGenerator(), Fueltime(quarterTank,Car.maxFuel));
                    vehicles.Add(v);
                    createdVehicle = v; //the vehicle created is assigned to createdVehicle
                    VehicleLifeTime(); //calls VehicleLifeTime which starts the vehicles waiting time in the forecourt
                }
                else if (type == "Van")
                {
                    quarterTank = (float)Van.maxFuel / 4;
                    Vehicle v = new Van(FuelTypeGenerator(1,2), Fueltime(quarterTank,Van.maxFuel));
                    vehicles.Add(v);
                    createdVehicle = v;
                    VehicleLifeTime();
                }
                else
                {
                    quarterTank = (float)HGV.maxFuel / 4;
                    Vehicle v = new HGV(fuelType[1], Fueltime(quarterTank,HGV.maxFuel));
                    vehicles.Add(v);
                    createdVehicle = v;
                    VehicleLifeTime();
                }
            }
            else { return; }
        }

        /// <summary>
        /// uses a timer to start a vehicles waiting time in the forecourt before it leaves
        /// </summary>
        private static void VehicleLifeTime()
        {
            timer = new Timer();
            timer.Interval = WaitingTime(); //WaitingTime method will provide the timer interval
            timer.AutoReset = false; //this timer won't repeat because a vehicle can wait only once. the timer executes only once
            timer.Elapsed += RemoveVehicle;
            timer.Enabled = true;
            timer.Start();
        }

        /// <summary>
        /// removes a vehicle from the forecourt by removing it from the vehicles list
        /// </summary>
        /// <param name="sender">contains information of the timer object calling the event handler when the timer is elapsed</param>
        /// <param name="e">contains information about the timer elapsed event</param>
        private static void RemoveVehicle(object sender,ElapsedEventArgs e)
        {
            vehicles.Remove(createdVehicle);
            vehicleLeft++;
        }


        /// <summary>
        /// uses a random object to generate a random time between 1500 and 2200 milliseconds to create a vehicle
        /// https://docs.microsoft.com/en-us/dotnet/api/system.random?redirectedfrom=MSDN&view=netcore-3.1
        /// </summary>
        /// <returns>random time to create a vehicle</returns>
        private static int RandomVehicleCreation()
        {
            return random.Next(1500, 2201);
            //when using a random, exceed the upper limit by 1 to include the upper limit in the
            //pool of numbers to be randomly picked.
            //you don't have to worry about this for the lower limit as it is automatically included
        }

        /// <summary>
        /// uses a random object to generate a random time between 1000 and 2000 milliseconds
        /// to remove a vehicle from the foreccourt
        /// </summary>
        /// <returns>random time to remove a vehicle from forecourt</returns>
        private static int WaitingTime()
        {
            return random.Next(1000, 2001);
        }

        /// <summary>
        /// calculates the random fuelling time of a vehicle but also taking into account
        /// the amount of fuel in the tank 
        /// </summary>
        /// <param name="quarter">quarter of total tank capacity depending on vehicle type</param>
        /// <param name="max_fuel">total tank capacity depending on vehicle type</param>
        /// <returns>random fuel time</returns>
        private static int Fueltime(float quarter, int maxFuel)
        {
            int fTime;
            //the maximum amount of time a vehicle can spend at the pump depending on the amount
            //of fuel already in the car
            float randomFuel;
            //random fuel in vehicle when created;

            randomFuel = (float)(random.NextDouble() * quarter);
            fTime = (int)(((maxFuel - randomFuel) / dispensingRate) * 1000);
            /*the amount of fuel in a vehicle at creation is subtracted from
            the maximum its fuel tank can hold to get the missing amount. This is then divided
            by the dispensing rate of the pump to get the time it will spend at the pump in seconds.
            The seconds are then multiplied by 1000 to get the equivalent milliseconds*/
            return fTime;
        }

        /// <summary>
        /// randomly generates a fuel type when a car is created 
        /// a car can run on all three type of fuel
        /// </summary>
        /// <returns>random fuel type for car</returns>
        private static string FuelTypeGenerator()
        {
            int index = random.Next(fuelType.Length);
            return fuelType[index];
        }

        /// <summary>
        /// randomly generates a fuel type when a van is created but bearing in mind
        /// that a van only runs on Diesel and LPG
        /// the fuel type is randomly picked from these two
        /// </summary>
        /// <param name="a">index of diesel fuel type location in fuelType array</param>
        /// <param name="b">index of LPG fuel type location in fuelType array</param>
        /// <returns>random fuel type for van</returns>
        private static string FuelTypeGenerator(int a, int b)
        {
            int index = random.Next(a, b);
            return fuelType[index];
        }

        /// <summary>
        /// randomly generates a vehicle type to be created in the forecourt
        /// </summary>
        /// <returns>random type of vehicle</returns>
        private static string VehicleTypeGenerator()
        {
            int index = random.Next(vehicleType.Length);
            return vehicleType[index];
        }

        /// <summary>
        /// creates a list of pumps at the petrol station
        /// </summary>
        private static void InitialisePumps()
        {
            pumps = new List<Pump>();

            Pump p;

            for (int i = 0; i < 9; i++)
            {
                p = new Pump(fuelType);
                pumps.Add(p);
            }
        }

        /// <summary>
        /// writes titles of columns of a table of records where all transaction are recorded in the text file
        /// </summary>
        private static void FileTitle()
        {
            StreamWriter title = new StreamWriter(transaction, true);
            title.WriteLine("Vehicle Type\tFuel Type\tNumber of litres\tPump Number"); // the title at the beginning of the file.
            title.Close();
        }

        /// <summary>
        /// assigns a vehicle to a pump by first checking if it's available
        /// </summary>
        public static void AssignVehicleToPump()
        {
            Vehicle v;
            Pump p;
            int j=0;

            while(j<9)
            {
                if (vehicles.Count == 0) { return; }
                for (int i = j; i <= j+2; i++)
                {

                    p = pumps[i];

                    //before assigning a vehicle, we have to make sure that a vehicle is not assigned to the first available
                    //pump but instead the last available pump
                    //this is to make sure vehicles don't block passage for other vehicles to access other pumps

                    if (p.IsAvailable())
                    {
                        if(i%3==2)
                        {
                            v = vehicles[0]; // get first vehicle
                            v.pumpNumber = i+1;
                            vehicles.RemoveAt(0); // remove vehicle from queue
                            p.AssignVehicle(v); // assign it to the pump
                            break;
                        }
                        continue;
                    }
                    else if(!p.IsAvailable())
                    {
                        if (i % 3 == 0) { break; }
                        p = pumps[--i];
                        v = vehicles[0]; // get first vehicle
                        v.pumpNumber = i+1;
                        vehicles.RemoveAt(0); // remove vehicle from queue
                        p.AssignVehicle(v); // assign it to the pump
                        break;
                    }

                    if (vehicles.Count == 0) { break; }
                }
                j += 3;
            }
        }
    }
}
