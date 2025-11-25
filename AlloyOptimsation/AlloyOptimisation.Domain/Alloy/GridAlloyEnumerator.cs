
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

            foreach (var element in system.VariableElements)
            {
                if (!_ranges.TryGetValue(element, out var range))
                {
                    throw new InvalidOperationException(
                        $"No range defined for element {element.Symbol}"
                    );
                }

                for (var val = range.Min; val <= range.Max; val += range.Step)
                {
                    var dict = new Dictionary<ElementDefinition, double>
                    {
                        [element] = val
                    };

                    yield return new AlloyComposition(system, dict);
                }
            }
        }
    }
}
