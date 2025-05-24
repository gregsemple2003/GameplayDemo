using GameplayGraph;

namespace GameplayGraph.Tests
{
    /// <summary>
    /// Simple container of floating point stats.
    /// </summary>
    public partial class StatsContainer : PropertyContainer<float>
    {
        [ContainerProperty("strength")]
        public float Strength { get; set; }

        [ContainerProperty("intelligence")]
        public float Intelligence { get; set; }

        [ContainerProperty("dexterity")]
        public float Dexterity { get; set; }
    }
}
