using AlloyOptimisation.Domain.Alloy;
using AlloyOptimisation.Domain.Calculators;

namespace AlloyOptimisation.Domain.Optimisers
{
    public class AlloyOptimiser
    {
        private readonly IAlloyEnumerator _enumerator;
        private readonly ICreepResistanceCalculator _creep;
        private readonly ICostCalculator _cost;

        public AlloyOptimiser(
            IAlloyEnumerator enumerator,
            ICreepResistanceCalculator creep,
            ICostCalculator cost)
        {
            _enumerator = enumerator;
            _creep = creep;
            _cost = cost;
        }

        public AlloyComposition? FindBest(AlloySystem system, double maxCostPerKg)
        {
            AlloyComposition? best = null;
            var bestCreep = double.MinValue;

            foreach (var composition in _enumerator.Enumerate(system))
            {
                var cost = _cost.Compute(composition);
                if (cost > maxCostPerKg)
                    continue;
                var creep = _creep.Compute(composition);
                if (creep > bestCreep)
                {
                    bestCreep = creep;
                    best = composition;
                }
            }

            return best;
        }
    }
}
