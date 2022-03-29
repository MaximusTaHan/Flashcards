// See https://aka.ms/new-console-template for more information
using Flashcards;
using System.Configuration;
using System.Data.SqlClient;

internal class StackController
{
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
    internal List<StackModel> Get()
    {
        var stackTable = new List<StackModel>();

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

        return stackTable;
    }

    internal void Post(string name)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();

        connection.Open();
        command.CommandText = $"INSERT INTO Stacks (StacksName) Values ('{name}')";

        command.ExecuteNonQuery();
    }

    internal void Delete(StackModel stack)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();

        connection.Open();
        command.CommandText = $"DELETE from Stacks WHERE StacksID ='{stack.StackId}'";
        command.ExecuteNonQuery();

        Console.WriteLine($"\nThe '{stack.StackName}' Stack has been Deleted");
    }

    internal void Update(string newName, StackModel stack)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();

        connection.Open();
        command.CommandText = 
            $@"UPDATE Stacks
                SET
                    StacksName = '{newName}'
                WHERE
                    StacksID = {stack.StackId}
            ";
        command.ExecuteNonQuery();

        Console.WriteLine($"\nThe '{stack.StackName}' Stack has been Update to: {newName}");
    }
}