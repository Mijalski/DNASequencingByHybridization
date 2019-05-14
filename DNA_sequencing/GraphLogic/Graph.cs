﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA_sequencing.GraphLogic
{
    public class Graph
    {
        public List<Node> Nodes;
        public string OriginalDna;
        public int LengthK = 0; //TODO Change this name

        public Graph(string dna, int dnaLength, int k)
        {
            LengthK = k;
            Nodes = new List<Node>();
            OriginalDna = dna;

            for (var i = 0; i <= dnaLength - LengthK; i++)
            {
                this.AddNode(dna.Substring(i, LengthK)); //add a node for every K-length substring of our DNA
            }

            AddEdges();
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
            var node = Nodes.FirstOrDefault(_ => _.DnaSequence == dnaSequence);

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