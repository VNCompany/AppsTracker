using System.Security.Cryptography;
using System.Text;

namespace DataLayer
{
    public partial class DatabaseService
    {
        public static string HashPassword(string pw)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] result = sha256.ComputeHash(Encoding.UTF8.GetBytes(pw));
            var sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x2"));
            }
            sha256.Clear();
            return sb.ToString();
        }

        public static string AntiSqlInjection(string input) => input.Replace("'", "\\'");
    }
}