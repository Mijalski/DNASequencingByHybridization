using System;
using System.IO;

namespace DNA_sequencing
{
    class Program
    { 
        static void Main(string[] args)
        {
            int k = int.Parse(args[1]);
            string originalSequence = ReadSequenceInFile(args[0]);
            string generatedSequence = SequenceDNA(originalSequence, k);
            PrintResult(originalSequence, generatedSequence);
        }

        private static string ReadSequenceInFile(string filename) {
            return "ACGT";
        }

        private static string SequenceDNA(string baseSequence, int k) {
            return "AGCT";
        }

        private static void PrintResult(string originalSequence, string generatedSequence) {
            double similarity = UtilHelper.GetSimilarityOf(originalSequence, generatedSequence);

            Console.WriteLine($"Original sequence:\t{originalSequence}");
            Console.WriteLine($"Generated sequence:\t{generatedSequence}");
            Console.WriteLine($"Similarity: {similarity * 100}%");
        }
    }
}
