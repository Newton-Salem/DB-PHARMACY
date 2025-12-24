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
        //public void Add(Medicine m)
        //{
        //    string query = @"
        //        INSERT INTO Medicine (Name, Stock_Quantity, Price, Expiry_Date)
        //        VALUES (@n, @s, @p, @e)";

        //    using SqlConnection con = db.GetConnection();
        //    SqlCommand cmd = new(query, con);

        //    cmd.Parameters.AddWithValue("@n", m.Name);
        //    cmd.Parameters.AddWithValue("@s", m.Stock_Quantity);
        //    cmd.Parameters.AddWithValue("@p", m.Price);
        //    cmd.Parameters.AddWithValue("@e", m.Expiry_Date);

        //    con.Open();
        //    cmd.ExecuteNonQuery();
        //}



        // ADD WITH CATEGORY  ✅ (جديد)
        public void AddWithCategory(Medicine m, int categoryId)
        {
            using SqlConnection con = db.GetConnection();
            con.Open();

            // 1️⃣ Add medicine
            string q1 = @"
        INSERT INTO Medicine (Name, Stock_Quantity, Price, Expiry_Date)
        VALUES (@n,@s,@p,@e);
        SELECT SCOPE_IDENTITY();";

            SqlCommand cmd1 = new(q1, con);
            cmd1.Parameters.AddWithValue("@n", m.Name);
            cmd1.Parameters.AddWithValue("@s", m.Stock_Quantity);
            cmd1.Parameters.AddWithValue("@p", m.Price);
            cmd1.Parameters.AddWithValue("@e", m.Expiry_Date);

            int medicineId = Convert.ToInt32(cmd1.ExecuteScalar());

            // 2️⃣ Link with category
            string q2 = "INSERT INTO MEDICINE_CATEGORY VALUES (@m,@c)";
            SqlCommand cmd2 = new(q2, con);
            cmd2.Parameters.AddWithValue("@m", medicineId);
            cmd2.Parameters.AddWithValue("@c", categoryId);

            cmd2.ExecuteNonQuery();
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



        public List<Medicine> GetByCategory(int categoryId)
        {
            List<Medicine> list = new();

            string q = @"
        SELECT M.Medicine_ID, M.Name, M.Price, M.Stock_Quantity, M.Expiry_Date, C.Category_Name
        FROM Medicine M
        JOIN MEDICINE_CATEGORY MC ON M.Medicine_ID = MC.Medicine_ID
        JOIN Category C ON MC.Category_ID = C.Category_ID
        WHERE C.Category_ID = @cid";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(q, con);
            cmd.Parameters.AddWithValue("@cid", categoryId);

            con.Open();
            var r = cmd.ExecuteReader();

            while (r.Read())
            {
                list.Add(new Medicine
                {
                    Medicine_ID = (int)r["Medicine_ID"],
                    Name = r["Name"].ToString()!,
                    Price = (decimal)r["Price"],
                    Stock_Quantity = (int)r["Stock_Quantity"],
                    Expiry_Date = (DateTime)r["Expiry_Date"],
                    Category_Name = r["Category_Name"].ToString()
                });
            }
            return list;
        }



    }
}
