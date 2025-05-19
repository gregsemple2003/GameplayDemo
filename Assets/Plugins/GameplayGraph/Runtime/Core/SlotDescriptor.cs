namespace GameplayGraph
{
    public interface ISlotDescriptor
    {
        string Name { get; }
    }

    public readonly struct SlotDescriptor : ISlotDescriptor
    {
        public string Name { get; }

        public SlotDescriptor(string name)
        {
            Name = name;
        }
    }
}
