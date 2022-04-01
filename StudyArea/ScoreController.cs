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
        command.CommandText = "SELECT * FROM Scores";

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
                    Date = reader.GetDateTime(3).ToShortDateString(),
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
        command.CommandText = @"INSERT INTO Scores (date, start_time, end_time, StacksID, score, stack_name)
            VALUES (@startDate, @startDateStamp, @endTime, @currentStack, @score, @stackName)";

        command.Parameters.AddWithValue("@startDate", startDate);
        command.Parameters.AddWithValue("@startDateStamp", startDate);
        command.Parameters.AddWithValue("@endTime", endTime);
        command.Parameters.AddWithValue("@currentStack", currentStack.StackID );
        command.Parameters.AddWithValue("@score", score);
        command.Parameters.AddWithValue("@stackName", stackName);

        command.ExecuteNonQuery();
    }

    internal List<SessionHistoryDTO> GetSessionHistory()
    {
        List<SessionHistoryDTO> sessionHistory = new List<SessionHistoryDTO>();

        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();

        connection.Open();
        command.CommandText = @"
            DECLARE
                @columns NVARCHAR(MAX) = '',
                @sql NVARCHAR(MAX) = '';
            SELECT
                @columns += QUOTENAME(DATENAME(MONTH,date)) + ','
            FROM
                Scores
            GROUP BY
                DATENAME(MONTH,date)

            SET @columns = LEFT(@columns, LEN(@columns) - 1);

            SET @sql='SELECT * FROM
            (
                SELECT
	                stack_name,
	                ScoresID,
	                DATENAME(MONTH,date) as [month]
                FROM
	                Scores
            ) t

            PIVOT(
                COUNT(ScoresID)
                FOR [month] IN ('+ @columns +')
            ) AS pivot_table;'


            EXECUTE sp_executesql @sql;";

        using var reader = command.ExecuteReader();

        if(reader.HasRows)
        {
            for(int i = 0; i <= reader.FieldCount; i++)
            {
                sessionHistory.Add(new SessionHistoryDTO());

            }
        }
        else
        {
            Console.WriteLine("No rows found");
        }

        return sessionHistory;
    }

    internal void Delete(int scoreId)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();

        connection.Open();
        command.CommandText = "DELETE from Scores WHERE ScoresID=@scoreId";

        command.Parameters.AddWithValue("@scoreId", scoreId);
        command.ExecuteNonQuery();

        Console.WriteLine("Score entry was Deleted");
    }

}