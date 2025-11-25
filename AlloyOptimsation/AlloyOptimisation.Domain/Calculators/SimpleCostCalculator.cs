using AlloyOptimisation.Domain.Alloy;

namespace AlloyOptimisation.Domain.Calculators
{
    public class SimpleCostCalculator : ICostCaclulator
    {
        public double Compute(AlloyComposition composition)
        {
            if (composition == null)
            {
                throw new ArgumentNullException(nameof(composition));
            }

            var cost = 0.0;

            // sum costs of each element times its atomic fraction
            foreach (var (element, atomicPercent) in composition.AtomicPercents)
            {
                double fraction = atomicPercent / 100.0;
                cost += element.CostPerKg * fraction;
            }

            var baseFraction = composition.BaseElementPercent / 100.0;
            cost += composition.System.BaseElement.CostPerKg * baseFraction;

            return cost;
        }
    }
}
