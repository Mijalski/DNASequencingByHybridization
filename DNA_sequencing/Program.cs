using System;
using System.IO;

namespace DNA_sequencing
{
    class Program
    { 
        static void Main(string[] args)
        {
            var dnaSequence = string.Empty;
            Console.WriteLine("Generate random DNA sequences (g)\nLoad in DNA seqence (l)");
            var option =  Console.ReadLine();

            if (option == "g")
            {
                var correctNumberInput = false;
                var dnaLength = 0;

                while (!correctNumberInput)
                {
                    Console.WriteLine("How long should DNA sequences be: ");
                    correctNumberInput = int.TryParse(Console.ReadLine(), out dnaLength);
                }
                
                GenerateRandomDnaInstances(dnaLength);
            }
            else
            {
                var correctNumberInput = false;
                var dnaLength = 0;
                
                while (!correctNumberInput)
                {
                    Console.WriteLine("DNA length: ");
                    correctNumberInput = int.TryParse(Console.ReadLine(), out dnaLength);
                }

                //for now just load in dna-random, should be replaced in the future with a for loop or a file choice :)
                var fileName = $"sequences-{dnaLength}\\dna-random-1.txt";
                try
                {
                    dnaSequence = File.ReadAllText(fileName);
                }
                catch
                {
                    Console.WriteLine($"No DNA file at --> {fileName}");
                }

                correctNumberInput = false;

                var k = 0; //TODO i hate this, have to think of a better name for

                while (!correctNumberInput)
                {
                    Console.WriteLine("K size: ");
                    correctNumberInput = int.TryParse(Console.ReadLine(), out k);
                }

                var graph = new GraphLogic.Graph(dnaSequence, dnaLength, k);
                graph.PrintOutGraph();
                
            }
            
            //try to recreate this DNA
            //for now let's compare to sample string

            var sampleDna = "AAAAAAAAGGGGTACAGCCC";

            StringHelper.CompareString(dnaSequence, sampleDna);
            Console.ReadLine();
        }

        public static void GenerateRandomDnaInstances(int length)
        {
            var directory = $"sequences-{length}";

            if(!Directory.Exists(directory))  // if it doesn't exist, create
                Directory.CreateDirectory(directory);

            //random dna

            var dnaSequence = StringHelper.GetRandomString(length);
            File.WriteAllText($"{directory}\\dna-random-1.txt", dnaSequence);

            dnaSequence = StringHelper.GetRandomString(length);
            File.WriteAllText($"{directory}\\dna-random-2.txt", dnaSequence);

            //very repetitive 

            dnaSequence = StringHelper.GetRandomString(length, 0.85f, 0.05f, 0.05f);
            File.WriteAllText($"{directory}\\dna-repetitve-1.txt", dnaSequence);
            
            //only two nucleotides

            dnaSequence = StringHelper.GetRandomString(length, 0.5f, 0.5f, 0f);
            File.WriteAllText($"{directory}\\dna-only-AC-1.txt", dnaSequence);

            //mostly two nucleotides

            dnaSequence = StringHelper.GetRandomString(length, 0.4f, 0.4f, 0.1f);
            File.WriteAllText($"{directory}\\dna-mostly-AC-1.txt", dnaSequence);

            //mostly three nucleotides

            dnaSequence = StringHelper.GetRandomString(length, 0.3f, 0.3f, 0.3f);
            File.WriteAllText($"{directory}\\dna-mostly-ACG-1.txt", dnaSequence);
        }
    }
}
