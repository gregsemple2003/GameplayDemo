using NUnit.Framework;

namespace GameplayGraph.Tests.Runtime
{
    public class NodeTests
    {
        [Test]
        public void AddNode_AddsOne()
        {
            var graph = new GameplayGraph.Graph();
            graph.AddNode(new GameplayGraph.Node());
            Assert.AreEqual(1, graph.NodeCount);
        }
    }
}
