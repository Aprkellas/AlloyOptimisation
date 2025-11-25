
using AlloyOptimisation.Domain.Elements;

namespace AlloyOptimisation.Domain.Alloy
{
    public class GridAlloyEnumerator : IAlloyEnumerator
    {

        private readonly Dictionary<ElementDefinition, ElementRange> _ranges
            = new();

        public void AddRange(ElementRange range)
        {
            _ranges[range.Element] = range;
        }

        public IEnumerable<AlloyComposition> Enumerate(AlloySystem system)
        {
            if (system is null)
                throw new ArgumentNullException(nameof(system));


        }
    }
}
