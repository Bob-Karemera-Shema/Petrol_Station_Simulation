using System;
using System.IO;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Assignment2_Petrol_Skeleton
{
    class Program
    {
        /* The code below was copied from the internet. It is the code I used to maximise
           the console while the program is running. I maximized the console because
           my display could not fit in the default console size. It was copied from the
           the following website:
           https://www.c-sharpcorner.com/code/448/code-to-auto-maximize-console-application-according-to-screen-width-in-c-sharp.aspx
         */
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;

        static void Main(string[] args)
        {
            //The next two lines of code are also part of the code to maximize the
            //console size as mentioned above
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);

            //The next line of code marks the beginning of the data manipulation proccess
            Data.Initialise();

            /* The console needs to be updated after a given timeframe to give the
               user an impression of a real-time simulation of the petrol station
               in operation. The timer below is responsible for displaying the petrol station
               after every 2 seconds which is the interval used in this program. Everytime the
               petrol station is displayed change in information will be noticeable to the user.
            */

            //This leads to the use of the timer class which is a built-in class.
            // https://msdn.microsoft.com/en-us/library/system.timers.timer(v=vs.71).aspx
            //below we declare an object of the class
            Timer timer = new Timer();
            timer.Interval = 2000;
            timer.AutoReset = true; // repeat every 2 seconds
            timer.Elapsed += RunProgramLoop; // Setting up the Elapsed event handler for the timer
            timer.Enabled = true; //enable the the timer
            timer.Start(); //start the timer

            Console.ReadLine();
        }

        /*/// <summary>
        /// Gets whole number from keyboard (Console).
        /// </summary>
        /// <param name="message">Message to show to user</param>
        /// <param name="getNumber">True will return a number, False will return just a digit</param>
        /// <returns>Captured number</returns>*/
        
        /// <summary>
        /// Calls methods in the Display class to produce the display on the console
        /// </summary>
        /// <param name="sender">contains information of the timer object calling the event handler when the timer is elapsed</param>
        /// <param name="e">contains information about the timer elapsed event</param>
        static void RunProgramLoop(object sender, ElapsedEventArgs e)
        {
            Console.Clear();
            Display.DrawTitle();
            Console.WriteLine();
            Console.WriteLine();
            Display.DrawPumps();
            Console.WriteLine();
            Console.WriteLine();
            Data.AssignVehicleToPump();
            Display.ShowCounters();
            Console.WriteLine();
            Display.TransactionDisplay();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Display.ExitProgram();
        }
    }
}
