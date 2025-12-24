using Microsoft.Data.SqlClient;
using PHARMACY.Data;
using PHARMACY.Pages.Admin.Reports;
using System;

namespace PHARMACY.DAO
{
    public class DashboardDAO
    {
        DB db = DB.Instance;

        public int CountOrdersByStatus(string status)
        {
            using SqlConnection con = db.GetConnection();
            con.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM [Order] WHERE Status = @status", con);

            cmd.Parameters.AddWithValue("@status", status);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public decimal GetTotalRevenue()
        {
            using SqlConnection con = db.GetConnection();
            con.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT ISNULL(SUM(Amount),0) FROM Payment WHERE Status = 'Paid'", con);

            return Convert.ToDecimal(cmd.ExecuteScalar());
        }


    //    public void AddPaymentForCompletedOrder(int orderId)
    //    {
    //        using SqlConnection con = db.GetConnection();
    //        con.Open();

    //        SqlCommand cmd = new SqlCommand(@"
    //    INSERT INTO Payment (Payment_Method, Payment_Date, Amount, Status, Order_ID)
    //    SELECT
    //        'Cash',
    //        GETDATE(),
    //        O.Total_Amount,
    //        'Paid',
    //        O.Order_ID
    //    FROM [Order] O
    //    WHERE O.Order_ID = @orderId
    //      AND O.Status = 'Completed'
    //      AND NOT EXISTS (
    //          SELECT 1 FROM Payment P
    //          WHERE P.Order_ID = O.Order_ID
    //      )
    //", con);

    //        cmd.Parameters.AddWithValue("@orderId", orderId);
    //        cmd.ExecuteNonQuery();
    //    }



        public decimal GetTotalRevenueByDate(DateTime from, DateTime to)
        {
            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(@"
        SELECT ISNULL(SUM(Total_Amount),0)
        FROM [Order]
        WHERE Status = 'Completed'
        AND ORDER_Date >= @from AND ORDER_Date < @to
    ", con);

            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);

            con.Open();
            return (decimal)cmd.ExecuteScalar();
        }


        public int CountCompletedOrdersByDate(DateTime from, DateTime to)
        {
            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(@"
        SELECT COUNT(*)
        FROM [Order]
        WHERE Status = 'Completed'
        AND ORDER_Date >= @from AND ORDER_Date < @to
    ", con);

            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);

            con.Open();
            return (int)cmd.ExecuteScalar();
        }
        public List<PharmacistSalesVM> GetSalesByPharmacist(DateTime from, DateTime to)
        {
            List<PharmacistSalesVM> list = new();

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(@"
        SELECT 
            U.Name,
            COUNT(O.Order_ID) AS OrdersCount,
            SUM(O.Total_Amount) AS TotalSales
        FROM [Order] O
        JOIN Pharmacist P ON O.PharmacistID = P.UserID
        JOIN [USER] U ON P.UserID = U.UserID
        WHERE O.Status = 'Completed'
        AND O.ORDER_Date >= @from AND O.ORDER_Date < @to
        GROUP BY U.Name
    ", con);

            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);

            con.Open();
            var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new PharmacistSalesVM
                {
                    PharmacistName = r.GetString(0),
                    OrdersCount = r.GetInt32(1),
                    TotalSales = r.GetDecimal(2)
                });
            }

            return list;
        }






        public void AddPaymentForCompletedOrder(int orderId)
        {
            using SqlConnection con = db.GetConnection();
            con.Open();

            string query = @"
        INSERT INTO Payment (Payment_Method, Payment_Date, Amount, Status, Order_ID)
        SELECT
            'Cash',
            GETDATE(),
            O.Total_Amount,
            'Paid',
            O.Order_ID
        FROM [Order] O
        WHERE O.Order_ID = @oid
          AND NOT EXISTS (
              SELECT 1 FROM Payment P WHERE P.Order_ID = O.Order_ID
          );
    ";

            SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@oid", orderId);
            cmd.ExecuteNonQuery();
        }





    }
}
