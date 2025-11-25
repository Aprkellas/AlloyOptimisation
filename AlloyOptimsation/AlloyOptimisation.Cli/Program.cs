using AlloyOptimisation.Domain.Alloy;
using AlloyOptimisation.Domain.Calculators;
using AlloyOptimisation.Domain.Elements;
using Spectre.Console;

namespace AlloyOptimisation.Cli;

public record SpecAlloy(string Name, double Cr, double Co, double Nb, double Mo);

public class Program
{
    public static void Main()
    {
        var ni = new ElementDefinition("Ni", 0.0, 8.9);
        var cr = new ElementDefinition("Cr", 2.0911350E+16, 14.0);
        var co = new ElementDefinition("Co", 7.2380280E+16, 80.5);
        var nb = new ElementDefinition("Nb", 1.0352738E+16, 42.5);
        var mo = new ElementDefinition("Mo", 8.9124547E+16, 16.0);

        var system = new AlloySystem(ni, [cr, co, nb, mo]);

        // Table 3 alloys from the spec
        var alloys = new[]
        {
            new SpecAlloy("Alloy A: Cr15 Co10 Nb1 Mo2",  15.0, 10.0, 1.0, 2.0),
            new SpecAlloy("Alloy B: Cr20 Co0  Nb0 Mo1.5",20.0,  0.0, 0.0, 1.5),
            new SpecAlloy("Alloy C: Cr22 Co25 Nb1.5 Mo6",22.0, 25.0, 1.5, 6.0),
        };

        var creepCalculator = new NickelCreepResistanceCalculator();
        var costCalculator = new SimpleCostCalculator();

        

        bool running = true;

        while (running)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText("Alloy Optimisation")
                    .Centered()
                    .Color(Color.Yellow)
            );

            AnsiConsole.WriteLine();
            AnsiConsole.Write(
                new Rule("[blue]Select alloy[/]").Centered()
            );

            // Prompt user to choose an alloy or exit
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an [green]alloy[/] from Table 3 (Esc to exit):")
                    .PageSize(10)
                    .AddChoices(alloys.Select(a => a.Name).Concat(["Exit"]))
            );

            if (choice == "Exit")
            {
                running = false;
                break;
            }

            var chosenAlloy = alloys.First(a => a.Name == choice);

            // Build composition from chosen alloy
            var composition = new AlloyComposition(
                system, 
                new Dictionary<ElementDefinition, double>
                {
                    [cr] = chosenAlloy.Cr,
                    [co] = chosenAlloy.Co,
                    [nb] = chosenAlloy.Nb,
                    [mo] = chosenAlloy.Mo
                }
            );

            var creep = creepCalculator.Compute(composition);
            var cost = costCalculator.Compute(composition);

            // Table for composition
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

            // Panel with creep + cost
            var grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();

            grid.AddRow("[green]Creep resistance:[/]", $"[bold]{creep:E3}[/] m^2/s");
            grid.AddRow("[yellow]Cost:[/]", $"[bold]{cost:F2}[/] £/kg");

            var panel = new Panel(grid)
            {
                Border = BoxBorder.Rounded,
                Header = new PanelHeader("Result", Justify.Center),
                Padding = new Padding(1, 1, 1, 1)
            };

            AnsiConsole.Write(panel);
            AnsiConsole.WriteLine();

            AnsiConsole.MarkupLine(
                "\nPress [green]Enter[/] to go back to the alloy list, or [red]Esc[/] to exit.");

            while (true)
            {
                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                {
                    // break inner loop, go back to selection prompt
                    break;
                }

                if (key.Key == ConsoleKey.Escape)
                {
                    running = false;
                    break;
                }
            }
        }

        AnsiConsole.MarkupLine("\n[grey]Goodbye![/]");
    }
}
