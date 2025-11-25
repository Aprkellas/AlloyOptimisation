using AlloyOptimisation.Domain.Alloy;
using AlloyOptimisation.Domain.Elements;

namespace AlloyOptimisation.Tests
{
    public class GridAlloyEnumeratorTests
    {
        [Fact]
        public void EnumeratesAllCombinations_AndBalancesBaseElement()
        {
            var ni = new ElementDefinition("Ni", 0.0, 10.0);
            var cr = new ElementDefinition("Cr", 1.0, 5.0);

            var range = new ElementRange(cr, min: 0, max: 20, step: 10);
            var system = new AlloySystem(ni, [ cr ]);

            var enumerator = new GridAlloyEnumerator();

            enumerator.AddRange(range);
            var alloys = enumerator.Enumerate(system).ToList();

            Assert.Equal(3, alloys.Count);
            Assert.Contains(
                alloys, 
                a => a.AtomicPercents[cr] == 0 && a.BaseElementPercent == 100
            );
            Assert.Contains(
                alloys, 
                a => a.AtomicPercents[cr] == 20 && a.BaseElementPercent == 80
            );
        }
    }
}
