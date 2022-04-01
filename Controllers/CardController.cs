// See https://aka.ms/new-console-template for more information
using Flashcards;
using System.Configuration;
using System.Data.SqlClient;

internal class CardController
{
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
    TableToStackContextDTO currentStack = SetTableContext.TableContext;
    internal List<TableToCardsContextDTO> GetCards()
    {
        var cardsTable = new List<TableToCardsContextDTO>();

        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();

        connection.Open();
        command.CommandText = "SELECT * FROM Cards WHERE StacksID = @currentStack";

        command.Parameters.AddWithValue("@currentStack", currentStack.StackID);
        using var reader = command.ExecuteReader();
        if(reader.HasRows)
        {
            while(reader.Read())
            {
                cardsTable.Add(
                new TableToCardsContextDTO
                {
                    CardsName = reader.GetString(1),
                });
            }
        }
        else
        {
            Console.WriteLine("\n\nNo rows found.\n\n");
        }

        return cardsTable;
    }

    internal void Post(string name)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();

        connection.Open();
        command.CommandText = "INSERT INTO Cards (CardsName, StacksID) VALUES (@name, @currentStack)";

        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@currentStack", currentStack.StackID);

        command.ExecuteNonQuery();
    }

    internal void Delete(TableToCardsContextDTO card)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();

        connection.Open();
        command.CommandText = "DELETE from Cards WHERE CONVERT(NVARCHAR, CardsName)=@cards";

        command.Parameters.AddWithValue("@cards", card.CardsName);
        command.ExecuteNonQuery();

        Console.WriteLine($"\nThe '{card.CardsName}' Card has been Deleted");
    }

    internal void Update(string? newName, TableToCardsContextDTO card)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();

        connection.Open();
        command.CommandText =
            @"UPDATE Cards
                SET
                    CardsName = @newName
                WHERE
                CONVERT
                    (NVARCHAR, CardsName) = @cardsName
            ";

        command.Parameters.AddWithValue("@newName", newName);
        command.Parameters.AddWithValue("@cardsName", card.CardsName);

        command.ExecuteNonQuery();

        Console.WriteLine($"\nThe '{card.CardsName}' Card has been Update to: '{newName}'");
    }
}