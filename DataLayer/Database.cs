using Npgsql;
using System.IO;
using DataLayer.Repositories;

namespace DataLayer
{
    public partial class Database : IDisposable
    {
        static string? connString = null;
        static bool isInit = false;
        static string configureTablesSqlPath = "./query.sql";
        public static void Configure(string host, string user, string password, string dbname, string? csp = null)
        {
            connString = $"Host={host};Username={user};Password={password};Database={dbname}";

            if (csp != null)
                configureTablesSqlPath = csp;
        }

        NpgsqlConnection _conn;
        public Database()
        {
            if (connString != null)
            {
                _conn = new NpgsqlConnection(connString);
                _conn.Open();
                if (!isInit)
                {
                    isInit = true;
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = _conn;
                        cmd.CommandText = File.ReadAllText(configureTablesSqlPath);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            else
                throw new InvalidOperationException("Database not configured");
        }

        public int Execute(string query)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, _conn))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public NpgsqlCommand ExecuteCommand(string query)
        {
            return new NpgsqlCommand(query, _conn);
        }

        public object? ExecuteScalar(string query)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, _conn))
            {
                return cmd.ExecuteScalar();
            }
        }

        UsersRepository? users;
        public UsersRepository Users
        {
            get
            {
                if (users == null)
                    users = new UsersRepository(this);
                return users;
            }
        }

        public void Dispose()
        {
            _conn.Dispose();
        }
    }
}