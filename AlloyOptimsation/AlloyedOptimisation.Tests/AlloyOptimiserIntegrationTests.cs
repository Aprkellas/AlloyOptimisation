using AlloyOptimisation.Domain.Alloy;
using AlloyOptimisation.Domain.Calculators;
using AlloyOptimisation.Domain.Elements;
using AlloyOptimisation.Domain.Optimisers;

namespace AlloyOptimisation.Tests
{
    public class AlloyOptimiserIntegrationTests
    {
        [Fact]
        public void FindsMaximumCreepResistanceWithinCostCap_FromTable2Ranges()
        {
            var ni = new ElementDefinition("Ni", 0.0, 8.9);

            var cr = new ElementDefinition("Cr", alpha: 2.0911350E16, costPerKg: 14.0);
            var co = new ElementDefinition("Co", alpha: 7.2380280E16, costPerKg: 80.5);
            var nb = new ElementDefinition("Nb", alpha: 1.0352738E16, costPerKg: 42.5);
            var mo = new ElementDefinition("Mo", alpha: 8.9124547E16, costPerKg: 16.0);

            var system = new AlloySystem(ni, [cr, co, nb, mo]);

            // Table 2 ranges
            var enumerator = new GridAlloyEnumerator();
            enumerator.AddRange(new ElementRange(cr, min: 14.5, max: 22.0, step: 0.5));
            enumerator.AddRange(new ElementRange(co, min: 0.0, max: 25.0, step: 1.0));
            enumerator.AddRange(new ElementRange(nb, min: 0.0, max: 1.5, step: 0.1));
            enumerator.AddRange(new ElementRange(mo, min: 1.5, max: 6.0, step: 0.5));

            var creepCalc = new NickelCreepResistanceCalculator();
            var costCalc = new SimpleCostCalculator();

            var optimiser = new AlloyOptimiser(enumerator, creepCalc, costCalc);

            var best = optimiser.FindBest(system, maxCostPerKg: 18.0);

            Assert.NotNull(best);

            var bestCreep = creepCalc.Compute(best!);
            var bestCost = costCalc.Compute(best!);

            Assert.True(bestCost <= 18.0 + 1e-6);
            Assert.InRange(bestCreep, 1.72999e18 * 0.999, 1.72999e18 * 1.001);
        }
    }
}
