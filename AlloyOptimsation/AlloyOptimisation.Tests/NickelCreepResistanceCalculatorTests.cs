using AlloyOptimisation.Domain.Alloy;
using AlloyOptimisation.Domain.Calculators;
using AlloyOptimisation.Domain.Elements;

namespace AlloyOptimisation.Tests
{
    public class NickelCreepResistanceCalculatorTests
    {
        private static readonly ElementDefinition Ni =
            new("Ni", alpha: 0.0, costPerKg: 8.9);

        private static readonly ElementDefinition Cr =
            new("Cr", alpha: 2.0911350E+16, costPerKg: 14.0);

        private static readonly ElementDefinition Co =
            new("Co", alpha: 7.2380280E+16, costPerKg: 80.5);

        private static readonly ElementDefinition Nb =
            new("Nb", alpha: 1.0352738E+16, costPerKg: 42.5);

        private static readonly ElementDefinition Mo =
            new("Mo", alpha: 8.9124547E+16, costPerKg: 16.0);

        private static readonly AlloySystem NickelSystem = new(
            baseElement: Ni,
            variableElements: [Cr, Co, Nb, Mo]
        );

        [Theory]
        // Cr   Co    Nb    Mo    expected creep resistance (from Table 3 in spec)
        [InlineData(15.0, 10.0, 1.00, 2.00, 1.226E18)]
        [InlineData(20.0, 0.0, 0.00, 1.50, 5.519E17)]
        [InlineData(22.0, 25.0, 1.50, 6.00, 2.820E18)]
        public void MatchesTable3Values(
            double crPct,
            double coPct,
            double nbPct,
            double moPct,
            double expected)
        {
            var composition = new AlloyComposition(NickelSystem, new Dictionary<ElementDefinition, double>
            {
                [Cr] = crPct,
                [Co] = coPct,
                [Nb] = nbPct,
                [Mo] = moPct
            });

            var calculator = new NickelCreepResistanceCalculator();

            var creep = calculator.Compute(composition);

            // allow small rounding differences (i believe the spec values are rounded)
            var tolerance = expected * 1e-4; // 0.01%

            Assert.InRange(creep, expected - tolerance, expected + tolerance);
        }
    }
}
