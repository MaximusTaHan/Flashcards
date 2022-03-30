// See https://aka.ms/new-console-template for more information
using Flashcards;

internal class GetUserInput
{
    StackController stackController = new();
    internal void MainMenu()
    {
        bool closeApp = false;

        while(closeApp == false)
        {

            Console.WriteLine($"\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to Check out a Stack.");
            Console.WriteLine("Type 2 to Create a Stack.");
            Console.WriteLine("Type 3 to Delete a Stack.");
            Console.WriteLine("Type 4 to Update a Stack.");

            string? commandInput = Console.ReadLine();

            while(string.IsNullOrEmpty(commandInput))
            {
                Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                commandInput = Console.ReadLine();
            }

            switch (commandInput)
            {
                case "0":
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    ProcessIntoStack();
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

    private void ProcessIntoStack()
    {
        var stackTable = stackController.Get();
        TableVisualisation.ShowTable(stackTable);

        Console.WriteLine("\nPlease type name of Stack you want to Go Into (or 0 to return to Main Menu).");

        string nameInput = Console.ReadLine();

        var stack = TryFindName(nameInput, stackTable);
        
        SetTableContext.TableContext = stack;
        CardMenu cardMenu = new();
        cardMenu.CardMainMenu();
    }

    private void ProcessUpdate()
    {
        var stackModel = stackController.Get();

        TableVisualisation.ShowTable(stackModel);
        Console.WriteLine("\nPlease type name of Stack you want to Update (or 0 to return to Main Menu).");

        string nameInput = Console.ReadLine();

        var stack = TryFindName(nameInput, stackModel);

        Console.WriteLine($"Current Stack name: {stack.StackName}\n");
        Console.WriteLine($"Choose a new name for the '{stack.StackName}' Stack: ");

        string newName = Console.ReadLine();

        stackController.Update(newName, stack);
    }



    private void ProcessDelete()
    {
        var stackDTO = stackController.Get();

        TableVisualisation.ShowTable(stackDTO);
        Console.WriteLine("\nPlease type name of Stack you want to Delete (or 0 to return to Main Menu).");

        string nameInput = Console.ReadLine();

        var stack = TryFindName(nameInput, stackDTO);

        stackController.Delete(stack);
    }

    private void ProcessPost()
    {
        string name = GetNameInput();
        var stackModel = stackController.Get();

        var stack = TryPostName(name, stackModel);
        if(name == stack.StackName)
        {
            Console.WriteLine("Stack with that Name already exists");
            ProcessPost();
        }
        else
        {
            stackController.Post(name);
        }
    }


    // Currently a stack with the same name can be added to the DB. Deleting/Updating a Stack with the same name would Delete/Update the first one created.
    private string GetNameInput()
    {
        string name;
        Console.WriteLine("\n\nPlease enter a name for the card Stack\n");

        name = Console.ReadLine();

        Console.WriteLine($"You chose: {name}. \nPress 1 to confirm.\nPress any key to Write a new name (press 0 to return to Main Menu).");
        string check = Console.ReadLine();



        if (check == "1")
            return name;

        else if (check == "0")
            MainMenu();

        else
            GetNameInput();

        return name;
    }

    private TableToStackContextDTO TryPostName(string? nameInput, List<TableToStackContextDTO> stackModel)
    {
        if (nameInput == "0")
            MainMenu();

        TableToStackContextDTO foundStack = new();

        foreach (var stack in stackModel)
        {
            if (stack.StackName == nameInput)
            {
                foundStack = stack;
            }
        }
        // should never be reached
        return foundStack;
    }

    private TableToStackContextDTO TryFindName(string? nameInput, List<TableToStackContextDTO> stackModel)
    {
        bool found = false;
        if (nameInput == "0")
            MainMenu();

        TableToStackContextDTO foundStack = new();
        while (found == false)
        {
            foreach (var stack in stackModel)
            {
                if (stack.StackName == nameInput)
                {
                    foundStack = stack;
                    found = true;
                }
            }
            if (found == false)
            {
                Console.WriteLine("\nCould not find a Stack with that name.\nPress 0 to return to Main Menu.\n\nTry Again: ");
                nameInput = Console.ReadLine();

                if (nameInput == "0")
                    MainMenu();
            }
        }
        // should never be reached
        return foundStack;
    }
}