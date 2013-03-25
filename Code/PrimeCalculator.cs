using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyIDGenerator.Code
{
    class PrimeCalculator
    {
        /// <summary>
        /// Gets the number of primes till the limiting number
        /// Uses Sieve of Eratosthenes method
        /// </summary>
        /// <remarks>Aravind Mohan</remarks>
        /// <param name="intLimitingNumber"></param>
        /// <returns></returns>
        public static Int32 GetNumberOfPrimes(Int32 intLimitingNumber)
        {
            //Declarations
            Int32 intNumberOfPrimes = Int32.MinValue;

            try
            {
                List<Int32> lstNumbers = Enumerable.Range(2, intLimitingNumber - 2).ToList<Int32>();
                Int32 intPrimeToCancel;
                Int32 intIndex = 0;
                Int32 intNumbersLengthBeforeCancellation;
                Int32 intNumbersLengthAfterCancellation;

                do
                {
                    //Get the prime number to cancel out
                    intPrimeToCancel = lstNumbers[intIndex];

                    //Get the length
                    intNumbersLengthBeforeCancellation = lstNumbers.Count;

                    lstNumbers.RemoveAll(intNumber => { return (intNumber > intPrimeToCancel && intNumber % intPrimeToCancel == 0); });

                    //Get the length
                    intNumbersLengthAfterCancellation = lstNumbers.Count;

                    //Increment the index
                    intIndex++;
                } while (intNumbersLengthBeforeCancellation != intNumbersLengthAfterCancellation);

                intNumberOfPrimes = lstNumbers.Count;
            }
            catch (Exception e)
            {
                intNumberOfPrimes = Int32.MinValue;
                Console.WriteLine(e.Message);
            }


            return intNumberOfPrimes;
        }
    }
}
