// See https://aka.ms/new-console-template for more information
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

            string commandInput = Console.ReadLine();

            while(string.IsNullOrEmpty(commandInput))
            {
                Console.WriteLine("\nInvalid Command. Please type a number from 0 to 5.\n");
                commandInput = Console.ReadLine();
            }

            switch (commandInput)
            {
                case "0":
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    stackController.Get();
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;

            }
        }
    }
}