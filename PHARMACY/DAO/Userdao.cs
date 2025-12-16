using Microsoft.Data.SqlClient;
using PHARMACY.Data;

namespace PHARMACY.DAO
{
    public class UserDAO
    {
        DB db = DB.Instance;

        // REGISTER
        public void InsertUser(string username, string password, string name,
                               string role, string email, string phone,
                               string address)
        {
            string query = @"
                INSERT INTO [USER]
                (USERNAME, PASSWORD, NAME, ROLE, Email, Phone, Address)
                VALUES
                (@Username, @Password, @Name, @Role, @Email, @Phone, @Address)
            ";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Role", role);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@Address", address);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        // LOGIN
        public SqlDataReader Login(string username, string password)
        {
            string query = @"
                SELECT UserID, Username, Role
                FROM [USER]
                WHERE Username=@Username AND Password=@Password
            ";

            SqlConnection con = db.GetConnection();
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);

            con.Open();
            return cmd.ExecuteReader();
        }
    }
}
