using AlloyOptimisation.Domain.Alloy;
using AlloyOptimisation.Domain.Calculators;
using AlloyOptimisation.Domain.Elements;
using Spectre.Console;

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
var costCalculator = new SimpleCostCalculator();
var creep = creepCalculator.Compute(composition);
var cost = costCalculator.Compute(composition);

// console output
var table = new Table()
    .Border(TableBorder.Rounded)
    .AddColumn("Element")
    .AddColumn("Atomic %");

table.AddRow("Cr", composition.AtomicPercents[cr].ToString("0.###"));
table.AddRow("Co", composition.AtomicPercents[co].ToString("0.###"));
table.AddRow("Nb", composition.AtomicPercents[nb].ToString("0.###"));
table.AddRow("Mo", composition.AtomicPercents[mo].ToString("0.###"));
table.AddRow("[grey]Ni (balance)[/]", composition.BaseElementPercent.ToString("0.###"));

AnsiConsole.Write(table);
AnsiConsole.WriteLine();

var panelContent = new Markup(
    $"[green]Creep resistance:[/] [bold]{creep:E3}[/] m^2/s\n" +
    $"[yellow]Cost:[/] [bold]{cost:F2}[/] £/kg"
);

var panel = new Panel(panelContent)
{
    Border = BoxBorder.Rounded,
    Header = new PanelHeader("Result", Justify.Center),
    Padding = new Padding(1, 1, 1, 1)
};

AnsiConsole.Write(panel);
AnsiConsole.WriteLine();

//Console.WriteLine("Nickel alloy demo (Table 3 example)");
//Console.WriteLine($"Cr: {composition.AtomicPercents[cr]} at.%");
//Console.WriteLine($"Co: {composition.AtomicPercents[co]} at.%");
//Console.WriteLine($"Nb: {composition.AtomicPercents[nb]} at.%");
//Console.WriteLine($"Mo: {composition.AtomicPercents[mo]} at.%");
//Console.WriteLine($"Ni (balance): {composition.BaseElementPercent} at.%");
//Console.WriteLine();
//Console.WriteLine($"Creep resistance: {creep:E3} m^2/s");
