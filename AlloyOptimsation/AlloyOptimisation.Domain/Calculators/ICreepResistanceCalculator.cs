using AlloyOptimisation.Domain.Alloy;

namespace AlloyOptimisation.Domain.Calculators
{
    public interface ICreepResistanceCalculator
    {
        double Compute(AlloyComposition composition);
    }
}
