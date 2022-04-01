// See https://aka.ms/new-console-template for more information
using Flashcards;
using System.Configuration;
using System.Data.SqlClient;

internal class StackController
{
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
    internal List<TableToStackContextDTO> Get()
    {
        var stackTable = new List<TableToStackContextDTO>();

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
                new TableToStackContextDTO
                {
                    StackID = reader.GetInt32(0),
                    StackName = reader.GetString(1)
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
        command.CommandText = "INSERT INTO Stacks (StacksName) Values (@name)";

        command.Parameters.AddWithValue("@name", name);

        command.ExecuteNonQuery();
    }

    internal void Delete(TableToStackContextDTO stack)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();

        connection.Open();
        command.CommandText = "DELETE from Stacks WHERE CONVERT(NVARCHAR, StacksName)=@stackName";

        command.Parameters.AddWithValue("@stackName", stack.StackName);

        command.ExecuteNonQuery();

        Console.WriteLine($"\nThe '{stack.StackName}' Stack has been Deleted");
    }

    internal void Update(string newName, TableToStackContextDTO stack)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();

        connection.Open();
        command.CommandText = 
            @"UPDATE Stacks
                SET
                    StacksName = @newName
                WHERE
                CONVERT
                    (NVARCHAR, StacksName) = @stackName
            ";

        command.Parameters.AddWithValue("@newName", newName);
        command.Parameters.AddWithValue("@stackName", stack.StackName);

        command.ExecuteNonQuery();

        Console.WriteLine($"\nThe '{stack.StackName}' Stack has been Update to: {newName}");
    }
}