using System;
using System.Collections.Generic;
using System.Text;
using DNA_sequencing.GraphLogic;

namespace DNA_sequencing.GraphLogic
{
    public class Edge
    {
        public Node FromNode;
        public Node ToNode;
        public int Weight;

        public Edge(Node fromEdge, Node toNode, int weight)
        {
            FromNode = fromEdge;
            ToNode = toNode;
            Weight = weight;
        }
    }
}
