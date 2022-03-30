// See https://aka.ms/new-console-template for more information
using System.Configuration;


class Program
{
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
    static void Main(String[] args)
    {
        TableInitializer stackInitializer = new();
        GetUserInput getUserInput = new();

        stackInitializer.CreateTable(connectionString);
        getUserInput.MainMenu();
    }
}
