using AlloyOptimisation.Domain.Alloy;

namespace AlloyOptimisation.Domain.Calculators
{
    public class SimpleCostCalculator : ICostCalculator
    {
        public double Compute(AlloyComposition composition)
        {
            if (composition is null)
            {
                throw new ArgumentNullException(nameof(composition));
            }

            var cost = 0.0;
            var sumPercentNonBase = 0.0;

            // sum costs of each element times its atomic fraction
            foreach (var (element, atomicPercent) in composition.AtomicPercents)
            {
                sumPercentNonBase += atomicPercent;
                var fraction = atomicPercent / 100.0;
                cost += element.CostPerKg * fraction;
            }

            var basePercent = 100.0 - sumPercentNonBase;
            var baseFraction = basePercent / 100.0;

            cost += baseFraction * composition.System.BaseElement.CostPerKg;

            return cost;
        }
    }
}
