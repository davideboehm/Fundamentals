namespace Functional.Math
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public static class PrimesUtility
    {
        /// <summary>
        /// First 10 known primes
        /// </summary>
        private static List<Int64> knownPrimes = new List<Int64>()
        {
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29
        };

        public static IEnumerable<long> Primes() => new PrimesEnumerable();
        public static IEnumerable<long> GetPrimesUpTo(long maxValue)
        {
            if (knownPrimes.Last() < maxValue)
            {
                CalculatePrimesUpToValue(maxValue);
            }

            return knownPrimes.TakeWhile(prime => prime < maxValue);
        }
        public static IEnumerable<long> GetFirstXPrimes(int numberOfPrimes)
        {
            long logN = (long)Math.Log(numberOfPrimes);
            long upperBound = 2 * numberOfPrimes * logN;
            while (knownPrimes.Count < numberOfPrimes)
            {
                CalculatePrimesUpToValue(upperBound);
                upperBound *= logN;
            }

            return knownPrimes.Take(numberOfPrimes);
        }

        private static void CalculatePrimesUpToValue(long maxValue)
        {
            long start = knownPrimes.Last();
            var potentialPrimes = new bool[maxValue - start];

            foreach (var prime in knownPrimes)
            {
                for (long elimIndex = prime - start % prime; elimIndex < maxValue - start; elimIndex += prime)
                {
                    potentialPrimes[elimIndex] = true;
                }
            }

            for (int index = 2; index < maxValue - start; index += 2)
            {
                if (potentialPrimes[index] == false)
                {
                    var currentPrime = index + start;
                    knownPrimes.Add(currentPrime);
                    for (long elimIndex = index; elimIndex < maxValue - start; elimIndex += currentPrime)
                    {
                        potentialPrimes[elimIndex] = true;
                    }
                }
            }
        }

        internal class PrimesEnumerable : IEnumerable<Int64>
        {
            public IEnumerator<Int64> GetEnumerator()
            {
                int index = 0;
                while (index < PrimesUtility.knownPrimes.Count)
                {
                    yield return knownPrimes.ElementAt(index);
                    index++;
                    if (index == knownPrimes.Count)
                    {
                        CalculatePrimesUpToValue(2 * knownPrimes.Last());
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }





}
