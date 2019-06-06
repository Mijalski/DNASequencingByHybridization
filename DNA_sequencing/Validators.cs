using System;
using System.IO;

namespace DNA_sequencing {
  class Validators {
    
        static readonly char[] ValidNucleotides =  new char[] {'A', 'C', 'G', 'T'};

        public static void ValidateFile(string filename) {
            if (!File.Exists(filename)) {
                Console.WriteLine("File doesn't exist or you don't have permission to read it.");
                Environment.Exit(1);
            }
        }

        public static void ValidateSequence(string sequence) {
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