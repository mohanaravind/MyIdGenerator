using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyIDGenerator.Code.AWS;
using Amazon.SQS.Model;

namespace MyIDGenerator.Code
{
    
    class Worker
    {
        private static Worker objWorker;
        private const Int32 _POLLTIME = 5000;

        private Worker()
        {
            
        }

        public static Worker GetWorker()
        {
            if(objWorker == null)
                objWorker = new Worker();

            return objWorker;
        }


        public void Start()
        {
            //Start the generation
            GeneratePrimeCounts();            
        }

        /// <summary>
        /// Generates the number of primes for each of the requests
        /// </summary>
        private void GeneratePrimeCounts()
        {
            //Declarations
            Message objMessage = new Message();           
            String strResponse = String.Empty;
            SQSUtility objSQSUtility = new SQSUtility();

            while (true)
            {
                objMessage = objSQSUtility.GetRequest();

                if (!objMessage.IsSetBody())
                {
                    Console.WriteLine("There are no requests currently. Will try again in 5 seconds");
                    System.Threading.Thread.Sleep(_POLLTIME);
                }
                else
                {
                    //Re-initialize
                    strResponse = String.Empty;

                    Console.WriteLine("----------------------------------------------------------------------------");
                    //Notify that processing has been started
                    Console.WriteLine("# Processing " + objMessage.Body);
                    
                    //Start processing the request                    
                    strResponse = ProcessRequest(objMessage.Body);

                    //Display the response message
                    Console.WriteLine(strResponse);
                    Console.WriteLine("----------------------------------------------------------------------------");
                    
                    //Post the result to the response queue
                    //objSQSUtility.PostResponse(strResponse);

                    //Insert data to the responses table
                    DynamoDBUtility.getDynamoDBUtility().InsertData("Responses", objMessage.MessageId, strResponse);
                    
                    //Delete data from requests table
                    DynamoDBUtility.getDynamoDBUtility().DeleteData("Requests", objMessage.MessageId);
                }
            }
            

        }

        /// <summary>
        /// Process the requests and generates the result
        /// </summary>
        /// <param name="strRequest"></param>
        /// <returns></returns>
        private String ProcessRequest(String strRequest)
        {
            //Declarations
            DateTime dtDateTime;
            String strTimeTaken = String.Empty;
            Int32 intPrimes = 0;
            StringBuilder sbProcessedResult = new StringBuilder();
            String[] arrRequest;
            String[] arrItem;
            Dictionary<String, String> dicRequest = new Dictionary<string, string>();
            String strLimit;
            String strPrimes = "Failed";
           
            arrRequest = strRequest.Split(new Char[] { ',' });

            foreach (String strItem in arrRequest)
            {
                //Get the individual item
                arrItem = strItem.Split(new Char[] { ':' });

                //Create the key value mapping
                dicRequest.Add(arrItem[0], arrItem[1]);
            }

            //Getting the limits
            dicRequest.TryGetValue("Limit", out strLimit);
            Int32.TryParse(strLimit, out intPrimes);

            //Get the current date time
            dtDateTime = DateTime.Now;
            
            //Get the number of primes
            intPrimes = PrimeCalculator.GetNumberOfPrimes(intPrimes);

            strTimeTaken = (DateTime.Now - dtDateTime).TotalSeconds.ToString();
            strTimeTaken += " seconds";

            //If successfully generated the primes
            if (intPrimes != Int32.MinValue)
                strPrimes = intPrimes.ToString();

            //Construct the result
            sbProcessedResult.Append(strRequest);
            sbProcessedResult.Append(",");
            sbProcessedResult.Append("Count:");
            sbProcessedResult.Append(strPrimes);
            sbProcessedResult.Append(",");
            sbProcessedResult.Append("Time:");
            sbProcessedResult.Append(strTimeTaken);
            
            return sbProcessedResult.ToString();
        }



    }
}
