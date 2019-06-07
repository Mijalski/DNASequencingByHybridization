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
        
        public Edge GetRandomEdgeOut() => EdgesOut.ElementAt(UtilHelper.GetRandomValue(EdgesOut.Count));

        public Edge GetRandomEdgeOut(double[] probabilityMultipliers, bool canVisitTheSameNode, List<Node> visitedNodes)
        {
            var randomValue = UtilHelper.GetRandomDoubleValue() * probabilityMultipliers.Last();

            var edgeMinWeight = probabilityMultipliers
                .Select((v, i) => new {value = v, index = i})
                .OrderBy(_ => _.value)
                .First(_ => _.value >= randomValue)
                .index + 1;

            var baseEdgeMinWeight = edgeMinWeight;

            while (true)
            {
                var edgesWithRequiredWeight = EdgesOut
                    .Where(_ => _.Weight == edgeMinWeight &&
                                (canVisitTheSameNode || !visitedNodes.Contains(_.ToNode)))
                    .ToList();

                if (edgesWithRequiredWeight.Any())
                {
                    var randomEdgeWithRequiredWeight = edgesWithRequiredWeight.ElementAt(UtilHelper.GetRandomValue(edgesWithRequiredWeight.Count));
                    return randomEdgeWithRequiredWeight;
                }

                if (edgeMinWeight > baseEdgeMinWeight && edgeMinWeight != DnaSequence.Length - 1)
                {
                    edgeMinWeight++;
                }
                else if (edgeMinWeight == DnaSequence.Length - 1)
                {
                    edgeMinWeight = baseEdgeMinWeight - 1;
                }
                else if(edgeMinWeight != 1)
                {
                    edgeMinWeight--;
                }
                else
                {
                    return EdgesOut.Last();
                }
            }
        }

        public void AddEdgeToNode(Node toNode, int strength)
        {
            var newEdge = new Edge(this, toNode, strength);

            this.EdgesOut.Add(newEdge);
            toNode.EdgesIn.Add(newEdge);
        }
    }
}
