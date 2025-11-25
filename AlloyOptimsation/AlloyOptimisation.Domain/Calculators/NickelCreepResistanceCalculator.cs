using AlloyOptimisation.Domain.Alloy;

namespace AlloyOptimisation.Domain.Calculators
{
    public class NickelCreepResistanceCalculator : ICreepResistanceCalculator
    {
        public double Compute(AlloyComposition composition)
        {
            if (composition is null)
            {
                throw new ArgumentNullException(nameof(composition));
            }

            // sum alpha_i * x_i for all elements in the composition
            var creep = 0.0;

            foreach (var (element, percent) in composition.AtomicPercents)
            {
                creep += element.Alpha * percent;
            }
            
            return creep;
        }
    }
}
