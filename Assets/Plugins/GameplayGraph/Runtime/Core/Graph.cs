using System.Collections.Generic;

namespace GameplayGraph
{
    public class Graph
    {
        private readonly List<Node> _nodes = new();

        public int NodeCount => _nodes.Count;

        public void AddNode(Node node)
        {
            _nodes.Add(node);
        }
    }
}
