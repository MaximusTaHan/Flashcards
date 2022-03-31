// See https://aka.ms/new-console-template for more information
using Flashcards;
using Flashcards.StudyArea;
using System.Configuration;
using System.Data.SqlClient;

internal class ScoreController
{
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
    TableToStackContextDTO currentStack = SetTableContext.TableContext;

    internal List<TableToScoreContextDTO> GetScores()
    {
        var scoreTable = new List<TableToScoreContextDTO>();

        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();

        connection.Open();
        command.CommandText = $"SELECT * FROM Scores";

        using var reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                scoreTable.Add(
                new TableToScoreContextDTO
                {
                    ScoresID = reader.GetInt32(0),
                    StackName = reader.GetString(1),
                    Score = reader.GetInt32(2),
                    Date = reader.GetDateTime(3),
                    StartTime = reader.GetTimeSpan(4),
                    EndTime = reader.GetTimeSpan(5),
                    Duration =  reader.GetTimeSpan(5) - reader.GetTimeSpan(4)
                });
            }
        }
        else
        {
            Console.WriteLine("\n\nNo rows found.\n\n");
        }

        return scoreTable;
    }

    internal void PostScore(DateTime startDate, DateTime endTime, int score, string stackName)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();

        connection.Open();
        command.CommandText = $@"INSERT INTO Scores (date, start_time, end_time, StacksID, score, stack_name)
            VALUES ('{startDate}', '{startDate}', '{endTime}', '{currentStack.StackID}', '{score}', '{stackName}')";

        command.ExecuteNonQuery();
    }

    internal void Delete(int scoreId)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();

        connection.Open();
        command.CommandText = $"DELETE from Scores WHERE ScoresID='{scoreId}'";
        command.ExecuteNonQuery();

        Console.WriteLine("Score entry was Deleted");
    }
}