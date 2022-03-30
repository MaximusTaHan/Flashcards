// See https://aka.ms/new-console-template for more information
using Flashcards;

internal class CardMenu
{
    bool leaveStack = false;
    CardController cardController = new();
    TableToStackContextDTO currentStack = SetTableContext.TableContext;
    internal void CardMainMenu()
    {
        Console.Clear();

        while(leaveStack == false)
        {
            Console.WriteLine($"\n\nCARD MENU. \nCurrent Stack: '{currentStack.StackName}'");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Leave stack.");
            Console.WriteLine("Type 1 to View all Cards.");
            Console.WriteLine("Type 2 to Create a Card.");
            Console.WriteLine("Type 3 to Delete a Card.");
            Console.WriteLine("Type 4 to Update a Card.");

            string? commandInput = Console.ReadLine();

            while (string.IsNullOrEmpty(commandInput))
            {
                Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                commandInput = Console.ReadLine();
            }

            switch (commandInput)
            {
                case "0":
                    leaveStack = true;
                    Console.Clear();
                    break;
                case "1":
                    ProcessGetCards();
                    break;
                case "2":
                    ProcessPost();
                    break;
                case "3":
                    ProcessDelete();
                    break;
                case "4":
                    ProcessUpdate();
                    break;

            }
        }
    }

    private void ProcessUpdate()
    {
        var cardDTO = cardController.GetCards();

        TableVisualisation.ShowTable(cardDTO);
        Console.WriteLine("\nPlease type name of the Card you want to Update (or 0 to return to Main Menu).");

        string nameInput = Console.ReadLine();

        var card = TryFindName(nameInput, cardDTO);

        Console.WriteLine($"Current Stack name: {card.CardsName}\n");
        Console.WriteLine($"Choose a new name for the '{card.CardsName}' Card. ");
        Console.Write("\nName: ");
        string newName = Console.ReadLine();

        cardController.Update(newName, card);
    }

    private void ProcessDelete()
    {
        var cardDTO = cardController.GetCards();
        CardTableVisualisation.ShowCardsTable(cardDTO);

        Console.WriteLine("\nPlease type name of the Card you want to Delete (or 0 to return to Main Menu).");

        string nameInput = Console.ReadLine();

        var stack = TryFindName(nameInput, cardDTO);

        cardController.Delete(stack);
    }

    private void ProcessPost()
    {
        string name = GetNameInput();

        cardController.Post(name);
    }

    private void ProcessGetCards()
    {
        var cardsTable = cardController.GetCards();
        CardTableVisualisation.ShowCardsTable(cardsTable);
    }

    private string GetNameInput()
    {
        string name;
        Console.WriteLine("\n\nPlease enter what your Card will say\n");

        name = Console.ReadLine();

        Console.WriteLine($"You chose: {name}. \nPress 1 to confirm.\nPress any key to Write a new Text (press 0 to return to Card Menu).");
        string check = Console.ReadLine();

        if (check == "1")
            return name;

        else if (check == "0")
            CardMainMenu();

        else
            GetNameInput();

        return name;
    }
    private TableToCardsContextDTO TryFindName(string? nameInput, List<TableToCardsContextDTO> stackModel)
    {
        bool found = false;
        if (nameInput == "0")
            CardMainMenu();

        TableToCardsContextDTO foundCard = new();
        while (found == false)
        {
            foreach (var card in stackModel)
            {
                if (card.CardsName == nameInput)
                {
                    foundCard = card;
                    found = true;
                }
            }
            if (found == false)
            {
                Console.WriteLine("\nCould not find a Stack with that name.\nPress 0 to return to Main Menu.\n\nTry Again: ");
                nameInput = Console.ReadLine();

                if (nameInput == "0")
                    CardMainMenu();
            }
        }
        // should never be reached
        return foundCard;
    }
}