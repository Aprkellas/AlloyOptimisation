using AlloyOptimisation.Domain;

namespace AlloyOptimsationTests
{
    public class CreepResistanceCalculatorTests
    {
        [Fact]
        public void ComputesExpectedCreepResistance()
        {
            var ni = new ElementDefinition("Ni", alpha: 0.0, costPerKg: 10.0);
            var cr = new ElementDefinition("Cr", alpha: 1.23, costPerKg: 5.0);
            var co = new ElementDefinition("Co", alpha: 2.34, costPerKg: 7.0);
        }
    }
}