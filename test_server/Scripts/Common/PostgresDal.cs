using System.Data;
using Npgsql;
using TestServer.Scripts.Common.Logs;
using System.Diagnostics;

namespace TestServer.Scripts.Common;

public class PostgresDal
{
    public async Task<int> ExecuteNonQueryAsync(string sql, NpgsqlParameter[]? parameters = null)
    {
        try
        {
            await using var connection = new NpgsqlConnection(AppSettings.ConnectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            var rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected;
        }
        catch (Exception ex)
        {
            Logger.Error($"{ex.Message}. Request: {sql}", "Database");
            return -1;
        }
    }

    public async Task<DataTable> ExecuteQueryAsync(string sql, NpgsqlParameter[]? parameters = null)
    {
        var dataTable = new DataTable();

        try
        {
            await using var connection = new NpgsqlConnection(AppSettings.ConnectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            using var adapter = new NpgsqlDataAdapter(command);
            adapter.Fill(dataTable);
        }
        catch (Exception ex)
        {
            Logger.Error($"{ex.Message}. Request: {sql}", "Database");
        }

        return dataTable;
    }
}