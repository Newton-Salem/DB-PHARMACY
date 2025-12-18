using Microsoft.Data.SqlClient;
using PHARMACY.Data;
using PHARMACY.Models;

namespace PHARMACY.DAO
{
    public class MedicineDAO
    {
        DB db = DB.Instance;

        // GET ALL
        public List<Medicine> GetAll()
        {
            List<Medicine> list = new();
            string query = "SELECT * FROM Medicine";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);

            con.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Medicine
                {
                    Medicine_ID = (int)reader["Medicine_ID"],
                    Name = reader["Name"].ToString()!,
                    Stock_Quantity = (int)reader["Stock_Quantity"],
                    Price = (decimal)reader["Price"],
                    Expiry_Date = (DateTime)reader["Expiry_Date"]
                });
            }
            return list;
        }

        // SEARCH
        public List<Medicine> Search(string name)
        {
            List<Medicine> list = new();
            string query = "SELECT * FROM Medicine WHERE Name LIKE '%' + @name + '%'";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@name", name);

            con.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Medicine
                {
                    Medicine_ID = (int)reader["Medicine_ID"],
                    Name = reader["Name"].ToString()!,
                    Stock_Quantity = (int)reader["Stock_Quantity"],
                    Price = (decimal)reader["Price"],
                    Expiry_Date = (DateTime)reader["Expiry_Date"]
                });
            }
            return list;
        }

        // OUT OF STOCK
        public List<Medicine> OutOfStock()
        {
            List<Medicine> list = new();
            string query = "SELECT * FROM Medicine WHERE Stock_Quantity = 0";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);

            con.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Medicine
                {
                    Medicine_ID = (int)reader["Medicine_ID"],
                    Name = reader["Name"].ToString()!,
                    Stock_Quantity = (int)reader["Stock_Quantity"],
                    Price = (decimal)reader["Price"],
                    Expiry_Date = (DateTime)reader["Expiry_Date"]
                });
            }
            return list;
        }

        // ADD 
        public void Add(Medicine m)
        {
            string query = @"
                INSERT INTO Medicine (Name, Stock_Quantity, Price, Expiry_Date)
                VALUES (@n, @s, @p, @e)";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);

            cmd.Parameters.AddWithValue("@n", m.Name);
            cmd.Parameters.AddWithValue("@s", m.Stock_Quantity);
            cmd.Parameters.AddWithValue("@p", m.Price);
            cmd.Parameters.AddWithValue("@e", m.Expiry_Date);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        // UPDATE
        public void Update(int id, int? stock, DateTime? expiry)
        {
            List<string> updates = new();
            using SqlConnection con = db.GetConnection();

            if (stock.HasValue)
                updates.Add("Stock_Quantity = @s");

            if (expiry.HasValue)
                updates.Add("Expiry_Date = @e");

            if (updates.Count == 0)
                return; // ❗ مفيش حاجة تتعمل

            string query = $@"
        UPDATE Medicine
        SET {string.Join(", ", updates)}
        WHERE Medicine_ID = @id
    ";

            SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@id", id);

            if (stock.HasValue)
                cmd.Parameters.AddWithValue("@s", stock.Value);

            if (expiry.HasValue)
                cmd.Parameters.AddWithValue("@e", expiry.Value);

            con.Open();
            cmd.ExecuteNonQuery();
        }


        public List<Medicine> GetOutOfStock()
        {
            List<Medicine> list = new();

            string query = @"
        SELECT Medicine_ID, Name, Stock_Quantity, Price, Expiry_Date
        FROM Medicine
        WHERE Stock_Quantity = 0
    ";

            using SqlConnection con = db.GetConnection();
            using SqlCommand cmd = new(query, con);

            con.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Medicine
                {
                    Medicine_ID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Stock_Quantity = reader.GetInt32(2),
                    Price = reader.GetDecimal(3),
                    Expiry_Date = reader.GetDateTime(4)
                });
            }

            return list;
        }



    }
}
