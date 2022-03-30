// See https://aka.ms/new-console-template for more information
using ConsoleTableExt;
using Flashcards;

internal class CardTableVisualisation
{
    internal static void ShowCardsTable<T>(List<T> tableData) where T : class
    {
        TableToStackContextDTO currentStack = SetTableContext.TableContext;
        Console.WriteLine("\n\n");

        ConsoleTableBuilder
            .From(tableData)
            .WithTitle(currentStack.StackName)
            .ExportAndWriteLine();
        Console.WriteLine("\n\n");
    }
}