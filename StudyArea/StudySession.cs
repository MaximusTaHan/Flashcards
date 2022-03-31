// See https://aka.ms/new-console-template for more information
using Flashcards;

internal class StudySession
{
    CardController cardController = new();
    internal void PreparationSession()
    {
        Console.Clear();
        Console.WriteLine("\n\nPreparation!");
        Console.WriteLine("Memories the cards in order\n");

        var viewCards = cardController.GetCards();
        CardTableVisualisation.ShowCardsTable(viewCards);

        Console.WriteLine("\nHit any key to start the test (or 0 to Quit this session)");
        var commandInput = Console.ReadLine();
        
        if(commandInput == "0")
        {
            return;
        }
        else
        {
            StartSession();
        }
    }

    private void StartSession()
    {
        var startDate = SetDateStamp();
        string stackName = SetTableContext.TableContext.StackName;
        int cardNumber = 1;

        int score = 0;
        Console.Clear();
        Console.WriteLine($"\n\nStart Time: {startDate.ToString("HH:mm:ss")}");
        Console.WriteLine("Return to Study Session to see the Cards again by typing 0 (Score will be discarded).\n");

        foreach (var card in cardController.GetCards())
        {
            Console.Write($"Guess card {cardNumber}: ");
            var guess = Console.ReadLine();

            if (guess == "0")
            {
                Console.Clear();
                return;
            }
            if(guess == card.CardsName)
                score++;
            cardNumber++;
        }

        var endDate = SetDateStamp();

        TimeSpan duration = endDate - startDate;
        Console.WriteLine("Time on test: " + duration.ToString("mm\\:ss"));
        Console.WriteLine("Your score is: " + score);

        ScoreController scoreController = new();
        scoreController.PostScore(startDate, endDate, score, stackName);
    }



    private DateTime SetDateStamp()
    {
        return DateTime.Now;
    }
}