using AlloyOptimisation.Domain.Alloy;
using AlloyOptimisation.Domain.Calculators;
using AlloyOptimisation.Domain.Elements;

// build elements from spec
var ni = new ElementDefinition("Ni", 0.0, 8.9);
var cr = new ElementDefinition("Cr", 2.0911350E+16, 14.0);
var co = new ElementDefinition("Co", 7.2380280E+16, 80.5);
var nb = new ElementDefinition("Nb", 1.0352738E+16, 42.5);
var mo = new ElementDefinition("Mo", 8.9124547E+16, 16.0);

var system = new AlloySystem(ni, [cr, co, nb, mo]);

// use one of the Table 3 alloys
var composition = new AlloyComposition(system, new Dictionary<ElementDefinition, double>
{
    [cr] = 15.0,
    [co] = 10.0,
    [nb] = 1.0,
    [mo] = 2.0
});

var creepCalculator = new NickelCreepResistanceCalculator();
var creep = creepCalculator.Compute(composition);

Console.WriteLine("Nickel alloy demo (Table 3 example)");
Console.WriteLine($"Cr: {composition.AtomicPercents[cr]} at.%");
Console.WriteLine($"Co: {composition.AtomicPercents[co]} at.%");
Console.WriteLine($"Nb: {composition.AtomicPercents[nb]} at.%");
Console.WriteLine($"Mo: {composition.AtomicPercents[mo]} at.%");
Console.WriteLine($"Ni (balance): {composition.BaseElementPercent} at.%");
Console.WriteLine();
Console.WriteLine($"Creep resistance: {creep:E3} m^2/s");
