using Microsoft.Data.SqlClient;
using PHARMACY.Data;
using PHARMACY.Models;
using System.Collections.Generic;

namespace PHARMACY.DAO
{
    public class FeedbackDAO
    {
        DB db = DB.Instance;

        // ADD FEEDBACK
        public void AddFeedback(int orderId, int customerId, string message)
        {
            string query = @"
        INSERT INTO Feedback (Date, Rating, Message, CustomerID, Order_ID)
        VALUES (GETDATE(), 5, @msg, @cid, @oid)
    ";

            using SqlConnection con = db.GetConnection();
            using SqlCommand cmd = new(query, con);

            cmd.Parameters.AddWithValue("@msg", message);
            cmd.Parameters.AddWithValue("@cid", customerId);
            cmd.Parameters.AddWithValue("@oid", orderId);

            con.Open();
            cmd.ExecuteNonQuery();
        }




        // CHECK IF FEEDBACK EXISTS
        public bool FeedbackExists(int orderId, int customerId)
        {
            string query = @"
        SELECT COUNT(*) 
        FROM Feedback
        WHERE CustomerID = @cid AND Order_ID = @oid
    ";

            using SqlConnection con = db.GetConnection();
            using SqlCommand cmd = new(query, con);

            cmd.Parameters.AddWithValue("@cid", customerId);
            cmd.Parameters.AddWithValue("@oid", orderId);

            con.Open();
            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }




        // GET ALL FEEDBACKS (Pharmacist / Admin)
        public List<(int OrderID, string CustomerName, string Message)> GetAll()
        {
            List<(int, string, string)> list = new();

            string query = @"
        SELECT
            F.Order_ID,
            U.Name,
            F.Message
        FROM Feedback F
        JOIN [USER] U ON F.CustomerID = U.UserID
        ORDER BY F.Date DESC
    ";

            using SqlConnection con = db.GetConnection();
            using SqlCommand cmd = new(query, con);

            con.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add((
                    reader.GetInt32(0),   // Order_ID
                    reader.GetString(1),  // Customer Name
                    reader.GetString(2)   // Feedback Message
                ));
            }

            return list;
        }



    }
}
