using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Amazon.SQS;
using Amazon.Runtime;
using MyIDGenerator.Code;
using System.Threading;

namespace MyIDGenerator
{
    class Program
    {
        [MTAThread]
        public static void Main(string[] args)
        {
            //Declarations
            ConsoleKey ki = ConsoleKey.A;            
            Thread thWorker;

            //Create the worker thread
            thWorker = new Thread(new ThreadStart(Worker.GetWorker().Start));
                                                         
            //Start the worker
            thWorker.Start();
                        
            //Display the message
            Console.Out.WriteLine("Press enter to quit anytime");

            //Get the user input
            ki = Console.ReadKey().Key;    
            
            if(ki == ConsoleKey.Enter)
               //Abort the running thread
               thWorker.Abort();            
        }

        
      
    }
}