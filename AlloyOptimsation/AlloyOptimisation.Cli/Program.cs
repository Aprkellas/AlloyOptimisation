using AlloyOptimisation.Domain.Alloy;
using AlloyOptimisation.Domain.Calculators;
using AlloyOptimisation.Domain.Elements;
using AlloyOptimisation.Domain.Optimisers;
using Spectre.Console;
using System.Linq;

namespace AlloyOptimisation.Cli;

public record SpecAlloy(string Name, double Cr, double Co, double Nb, double Mo);

public class Program
{
    private static readonly ElementDefinition Ni = new("Ni", 0.0, 8.9);
    private static readonly ElementDefinition Cr = new("Cr", 2.0911350E+16, 14.0);
    private static readonly ElementDefinition Co = new("Co", 7.2380280E+16, 80.5);
    private static readonly ElementDefinition Nb = new("Nb", 1.0352738E+16, 42.5);
    private static readonly ElementDefinition Mo = new("Mo", 8.9124547E+16, 16.0);

    private static readonly AlloySystem System = new(Ni, [Cr, Co, Nb, Mo]);

    private const string MenuCalculate = "Calculate Creep Resistance from Table 2";
    private const string MenuExit = "Exit";

    // predefined alloys from Table 3
    private static readonly SpecAlloy[] PredefinedAlloys =
    {
        new("Alloy A: Cr15 Co10 Nb1 Mo2", 15, 10, 1, 2),
        new("Alloy B: Cr20 Co0  Nb0 Mo1.5", 20, 0, 0, 1.5),
        new("Alloy C: Cr22 Co25 Nb1.5 Mo6", 22, 25, 1.5, 6),
    };

    // string safe usage of menu option
    private static readonly string[] CalculateString = [MenuCalculate];
    private static readonly Dictionary<string, SpecAlloy> AlloysByName =
        PredefinedAlloys.ToDictionary(a => a.Name, StringComparer.Ordinal);

    private static readonly NickelCreepResistanceCalculator CreepCalc = new();
    private static readonly SimpleCostCalculator CostCalc = new();

    public static void Main()
    {
        bool running = true;

        while (running)
        {
            ShowHeader();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an [green]alloy[/] from Table 3 or provide values in Table 2 (Esc to exit):")
                    .PageSize(10)
                    .AddChoices(
                        CalculateString
                            .Concat(PredefinedAlloys.Select(a => a.Name))
                            .Concat([MenuExit])
                    )
            );

            switch (choice)
            {
                case MenuExit:
                    running = false;
                    break;

                case MenuCalculate:
                    RunOptimisation();
                    running = AskContinue();
                    break;

                default:
                    if (AlloysByName.TryGetValue(choice, out var alloy))
                    {
                        ShowSingleAlloy(alloy);
                        running = AskContinue();
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Unknown selection[/]");
                    }
                    break;
            }
        }

        AnsiConsole.MarkupLine("\n[grey]Goodbye![/]");
    }

    private static void ShowHeader()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Alloy Optimisation")
                .Centered()
                .Color(Color.Yellow)
        );
        AnsiConsole.Write(new Rule("[blue]Select alloy[/]").Centered());
        AnsiConsole.WriteLine();
    }

    private static bool AskContinue()
    {
        AnsiConsole.MarkupLine(
            "\nPress [green]Enter[/] to return to menu or [red]Esc[/] to exit.");

        while (true)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Escape) return false;
            if (key == ConsoleKey.Enter) return true;
        }
    }

    private static void RenderComposition(AlloyComposition comp)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("Element")
            .AddColumn("Atomic %");

        foreach (var (el, pct) in comp.AtomicPercents)
            table.AddRow(el.Symbol, pct.ToString("0.###"));

        table.AddRow("[grey]Ni (balance)[/]", comp.BaseElementPercent.ToString("0.###"));
        AnsiConsole.Write(table);
    }

    private static void RenderResultPanel(double creep, double cost, string header)
    {
        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();
        grid.AddRow("[green]Creep resistance:[/]", $"[bold]{creep:E3}[/] m^2/s");
        grid.AddRow("[yellow]Cost:[/]", $"[bold]{cost:F2}[/] £/kg");

        var panel = new Panel(grid)
        {
            Border = BoxBorder.Rounded,
            Header = new PanelHeader(header, Justify.Center),
            Padding = new Padding(1, 1, 1, 1)
        };

        AnsiConsole.Write(panel);
    }

    private static void ShowSingleAlloy(SpecAlloy alloy)
    {
        var comp = new AlloyComposition(
            System,
            new Dictionary<ElementDefinition, double>
            {
                [Cr] = alloy.Cr,
                [Co] = alloy.Co,
                [Nb] = alloy.Nb,
                [Mo] = alloy.Mo
            }
        );

        var creep = CreepCalc.Compute(comp);
        var cost = CostCalc.Compute(comp);

        RenderComposition(comp);
        AnsiConsole.WriteLine();
        RenderResultPanel(creep, cost, "Result");
    }

    private static void RunOptimisation()
    {
        var enumerator = new GridAlloyEnumerator();
        enumerator.AddRange(new ElementRange(Cr, 14.5, 22.0, 0.5));
        enumerator.AddRange(new ElementRange(Co, 0.0, 25.0, 1.0));
        enumerator.AddRange(new ElementRange(Nb, 0.0, 1.5, 0.1));
        enumerator.AddRange(new ElementRange(Mo, 1.5, 6.0, 0.5));

        var optimiser = new AlloyOptimiser(enumerator, CreepCalc, CostCalc);
        AlloyComposition? best = null;

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start("Running optimisation…", _ =>
            {
                best = optimiser.FindBest(System, maxCostPerKg: 18.0);
            });

        if (best is null)
        {
            AnsiConsole.MarkupLine("[red]No alloy found under £18/kg![/]");
            return;
        }

        var creep = CreepCalc.Compute(best);
        var cost = CostCalc.Compute(best);

        RenderComposition(best);
        AnsiConsole.WriteLine();
        RenderResultPanel(creep, cost, "Best Alloy Found");
    }
}
