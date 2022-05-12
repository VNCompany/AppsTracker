using Npgsql;
using DataLayer.Models;

namespace DataLayer.Repositories
{
    public class UsersRepository
    {
        Database parent;

        internal UsersRepository(Database parent)
        {
            this.parent = parent;
        }

        public bool Create(string email, string password)
        {
            var value = parent.ExecuteScalar($"SELECT COUNT(*) FROM users WHERE email='{email}'");
            if (value != null && (long)value == 0)
            {
                parent.Execute($"INSERT INTO users (email, password) VALUES ('{email}', '{password}')");
                return true;
            }
            else return false;
        }

        public User? Get(string email)
        {
            using (var command = parent.ExecuteCommand($"SELECT * FROM users WHERE email='{email}'"))
            {
                using (var dataReader = command.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        return new User((int)dataReader["id"], (string)dataReader["email"], (string)dataReader["password"]);
                    }
                    else return null;
                }
            }
        }
    }
}