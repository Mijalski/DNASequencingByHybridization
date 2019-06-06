using System;
using System.IO;

namespace DNA_sequencing
{
    class Program
    { 
        // Shit happens here
        private static string SequenceDNA(string baseSequence) {
            return UtilHelper.GetRandomString(baseSequence.Length);
        }

        // Usage: program.dll <file.txt|--random>
        static void Main(string[] args)
        {
            switch (args[0]) {
                case "--random":
                    SequenceRandomDNA();
                    break;
                default:
                    SequenceDNAInFile(args[0]);
                    break;
            }

        }

        private static void SequenceRandomDNA() {
            int randomLength = new Random().Next(700);
            string randomSequence = UtilHelper.GetRandomString(randomLength);
            string generatedSequence = SequenceDNA(randomSequence);
            PrintResult(randomSequence, generatedSequence);
        }

        private static void SequenceDNAInFile(string filename) {
            string originalSequence = ReadSequenceInFile(filename);
            string generatedSequence = SequenceDNA(originalSequence);
            PrintResult(originalSequence, generatedSequence);
        }

        private static void PrintResult(string originalSequence, string generatedSequence) {
            double similarity = UtilHelper.GetSimilarityOf(originalSequence, generatedSequence);

            Console.WriteLine($"Original sequence:\t{originalSequence}");
            Console.WriteLine($"Generated sequence:\t{generatedSequence}");
            Console.WriteLine($"Similarity: {similarity * 100}%");
        }

        private static string ReadSequenceInFile(string filename) {
            Validators.ValidateFile(filename);

            string sequence = UtilHelper.ReadContentsOfFile(filename);

            Validators.ValidateSequence(sequence);

            return sequence;
        }

    }
}
