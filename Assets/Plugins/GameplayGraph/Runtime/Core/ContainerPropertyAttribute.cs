using System;

namespace GameplayGraph
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ContainerPropertyAttribute : Attribute
    {
        public SlotDescriptor Slot { get; }

        public ContainerPropertyAttribute(string name)
        {
            Slot = new SlotDescriptor(name);
        }
    }
}
