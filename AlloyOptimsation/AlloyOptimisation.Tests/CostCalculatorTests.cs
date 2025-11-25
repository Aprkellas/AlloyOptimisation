using AlloyOptimisation.Domain.Alloy;
using AlloyOptimisation.Domain.Calculators;
using AlloyOptimisation.Domain.Elements;

namespace AlloyOptimisation.Tests
{
    public class CostCalculatorTests
    {
        [Fact]
        public void ComputesCostAsSumOfElementCostTimesFraction()
        {
            var ni = new ElementDefinition("Ni", alpha: 0.0, costPerKg: 10.0);
            var cr = new ElementDefinition("Cr", alpha: 1.23, costPerKg: 5.0);

            var system = new AlloySystem(ni, [cr]);

            var composition = new AlloyComposition(system, new Dictionary<ElementDefinition, double>
            {
                [cr] = 20
            });

            var calculator = new SimpleCostCalculator();

            var cost = calculator.Compute(composition);

            var expected = (ni.CostPerKg * 0.8) + (cr.CostPerKg * 0.2);
            Assert.Equal(expected, cost, precision: 6);
        }
    }
}
