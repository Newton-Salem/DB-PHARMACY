using Microsoft.Data.SqlClient;
using PHARMACY.Data;
using PHARMACY.Model;
using PHARMACY.Pages.Admin.Reports;
using System.Collections.Generic;

namespace PHARMACY.DAO
{
    public class ReportDAO
    {
        DB db = DB.Instance;

        // ➕ Add Report
        public void Add(string description, string type, int adminId)
        {
            string query = @"
                INSERT INTO Report (Description, GeneratedDate, ReportType, AdminID)
                VALUES (@desc, GETDATE(), @type, @aid)
            ";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);

            cmd.Parameters.AddWithValue("@desc", description);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@aid", adminId);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        // 📄 Get All Reports (Admin)
        public List<Report> GetAll()
        {
            List<Report> list = new();

            string query = @"
                SELECT 
    P.UserID,
    U.Name AS PharmacistName,
    SUM(PY.Amount) AS TotalSales
FROM Payment PY
JOIN [Order] O ON PY.Order_ID = O.Order_ID
JOIN Pharmacist P ON O.PharmacistID = P.UserID
JOIN [USER] U ON U.UserID = P.UserID
WHERE PY.Status = 'Paid'
GROUP BY P.UserID, U.Name;

            ";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);

            con.Open();
            SqlDataReader r = cmd.ExecuteReader();

            while (r.Read())
            {
                list.Add(new Report
                {
                    ReportID = r.GetInt32(0),
                    Description = r.GetString(1),
                    GeneratedDate = r.GetDateTime(2),
                    ReportType = r.GetString(3),
                    AdminName = r.GetString(4)
                });
            }

            return list;
        }

        


    }
}
