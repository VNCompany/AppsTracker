using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;

using DataLayer.Models;

namespace DataLayer.Repositories
{
    public class AppsRepository
    {
        DatabaseService parent;

        private App _Map(NpgsqlDataReader reader)
        {
            return new App((int)reader["id"], (string)reader["name"], (int)reader["owner_id"], reader.GetDateTime(3));
        }

        internal AppsRepository(DatabaseService parent)
        {
            this.parent = parent;
        }

        public long Count(int ownerId)
        {
            var result = parent.ExecuteScalar($"SELECT COUNT(*) FROM apps WHERE owner_id={ownerId}");
            return result != null ? (long)result : 0;
        }

        public IEnumerable<App> GetList(int ownerId)
        {
            using (NpgsqlCommand command = parent.ExecuteCommand($"SELECT * FROM apps WHERE owner_id={ownerId}"))
            {
                using (NpgsqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                        yield return _Map(dataReader);
                }
            }
        }

        public bool Create(int ownerId, string name)
        {
            using (NpgsqlCommand command = parent.ExecuteCommand("SELECT COUNT(*) FROM apps WHERE owner_id=@ownerId AND name=@name"))
            {
                command.Parameters.AddWithValue("ownerId", ownerId);
                command.Parameters.AddWithValue("name", name);
                command.Prepare();

                if (((long)command.ExecuteScalar()!) > 0)
                    return false;
            }

            using (NpgsqlCommand command = parent.ExecuteCommand("INSERT INTO apps (name, owner_id, date) VALUES (@name, @ownerId, @date)"))
            {
                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("ownerId", ownerId);
                command.Parameters.AddWithValue("date", DateTime.Now);
                command.Prepare();

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool NewEvent(AppEvent appEvent)
        {
            using (NpgsqlCommand command = parent.ExecuteCommand("SELECT u_event_new (@appId, @name, @description, @date)"))
            {
                command.Parameters.AddWithValue("appId", NpgsqlDbType.Integer, appEvent.AppId!);
                command.Parameters.AddWithValue("name", appEvent.Name!);
                command.Parameters.AddWithValue("description", appEvent.Description!);
                command.Parameters.AddWithValue("date", appEvent.Date!);
                command.Prepare();

                return (bool)command.ExecuteScalar()!;
            }
        }
    }
}
