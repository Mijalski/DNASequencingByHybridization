using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA_sequencing.GraphLogic
{
    public class Graph
    {
        public List<Node> Nodes;
        public Node StartingNode;

        public string OriginalDna;
        public int DnaLength;
        public int LengthK; //TODO Change this name
        public int IdealNodeCount;

        public Graph(string dna, int dnaLength, int k)
        {
            LengthK = k;
            Nodes = new List<Node>();
            OriginalDna = dna;
            DnaLength = dna.Length;

            for (var i = 0; i <= dnaLength - LengthK; i++)
            {
                this.AddNode(dna.Substring(i, LengthK)); //add a node for every K-length substring of our DNA
            }
            
            IdealNodeCount = dnaLength - LengthK + 1; // S = n - k + 1

            StartingNode = Nodes.First();

            AddEdges();
        }
        
        public string RecreateDna(int maxRounds = 100)
        {
            var round = 0;
            var probabilityMultipliers = UtilHelper.CreateProbabilityArray(new double[LengthK - 2]);
            
            var currentBestDna = FindRandomDnaSequence();
            var currentBestSimilarity = UtilHelper.GetSimilarityOf(OriginalDna, currentBestDna);

            Console.WriteLine($"ideal node count: {IdealNodeCount}, node count {Nodes.Count}");
            Console.WriteLine($"Similarity after {round} rounds: {currentBestSimilarity}");

            while (round < maxRounds)
            {
                probabilityMultipliers = UtilHelper.AddToProbabilityArray(probabilityMultipliers, LengthK - 3, 5);
                currentBestDna = FindDnaSequence(probabilityMultipliers);
                currentBestSimilarity = UtilHelper.GetSimilarityOf(OriginalDna, currentBestDna);
                Console.WriteLine($"Similarity after {round} rounds: {currentBestSimilarity}");
                round++;
            }
            return currentBestDna;
        }
        
        public string FindRandomDnaSequence()
        {
            var currentNode = StartingNode;
            var reconstructedDna = StartingNode.DnaSequence;

            while (reconstructedDna.Length < DnaLength)
            {
                var nextEdge = currentNode.GetRandomEdgeOut();
                currentNode = nextEdge.ToNode;

                reconstructedDna = reconstructedDna.Remove(reconstructedDna.Length - nextEdge.Weight); //we need to remove overlapping DNA sequence from the next node
                reconstructedDna += currentNode.DnaSequence;
            }

            return reconstructedDna;
        }


        public string FindDnaSequence(double[] probabilityMultipliers)
        {
            var visitedNodesList = new List<Node>();
            var currentNode = StartingNode;
            var reconstructedDna = StartingNode.DnaSequence;

            while (reconstructedDna.Length < DnaLength)
            {
                var nextEdge = currentNode.GetRandomEdgeOut(probabilityMultipliers, visitedNodesList.Count < Nodes.Count, visitedNodesList);
                currentNode = nextEdge.ToNode;
                visitedNodesList.Add(currentNode);

                reconstructedDna = reconstructedDna.Remove(reconstructedDna.Length - nextEdge.Weight); //we need to remove overlapping DNA sequence from the next node
                reconstructedDna += currentNode.DnaSequence;
            }
            return reconstructedDna;
        }

        public void PrintOutGraph()
        {
            Console.WriteLine($"DNA: {OriginalDna}");

            foreach(var node in Nodes)
            {
                Console.Write($"[{node.Number}][{node.DnaSequence}] - ");

                foreach(var edgeOut in node.EdgesOut)
                {
                    Console.Write($"({edgeOut.ToNode.Number}, {edgeOut.Weight}) ");
                }
                Console.WriteLine("");
            }
        }

        private void AddNode(string dnaSequence)
        {
            var node = Nodes.SingleOrDefault(_ => _.DnaSequence == dnaSequence);

            if (node == null) //Only create new node when a node with that dna sequence is not existing
            {
                Nodes.Add(new Node(dnaSequence, Nodes.Count));
            }
        }

        private void AddEdges()
        {
            foreach(var fromNode in Nodes)
            {
                var nodeDnaSequence = fromNode.DnaSequence;

                for(var i = 1; i <= LengthK; i++)
                {
                    var endingDnaSequenceOfNode = nodeDnaSequence
                        .Substring(nodeDnaSequence.Length - i, i); //last i dna nucleotides of our node 
                                                          //to see if any other node is starting with the same nucleotides combination

                    var nodesToMatching = Nodes
                        .Where(_ => _.DnaSequence.Substring(0, i) == endingDnaSequenceOfNode && _.DnaSequence != fromNode.DnaSequence)
                        .ToList(); 
                                    //get every other node that starts  
                                    //with our other node nucleotides combination
                    
                    foreach(var nodeToMatching in nodesToMatching)
                    {
                        fromNode.AddEdgeToNode(nodeToMatching, i);
                    }
                }
            }
        }
    }
}
