using AlloyOptimisation.Domain.Alloy;

namespace AlloyOptimisation.Domain.Calculators
{
    public interface ICreeResistanceCalculator
    {
        double Compute(AlloyComposition composition);
    }
}
