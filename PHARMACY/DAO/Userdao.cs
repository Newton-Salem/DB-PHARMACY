using Microsoft.Data.SqlClient;
using PHARMACY.Data;
using PHARMACY.Model;
using System.Collections.Generic;

namespace PHARMACY.DAO
{
    public class UserDAO
    {
        DB db = DB.Instance;

        // ================= REGISTER USER =================
        public void InsertUser(string username, string password, string name,
                               string role, string email, string phone,
                               string address)
        {
            using SqlConnection con = db.GetConnection();
            con.Open();

            using SqlTransaction tx = con.BeginTransaction();

            try
            {
                string query = @"
                    INSERT INTO [USER]
                    (USERNAME, [PASSWORD], [NAME], [ROLE], Email, Phone, [Address])
                    OUTPUT INSERTED.UserID
                    VALUES
                    (@Username, @Password, @Name, @Role, @Email, @Phone, @Address)
                ";

                SqlCommand cmd = new(query, con, tx);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password); 
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Address", address);

                int userId = (int)cmd.ExecuteScalar();

                
                if (role == "Customer")
                {
                    string customerQuery = @"
                        INSERT INTO Customer (UserID, LoyaltyPoints, Gender, EmergencyPhone)
                        VALUES (@UserID, 0, NULL, NULL)
                    ";

                    SqlCommand customerCmd = new(customerQuery, con, tx);
                    customerCmd.Parameters.AddWithValue("@UserID", userId);
                    customerCmd.ExecuteNonQuery();
                }

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        // ================= CHECK USERNAME =================
        public bool UsernameExists(string username)
        {
            string query = "SELECT COUNT(*) FROM [USER] WHERE Username = @Username";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@Username", username);

            con.Open();
            return (int)cmd.ExecuteScalar() > 0;
        }

        // ================= LOGIN =================
        public SqlDataReader Login(string username, string password)
        {
            string query = @"
                SELECT UserID, Username, Role
                FROM [USER]
                WHERE Username=@Username AND Password=@Password
            ";

            SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);

            con.Open();
            return cmd.ExecuteReader();
        }

        // ================= GET ALL USERS =================
        public List<User> GetAllUsers()
        {
            List<User> users = new();

            string query = @"SELECT Username, Name, Role FROM [USER] ORDER BY Username";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);

            con.Open();
            using SqlDataReader r = cmd.ExecuteReader();

            while (r.Read())
            {
                users.Add(new User
                {
                    Username = r.GetString(0),
                    Name = r.GetString(1),
                    Role = r.GetString(2)
                });
            }

            return users;
        }

        // ================= DELETE USER =================
        public void DeleteUser(string username)
        {
            using SqlConnection con = db.GetConnection();
            con.Open();

            SqlCommand getIdCmd = new(
                "SELECT UserID FROM [USER] WHERE Username=@username", con);
            getIdCmd.Parameters.AddWithValue("@username", username);

            object result = getIdCmd.ExecuteScalar();
            if (result == null) return;

            int userId = Convert.ToInt32(result);

            string[] queries =
            {
                "DELETE FROM Feedback WHERE CustomerID=@id",
                "DELETE FROM [NOTIFICATION] WHERE UserID=@id",
                "DELETE FROM [Payment] WHERE Order_ID IN (SELECT Order_ID FROM [Order] WHERE CustomerID=@id)",
                "DELETE FROM Order_Medicine WHERE Order_ID IN (SELECT Order_ID FROM [Order] WHERE CustomerID=@id)",
                "DELETE FROM [Order] WHERE CustomerID=@id",
                "DELETE FROM Customer WHERE UserID=@id",
                "DELETE FROM Pharmacist WHERE UserID=@id",
                "DELETE FROM Supplier WHERE UserID=@id",
                "DELETE FROM [ADMIN] WHERE UserID=@id"
            };

            foreach (var q in queries)
            {
                SqlCommand cmd = new(q, con);
                cmd.Parameters.AddWithValue("@id", userId);
                cmd.ExecuteNonQuery();
            }

            SqlCommand deleteUserCmd = new(
                "DELETE FROM [USER] WHERE UserID=@id", con);
            deleteUserCmd.Parameters.AddWithValue("@id", userId);
            deleteUserCmd.ExecuteNonQuery();
        }

        // ================= PASSWORD =================
        public string? GetPasswordByUsername(string username)
        {
            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(
                "SELECT [PASSWORD] FROM [USER] WHERE USERNAME=@username", con);
            cmd.Parameters.AddWithValue("@username", username);

            con.Open();
            return cmd.ExecuteScalar()?.ToString();
        }

        public void UpdatePassword(string username, string newPassword)
        {
            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(
                "UPDATE [USER] SET [PASSWORD]=@pass WHERE USERNAME=@username", con);

            cmd.Parameters.AddWithValue("@pass", newPassword);
            cmd.Parameters.AddWithValue("@username", username);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        // ================= PROFILE =================
        public User? GetUserByUsername(string username)
        {
            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(@"
                SELECT Name, Email, Phone, Address
                FROM [USER] WHERE Username=@username", con);

            cmd.Parameters.AddWithValue("@username", username);
            con.Open();

            using SqlDataReader r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new User
            {
                Name = r.GetString(0),
                Email = r.IsDBNull(1) ? "" : r.GetString(1),
                Phone = r.IsDBNull(2) ? "" : r.GetString(2),
                Address = r.IsDBNull(3) ? "" : r.GetString(3)
            };
        }

        public void UpdateProfile(string username, string name, string email, string phone, string address)
        {
            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(@"
                UPDATE [USER]
                SET Name=@name, Email=@email, Phone=@phone, Address=@address
                WHERE Username=@username", con);

            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@address", address);
            cmd.Parameters.AddWithValue("@username", username);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        // ================= INSERT ADMIN =================
        public void InsertAdmin(string username, string password, string name,
                                string email, string phone, string address,
                                decimal salary)
        {
            if (UsernameExists(username))
                throw new Exception("Username already exists");

            using SqlConnection con = db.GetConnection();
            con.Open();
            using SqlTransaction tx = con.BeginTransaction();

            try
            {
                string userQuery = @"
                    INSERT INTO [USER]
                    (USERNAME, [PASSWORD], [NAME], [ROLE], Email, Phone, [Address])
                    OUTPUT INSERTED.UserID
                    VALUES
                    (@Username, @Password, @Name, 'Admin', @Email, @Phone, @Address)
                ";

                SqlCommand userCmd = new(userQuery, con, tx);
                userCmd.Parameters.AddWithValue("@Username", username);
                userCmd.Parameters.AddWithValue("@Password", password);
                userCmd.Parameters.AddWithValue("@Name", name);
                userCmd.Parameters.AddWithValue("@Email", email);
                userCmd.Parameters.AddWithValue("@Phone", phone);
                userCmd.Parameters.AddWithValue("@Address", address);

                int userId = (int)userCmd.ExecuteScalar();

                string adminQuery = @"
                    INSERT INTO [ADMIN] (UserID, Salary, HireDate)
                    VALUES (@UserID, @Salary, GETDATE())
                ";

                SqlCommand adminCmd = new(adminQuery, con, tx);
                adminCmd.Parameters.AddWithValue("@UserID", userId);
                adminCmd.Parameters.AddWithValue("@Salary", salary);
                adminCmd.ExecuteNonQuery();

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }
    }
}
