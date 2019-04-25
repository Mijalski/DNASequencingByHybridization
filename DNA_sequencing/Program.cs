using System;

namespace DNA_sequencing
{
    class Program
    { 
        public const string FileName = @"CurrentDNA.txt";

        static void Main(string[] args)
        {
            var dnaSequence = string.Empty;
            Console.WriteLine("Generate new DNA sequence (g)\nLoad in DNA seqence (l)");
            var option =  Console.ReadLine();

            if (option == "g")
            {
                Console.WriteLine("How long should the DNA sequence be: ");

                var correctNumberInput = Int32.TryParse(Console.ReadLine(), out var dnaLength);
                if (!correctNumberInput)
                {
                    dnaLength = 10;
                }

                dnaSequence = StringHelper.GetRandomString(dnaLength);
                System.IO.File.WriteAllText(FileName, dnaSequence);
            }
            else
            {
                try
                {
                    dnaSequence = System.IO.File.ReadAllText(FileName);
                }
                catch
                {
                    Console.WriteLine("No DNA file");
                }
            }
            
            //try to recreate this DNA
            //for now let's compare to sample string

            var sampleDna = "AAAAAAAAGGGGTACAGCCC";

            StringHelper.CompareString(dnaSequence, sampleDna);
            Console.ReadLine();
        }
    }
}
