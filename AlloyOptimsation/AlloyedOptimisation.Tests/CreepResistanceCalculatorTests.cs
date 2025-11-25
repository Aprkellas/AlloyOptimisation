using AlloyOptimisation.Domain.Alloy;
using AlloyOptimisation.Domain.Calculators;
using AlloyOptimisation.Domain.Elements;

namespace AlloyOptimisation.Tests
{
    public class CreepResistanceCalculatorTests
    {
        [Fact]
        public void ComputesExpectedCreepResistance()
        {
            var ni = new ElementDefinition("Ni", alpha: 0.0, costPerKg: 10.0);
            var cr = new ElementDefinition("Cr", alpha: 1.23, costPerKg: 5.0);
            var co = new ElementDefinition("Co", alpha: 2.34, costPerKg: 7.0);

            var system = new AlloySystem(
                baseElement: ni,
                variableElements: [cr, co]
            );

            var composition = new AlloyComposition(system, new Dictionary<ElementDefinition, double>
            {
                [cr] = 10.0,
                [co] = 5.0
            });

            var calculator = new NickelCreepResistanceCalculator();

            var creep = calculator.Compute(composition);

            var linear = (cr.Alpha * 10.0) + (co.Alpha * 5.0);
            var expected = Math.Exp(linear);

            Assert.InRange(creep, expected * 0.999, expected * 1.001);
        }
    }
}