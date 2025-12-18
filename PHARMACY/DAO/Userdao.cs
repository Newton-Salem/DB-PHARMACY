using Microsoft.Data.SqlClient;
using PHARMACY.Data;
using PHARMACY.Model;
using System.Collections.Generic;

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



        public List<User> GetAllUsers()
        {
            List<User> users = new();

            string query = @"
                SELECT Username, Name, Role
                FROM [USER]
                ORDER BY Username
            ";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);

            con.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    Username = reader.GetString(0),
                    Name = reader.GetString(1),
                    Role = reader.GetString(2)
                });
            }

            return users;
        }

        // ================= Delete User =================
        public void DeleteUser(string username)
        {
            using SqlConnection con = db.GetConnection();
            con.Open();

            // 1️⃣ هات UserID
            SqlCommand getIdCmd = new(
                "SELECT UserID FROM [USER] WHERE Username = @username", con);
            getIdCmd.Parameters.AddWithValue("@username", username);

            object result = getIdCmd.ExecuteScalar();
            if (result == null) return;

            int userId = Convert.ToInt32(result);

            // 2️⃣ امسح العلاقات (الترتيب مهم جدًا)
            string[] queries =
            {
        "DELETE FROM Feedback WHERE CustomerID = @id",
        "DELETE FROM [NOTIFICATION] WHERE UserID = @id",
        "DELETE FROM [Payment] WHERE Order_ID IN (SELECT Order_ID FROM [Order] WHERE CustomerID = @id)",
        "DELETE FROM Order_Medicine WHERE Order_ID IN (SELECT Order_ID FROM [Order] WHERE CustomerID = @id)",
        "DELETE FROM [Order] WHERE CustomerID = @id",

        "DELETE FROM Customer WHERE UserID = @id",
        "DELETE FROM Pharmacist WHERE UserID = @id",
        "DELETE FROM Supplier WHERE UserID = @id",
        "DELETE FROM [ADMIN] WHERE UserID = @id"
    };

            foreach (var q in queries)
            {
                SqlCommand cmd = new(q, con);
                cmd.Parameters.AddWithValue("@id", userId);
                cmd.ExecuteNonQuery();
            }

            // 3️⃣ امسح اليوزر نفسه
            SqlCommand deleteUserCmd = new(
                "DELETE FROM [USER] WHERE UserID = @id", con);
            deleteUserCmd.Parameters.AddWithValue("@id", userId);
            deleteUserCmd.ExecuteNonQuery();
        }

    }
}
