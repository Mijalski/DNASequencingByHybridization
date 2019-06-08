using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace DNA_sequencing
{
    public class UtilHelper
    {
        const string pool = "ACGT";

        private static int seed;

        private static ThreadLocal<Random> threadLocal = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));
        
        static UtilHelper()
        {
            seed = Environment.TickCount;
        }
        
        public static Random Instance => threadLocal.Value;

        //Methods to use:
        public static int GetRandomValue(int maxValue) => Instance.Next(0, maxValue);

        public static double GetRandomDoubleValue() => Instance.NextDouble();

        public static double[] AddToProbabilityArray(double[] array, int index, double value)
        {
            for (var i = index; i < array.Length; i++)
            {
                array[i] += value;
                if (array[i] < 0)
                    array[i] = 0;
            }

            return array;
        }
        
        public static double[] CreateProbabilityArray(double[] array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = i+1;
            }

            return array;
        }

        /// <summary>
        /// Returns a random DNA sequence; probablity for T is equal to 1 - (pA + pC + pG);
        /// </summary>
        /// <param name="length"></param>
        /// <param name="probabilityA"></param>
        /// <param name="probabilityC"></param>
        /// <param name="probabilityG"></param>
        /// <returns></returns>
        public static string GetRandomString(int length, float probabilityA = 0.25f, float probabilityC = 0.25f,
            float probabilityG = 0.25f)
        {
            var customPool = string.Empty;

            probabilityA *= 100;
            probabilityC *= 100;
            probabilityG *= 100;

            for (var i = 0; i < 100; i++)
            {
                if(i < probabilityA)
                    customPool += pool[0];

                else if(i < probabilityC + probabilityA)
                    customPool += pool[1];

                else if(i < probabilityG + probabilityC + probabilityA)
                    customPool += pool[2];

                else 
                    customPool += pool[3];
            }

            var chars = Enumerable.Range(0, length)
                .Select(x => customPool[Instance.Next(0, 100)]);

            return new string(chars.ToArray());
        }

        public static double GetSimilarityOf(string baseString, string referenceString)
        {
            InitializeMatrix(baseString.Length, referenceString.Length, out var scoringMatrix);
            FillScoresInMatrix(scoringMatrix, baseString, referenceString);
            var overallScore = CalculateScore(scoringMatrix, baseString, referenceString);
            return overallScore;
        }

        //Private methods:
        private static void InitializeMatrix(int baseLength, int referenceLength, out int[,]scoringMatrix)
        {
            scoringMatrix = new int[++baseLength, ++referenceLength];

            //Initialization Step - filled with 0 for the first row and the first column of matrix
            for (int i = 0; i < baseLength; i++)
            {
                scoringMatrix[i, 0] = 0;
            }

            for (int j = 0; j < referenceLength; j++)
            {
                scoringMatrix[0, j] = 0;
            }
        }

        private static void FillScoresInMatrix(int[,]scoringMatrix, string baseString, string referenceString)
        {
            for (int i = 1; i < scoringMatrix.GetLength(0); i++) //each element in base string
            {
                for (int j = 1; j < scoringMatrix.GetLength(1); j++) //each element in reference string
                {
                    var scoreDiag = referenceString.Substring(j - 1, 1) == baseString.Substring(i - 1, 1)
                        ? scoringMatrix[i - 1, j - 1] + 2
                        : scoringMatrix[i - 1, j - 1] + -1;

                    var scoreLeft = scoringMatrix[i, j - 1] - 2;
                    var scoreUp = scoringMatrix[i - 1, j] - 2;

                    var maxScore = Math.Max(Math.Max(scoreDiag, scoreLeft), scoreUp);

                    scoringMatrix[i, j] = maxScore;
                }
            } 
        }

        private static double CalculateScore(int[,]scoringMatrix, string baseString, string referenceString)
        {
            var baseElements = baseString.ToCharArray();
            var referenceElements = referenceString.ToCharArray();

            var alignmentA = string.Empty;
            var alignmentB = string.Empty;
            var m = baseString.Length;
            var n = referenceString.Length;

            while (m > 0 && n > 0)
            {
                int scoreDiag;

                //is +2 for a match, -1 for a mismatch and -2 for a gap
                if (baseElements[m - 1] == referenceElements[n - 1])
                    scoreDiag = 2;
                else
                    scoreDiag = -1;
                
                if (m > 0 && n > 0 && scoringMatrix[m, n] == scoringMatrix[m - 1, n - 1] + scoreDiag)
                {
                    alignmentA = referenceElements[n - 1] + alignmentA;
                    alignmentB = baseElements[m - 1] + alignmentB;
                    m = m - 1;
                    n = n - 1;
                }
                else if (n > 0 && scoringMatrix[m, n] == scoringMatrix[m, n - 1] - 2)
                {
                    alignmentA = referenceElements[n - 1] + alignmentA;
                    alignmentB = "-" + alignmentB;
                    n = n - 1;
                }
                else //if (m > 0 && scoringMatrix[m, n] == scoringMatrix[m - 1, n] + -2)
                {
                    alignmentA = "-" + alignmentA;
                    alignmentB = baseElements[m - 1] + alignmentB;
                    m = m - 1;
                }
            }

            var pointCoint = alignmentA.Where((t, i) => t == alignmentB[i]).Count(); //foreach loop where you count char array elements that are equal

            return (double)pointCoint / baseString.Length; //double check if you should divide by baseString.Length or the alignmentB.Length
        }

        public static string ReadContentsOfFile(string filename) {
            return File.ReadAllText(filename).Trim();
        }

    }
}
