
using AlloyOptimisation.Domain.Elements;

namespace AlloyOptimisation.Domain.Alloy
{
    public class GridAlloyEnumerator : IAlloyEnumerator
    {

        private readonly List<ElementRange> _ranges = new();

        public void AddRange(ElementRange range)
        {
            _ranges.Add(range);
        }

        public IEnumerable<AlloyComposition> Enumerate(AlloySystem system)
        {
            if (system is null)
                throw new ArgumentNullException(nameof(system));

            if (_ranges.Count == 0)
                yield break;

            var current = new Dictionary<ElementDefinition, double>();
            foreach (var comp in EnumerateRecursive(0, current, system))
                yield return comp;
        }

        private IEnumerable<AlloyComposition> EnumerateRecursive(
            int index,
            Dictionary<ElementDefinition, double> current,
            AlloySystem system)
        {
            if (index == _ranges.Count)
            {
                yield return new AlloyComposition(system, new Dictionary<ElementDefinition, double>(current));
                yield break;
            }

            var range = _ranges[index];

            for (double v = range.Min; v <= range.Max + 1e-9; v += range.Step)
            {
                // round to avoid floating drift
                double value = Math.Round(v, 3);
                current[range.Element] = value;

                foreach (var comp in EnumerateRecursive(index + 1, current, system))
                    yield return comp;
            }

            current.Remove(range.Element);
        }

    }
}
