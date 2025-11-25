using AlloyOptimisation.Domain.Alloy;

namespace AlloyOptimisation.Domain.Calculators
{
    public interface ICostCaclulator
    {
        double Compute(AlloyComposition composition);
    }
}
