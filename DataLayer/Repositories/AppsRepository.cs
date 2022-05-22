using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;

using DataLayer.Models;
using DataLayer.ViewModels;

namespace DataLayer.Repositories
{
    public enum AppStatisticsPeriod
    {
        Day = 1, Month = 2, Year = 3
    }

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
            using (NpgsqlCommand command = parent.ExecuteCommand($"SELECT * FROM apps WHERE owner_id={ownerId} ORDER BY id DESC"))
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

        public AppEventsViewModel? GetStatistics(int ownerId, int appId, AppStatisticsPeriod period)
        {
            object? checkApp = parent.ExecuteScalar($"SELECT COUNT(*) > 0 as result FROM apps WHERE id = {appId} AND owner_id = {ownerId}");

            DateTime now = DateTime.Now;

            if ((bool)checkApp!)
            {
                string predicate = period switch
                {
                    AppStatisticsPeriod.Day => $"date::date = '{now.ToString("yyyy-MM-dd")}'::date",
                    AppStatisticsPeriod.Month => $"date_part('month', date) = {now.Month} AND date_part('year', date) = {now.Year}",
                    AppStatisticsPeriod.Year => $"date_part('year', date) = {now.Year}",
                    _ => ""
                };

                using (NpgsqlCommand command = parent.ExecuteCommand(
                    "SELECT name, COUNT(*) as count FROM events " +
                    $"WHERE app_id = {appId} AND " + predicate +
                    " GROUP BY name"))
                {
                    List<AppEventsGroup> eventsGroups = new List<AppEventsGroup>();

                    using (NpgsqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            eventsGroups.Add(new AppEventsGroup()
                            {
                                AppId = appId,
                                Name = (string)dataReader[0],
                                Count = Convert.ToInt32((long)dataReader[1])
                            });
                        }
                    }

                    return new AppEventsViewModel(eventsGroups);
                }
            }

            return null;
        }
    }
}
