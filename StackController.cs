// See https://aka.ms/new-console-template for more information
using Flashcards;
using System.Configuration;
using System.Data.SqlClient;

internal class StackController
{
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
    internal void Get()
    {
        List<StackModel> stackTable = new List<StackModel>();

        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();

        connection.Open();
        command.CommandText = "SELECT * FROM Stacks";

        using var reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            while(reader.Read())
            {
                stackTable.Add(
                new StackModel
                {
                    StackId = reader.GetInt32(0),
                    StackName = reader.GetString(1),
                });
            }
        }
        else
        {
            Console.WriteLine("\n\nNo rows found.\n\n");
        }
    }
}