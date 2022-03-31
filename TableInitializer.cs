// See https://aka.ms/new-console-template for more information
using System.Data.SqlClient;

internal class TableInitializer
{
    internal void CreateTable(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();
        connection.Open();

        command.CommandText =
            @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Stacks' and xtype='U')
                CREATE TABLE Stacks (
		        StacksID int IDENTITY(1,1) PRIMARY KEY,
                StacksName text,
            )
            ";
        command.ExecuteNonQuery();

        command.CommandText =
            @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Cards' and xtype='U')
                CREATE TABLE Cards (
                CardsID int IDENTITY(1,1),
                CardsName text,
                StacksID int FOREIGN KEY REFERENCES Stacks(StacksID)
                ON DELETE CASCADE
                ON UPDATE CASCADE
             )";

        command.ExecuteNonQuery();

        command.CommandText =
            @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Scores' and xtype='U')
                CREATE TABLE Scores (
                ScoresID int IDENTITY(1,1),
                stack_name VARCHAR(10),
                score INT,
                date DATE,
                start_time TIME,
                end_time TIME,
                StacksID int FOREIGN KEY REFERENCES Stacks(StacksID)
                ON DELETE CASCADE
                ON UPDATE CASCADE
             )";

        command.ExecuteNonQuery();
    }
}