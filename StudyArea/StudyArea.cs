// See https://aka.ms/new-console-template for more information
using Flashcards;

internal class StudyArea
{
    StackController stackController = new();
    ScoreController scoreController = new();
    internal void StudyMenu()
    {
        bool leaveStudy = false;

        
        Console.Clear();

        while(leaveStudy == false)
        {
            Console.WriteLine("\n\nSTUDY AREA");
            Console.WriteLine("Here you can choose a stack to study.\nWhen you feel ready, hide the cards and try get them correct (In Order).");

            Console.WriteLine("\nType 0 to leave Study Area");
            Console.WriteLine("Type 1 to Choose a Stack");
            Console.WriteLine("Type 2 to Display all Scores");
            Console.WriteLine("Type 3 to Delete a chosen Scores");
            Console.WriteLine("Type 4 to Display number of Attempts per month");

            Console.WriteLine("\nThe Score of your study will be saved to the corresponding Stack");

            string commandInput = Console.ReadLine();

            while(string.IsNullOrEmpty(commandInput))
            {
                Console.WriteLine("\nInvalid Command. Please type a number from 0 to 3");
                commandInput = Console.ReadLine();
            }

            switch (commandInput)
            {
                case "0":
                    leaveStudy = true;
                    Console.Clear();
                    break;
                case "1":
                    ProcessStack();
                    break;
                case "2":
                    ProcessScoresGet();
                    break;
                case "3":
                    ProcessScoresDelete();
                    break;
                case "4":
                    ProcessSessionHistory();
                    break;
            }    
        }
    }

    private void ProcessSessionHistory()
    {
        var scoresListDTO = scoreController.GetSessionHistory();
        TableVisualisation.ShowTable(scoresListDTO);
    }

    private void ProcessScoresDelete()
    {
        var scoresListDTO = scoreController.GetScores();
        TableVisualisation.ShowTable(scoresListDTO);

        Console.WriteLine("\nPlease type ID of the Score you want to Delete (or 0 to return to Study Menu).");
        int scoreId;
        var tryInput = Console.ReadLine();

        if (tryInput == "0")
        {
            Console.Clear();
            return;
        }

        while(!int.TryParse(tryInput, out scoreId))
        {
            Console.WriteLine("Please enter a valid number");
            tryInput = Console.ReadLine();
        }
        
        foreach(var score in scoresListDTO)
        {
            if(score.ScoresID == scoreId)
            {
                scoreController.Delete(scoreId);
            }
        }
    }

    private void ProcessScoresGet()
    {
        var scoresListDTO = scoreController.GetScores();
        TableVisualisation.ShowTable(scoresListDTO);
    }

    //Currently allows Stacks with no cards in it
    private void ProcessStack()
    {
        var stackTable = stackController.Get();
        TableVisualisation.ShowTable(stackTable);

        Console.WriteLine("\nPlease type name of Stack you want to Study (or 0 to return to Main Menu).");

        string nameInput = Console.ReadLine();

        var stack = TryFindName(nameInput, stackTable);

        SetTableContext.TableContext = stack;

        StudySession studySession = new();
        studySession.PreparationSession();
    }

    private TableToStackContextDTO TryFindName(string? nameInput, List<TableToStackContextDTO> stackModel)
    {
        bool found = false;
        if (nameInput == "0")
            StudyMenu();

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
                Console.WriteLine("\nCould not find a Stack with that name.\nPress 0 to return to Study Menu.\n\nTry Again: ");
                nameInput = Console.ReadLine();

                if (nameInput == "0")
                    StudyMenu();
            }
        }
        // should never be reached
        return foundStack;
    }
}