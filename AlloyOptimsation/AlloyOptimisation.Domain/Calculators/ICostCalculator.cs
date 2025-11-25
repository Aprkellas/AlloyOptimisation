using AlloyOptimisation.Domain.Alloy;

namespace AlloyOptimisation.Domain.Calculators
{
    public interface ICostCalculator
    {
        double Compute(AlloyComposition composition);
    }
}
