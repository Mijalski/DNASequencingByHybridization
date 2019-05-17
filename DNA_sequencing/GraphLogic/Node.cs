using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA_sequencing.GraphLogic
{
    public class Node
    {
        public List<Edge> EdgesIn;
        public List<Edge> EdgesOut;
        
        public string DnaSequence;
        public int Number;

        public Node(string dnaSequence, int number)
        {
            this.DnaSequence = dnaSequence;
            this.Number = number;
            EdgesIn = new List<Edge>();
            EdgesOut = new List<Edge>();
        }

        public Edge GetRandomEdgeOut()
        {
            return EdgesOut.ElementAt(UtilHelper.GetRandomValue(EdgesOut.Count));
        }

        public void AddEdgeToNode(Node toNode, int strength)
        {
            var newEdge = new Edge(this, toNode, strength);

            this.EdgesOut.Add(newEdge);
            toNode.EdgesIn.Add(newEdge);
        }
    }
}
