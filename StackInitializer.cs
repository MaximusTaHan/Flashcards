// See https://aka.ms/new-console-template for more information
using System.Data.SqlClient;

internal class StackInitializer
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
                CardsID int,
                CardsName text,
                StacksID int FOREIGN KEY REFERENCES Stacks(StacksID)
             )";

        command.ExecuteNonQuery();
    }
}