using PHARMACY.Model;
using PHARMACY.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PHARMACY.DAO
{
    public class CommonDAO
    {
        private readonly string cs =
            "Data Source=.;Initial Catalog=PROJECT;Integrated Security=True";

        //Get all suppliers
        public List<SupplierModel> GetSuppliers()
        {
            var list = new List<SupplierModel>();

            using SqlConnection con = new(cs);
            con.Open();

            string query = @"
                SELECT U.UserID, S.Company_Name
                FROM Supplier S
                JOIN [USER] U ON S.UserID = U.UserID
            ";

            SqlCommand cmd = new(query, con);
            SqlDataReader r = cmd.ExecuteReader();

            while (r.Read())
            {
                list.Add(new SupplierModel
                {
                    UserID = r.GetInt32(0),
                    CompanyName = r.GetString(1)
                });
            }

            return list;
        }

        //Get all medicines
        public List<Medicine> GetMedicines()
        {
            var list = new List<Medicine>();

            using SqlConnection con = new(cs);
            con.Open();

            SqlCommand cmd = new(
                "SELECT Medicine_ID, Name FROM Medicine", con);

            SqlDataReader r = cmd.ExecuteReader();

            while (r.Read())
            {
                list.Add(new Medicine
                {
                    Medicine_ID = r.GetInt32(0),
                    Name = r.GetString(1)
                });
            }

            return list;
        }
    }
}
