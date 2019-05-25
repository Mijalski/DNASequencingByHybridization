using System;
using System.IO;

namespace DNA_sequencing
{
    class Program
    { 
        static readonly char[] ValidNucleotides =  new char[] {'A', 'C', 'G', 'T'};

        static void Main(string[] args)
        {
            int k = int.Parse(args[1]);
            string originalSequence = ReadSequenceInFile(args[0]);
            string generatedSequence = SequenceDNA(originalSequence, k);
            PrintResult(originalSequence, generatedSequence);
        }

        private static string ReadSequenceInFile(string filename) {
            ValidateFile(filename);

            string sequence = ReadContentsOfFile(filename);

            ValidateSequence(sequence);

            return sequence;
        }

        private static string SequenceDNA(string baseSequence, int k) {
            return UtilHelper.GetRandomString(baseSequence.Length);
        }

        private static void PrintResult(string originalSequence, string generatedSequence) {
            double similarity = UtilHelper.GetSimilarityOf(originalSequence, generatedSequence);

            Console.WriteLine($"Original sequence:\t{originalSequence}");
            Console.WriteLine($"Generated sequence:\t{generatedSequence}");
            Console.WriteLine($"Similarity: {similarity * 100}%");
        }

        private static void ValidateFile(string filename) {
            if (!File.Exists(filename)) {
                Console.WriteLine("File doesn't exist or you don't have permission to read it.");
                Environment.Exit(1);
            }
        }

        private static string ReadContentsOfFile(string filename) {
            return File.ReadAllText(filename).Trim();
        }

        private static void ValidateSequence(string sequence) {
            if (sequence.Length == 0) {
                Console.WriteLine("The file is empty.");
                Environment.Exit(2);
            }

            char[] sequenceCharacters = sequence.ToCharArray();
            foreach (var character in sequenceCharacters)
            {
                bool isValidCharacter = Array.IndexOf(ValidNucleotides, character) != -1;

                if (!isValidCharacter) { 
                    Console.WriteLine($"Sequence contains invalid character: {character}.");
                    Environment.Exit(2);
                }
            }
        }
    }
}
