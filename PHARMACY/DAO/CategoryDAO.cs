using Microsoft.Data.SqlClient;
using PHARMACY.Data;
using PHARMACY.Models;

namespace PHARMACY.DAO
{
    public class CategoryDAO
    {
        DB db = DB.Instance;

        public List<(int Id, string Name)> GetAll()
        {
            List<(int, string)> list = new();

            string q = "SELECT Category_ID, Category_Name FROM Category";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(q, con);
            con.Open();

            var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add((r.GetInt32(0), r.GetString(1)));
            }

            return list;
        }
    }
}
