using DataMigrator.Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class SQLServerWorker : ISQLServerWorker
{
  private readonly DatabaseOptions _databaseOptions;

  public SQLServerWorker(IOptionsMonitor<DatabaseOptions> optionsAccessor)
  {
    _databaseOptions = optionsAccessor.CurrentValue;
  }

  public List<Artist> GetArtists()
  {
    using (SqlConnection connection = new SqlConnection(_databaseOptions.SqlServerConnectionString))
    {
      string query = "select * from Artists;";

      // create the Command and Parameter objects.
      SqlCommand command = new SqlCommand(query, connection);

      // open the connection in a try/catch block. 
      // create and execute the DataReader, writing the result
      // set to the console window.
      try
      {
        connection.Open();
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          Console.WriteLine("\t{0}\t{1}\t{2}",
              reader[0], reader[1], reader[2]);
        }
        reader.Close();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      Console.ReadLine();
    }

    return new List<Artist>();
  }
}