using AlloyOptimisation.Domain.Elements;

namespace AlloyOptimisation.Domain.Alloy
{
    public class AlloyComposition
    {
        public AlloySystem System { get; }
        public IReadOnlyDictionary<ElementDefinition, double> AtomicPercents { get; }
        public double BaseElementPercent => 100.0 - AtomicPercents.Values.Sum();

        public AlloyComposition(AlloySystem system,
                                IDictionary<ElementDefinition, double> atomicPercents)
        {
            System = system;
            AtomicPercents = new Dictionary<ElementDefinition, double>(atomicPercents);
        }
    }
}
