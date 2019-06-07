﻿using System;
using System.IO;
using DNA_sequencing.GraphLogic;

namespace DNA_sequencing
{
    class Program
    {
        //k
        public static int SpectrumSize = 5;

        // Shit happens here
        private static string SequenceDNA(string baseSequence)
        {
            var graph = new Graph(baseSequence, baseSequence.Length, SpectrumSize);
            return graph.RecreateDna();
        }

        // Usage: program.dll <file.txt|--random>
        static void Main(string[] args)
        {
            if(args.Length > 0){

                switch (args[0]) {
                    case "--random":
                        SequenceRandomDNA();
                        break;
                    default:
                        SequenceDNAInFile(args[0]);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Args[0]: ");
                var option0 =  Console.ReadLine();
                switch (option0) {
                    case "--random":
                        SequenceRandomDNA();
                        break;
                    default:
                        SequenceDNAInFile(option0);
                        break;
                }
            }
            Console.ReadLine();
        }

        private static void SequenceRandomDNA()
        {
            int length = 200;
            string randomSequence = UtilHelper.GetRandomString(length);
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
