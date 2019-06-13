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
            
            IdealNodeCount = this.Nodes.Count; 

            StartingNode = Nodes.First();

            AddEdges();
        }
        
        public string RecreateDna()
        {
            //variables changing algorithm behaviour
            var maxRounds = 550;
            var nodeCountMaxDifferenceThreshold = 6;
            var shouldIncreaseProbabilityMatrix = 0;
            //--

            var round = 0;
            var probabilityMultipliers = UtilHelper.CreateProbabilityArray(new double[LengthK - 1]);
            
            var currentDna = FindRandomDnaSequence();
            var currentSimilarity = UtilHelper.GetSimilarityOf(OriginalDna, currentDna);
            Console.WriteLine($"Ideal node count: {IdealNodeCount}");
            Console.WriteLine($"Similarity after random algorithm: {currentSimilarity}");

            while (round < maxRounds)
            {
                TweakProbabilityArray(probabilityMultipliers, shouldIncreaseProbabilityMatrix);
                
                currentDna = FindDnaSequence(probabilityMultipliers, nodeCountMaxDifferenceThreshold, out var visitedNodesCount);
                currentSimilarity = UtilHelper.GetSimilarityOf(OriginalDna, currentDna);

                if (round % 10 == 0)
                {
                    Console.WriteLine($"Similarity after {round} rounds: {currentSimilarity} with node usage {visitedNodesCount}");
                }

                if (visitedNodesCount == IdealNodeCount && currentDna.Length == DnaLength)
                {
                    Console.WriteLine($"This DNA Sequence has ideal node count & ideal dna length");
                    return currentDna;
                }

                if (visitedNodesCount + 10 < IdealNodeCount)
                {
                    shouldIncreaseProbabilityMatrix = 1;
                }
                else if(visitedNodesCount - 10 < IdealNodeCount)
                {
                    shouldIncreaseProbabilityMatrix = 0;
                }
                else
                {
                    shouldIncreaseProbabilityMatrix = -1;
                }

                round++;
            }
            return currentDna;
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


        public string FindDnaSequence(double[] probabilityMultipliers, int nodeCountMaxDifferenceThreshold, out int visitedNodesCount)
        {
            var visitedNodesList = new List<Node>();
            var currentNode = StartingNode;
            var reconstructedDna = StartingNode.DnaSequence;

            while (reconstructedDna.Length < DnaLength)
            {
                var nextEdge = currentNode.GetRandomEdgeOut(probabilityMultipliers, visitedNodesList.Count < (Nodes.Count + nodeCountMaxDifferenceThreshold), visitedNodesList);
                currentNode = nextEdge.ToNode;
                visitedNodesList.Add(currentNode);

                reconstructedDna = reconstructedDna.Remove(reconstructedDna.Length - nextEdge.Weight); //we need to remove overlapping DNA sequence from the next node
                reconstructedDna += currentNode.DnaSequence;
            }

            visitedNodesCount = visitedNodesList.Count;
            return reconstructedDna;
        }

        public double[] TweakProbabilityArray(double[] probabilityMultipliers, int shouldIncreaseProbabilityMatrix)
        {
            if(shouldIncreaseProbabilityMatrix > 0 )
                UtilHelper.AddToProbabilityArray(probabilityMultipliers, probabilityMultipliers.Length - 1, 15);
            else if(shouldIncreaseProbabilityMatrix < 0)
                UtilHelper.AddToProbabilityArray(probabilityMultipliers, 0, 15);
            else
                UtilHelper.AddToProbabilityArray(probabilityMultipliers, probabilityMultipliers.Length/2, 15);

            return probabilityMultipliers;

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
