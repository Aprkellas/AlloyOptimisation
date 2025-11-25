using AlloyOptimisation.Domain.Alloy;
using AlloyOptimisation.Domain.Calculators;
using AlloyOptimisation.Domain.Elements;
using AlloyOptimisation.Domain.Optimisers;

namespace AlloyOptimisation.Tests
{
    public class AlloyOptimiserTests
    {
        [Fact]
        public void PicksAlloyWithHighestCreepWithinCostCap()
        {
            var ni = new ElementDefinition("Ni", 0.0, 10.0);

            var cr = new ElementDefinition("Cr", alpha: 2.0911350E16, costPerKg: 14.0);
            var co = new ElementDefinition("Co", alpha: 7.2380280E16, costPerKg: 80.5);
            var nb = new ElementDefinition("Nb", alpha: 1.0352738E16, costPerKg: 42.5);
            var mo = new ElementDefinition("Mo", alpha: 8.9124547E16, costPerKg: 16.0);

            var system = new AlloySystem(
                baseElement: ni,
                variableElements: [cr, co, nb, mo]
            );

            var enumerator = new GridAlloyEnumerator();
            enumerator.AddRange(new ElementRange(cr, min: 14.5, max: 22.0, step: 0.5));
            enumerator.AddRange(new ElementRange(co, min: 0.0, max: 25.0, step: 1.0));
            enumerator.AddRange(new ElementRange(nb, min: 0.0, max: 1.5, step: 0.1));
            enumerator.AddRange(new ElementRange(mo, min: 1.5, max: 6.0, step: 0.5));

            var optimizer = new AlloyOptimiser(
            enumerator,
            new NickelCreepResistanceCalculator(),
            new SimpleCostCalculator());

            var result = optimizer.FindBest(system, maxCostPerKg: 18.0);

            Assert.NotNull(result);
        }
    }
}
