using Microsoft.Data.SqlClient;
using PHARMACY.Data;
using PHARMACY.Model;
using System.Collections.Generic;

namespace PHARMACY.DAO
{
    public class OrderDAO
    {
        DB db = DB.Instance;

        // ================= Add Order (Customer) =================




        public void AddOrder(Order order, int medicineId)
        {
            using SqlConnection con = db.GetConnection();
            con.Open();

           
            if (order.PharmacistID <= 0)
            {
                order.PharmacistID = GetAnyPharmacistId(con);
            }

            // Insert Order
            string orderQuery = @"
        INSERT INTO [Order]
        (Status, ORDER_Date, Total_Amount, CustomerID, PharmacistID)
        VALUES
        (@status, GETDATE(), @total, @cid, @pid);

        SELECT CAST(SCOPE_IDENTITY() AS INT);
    ";

            SqlCommand orderCmd = new(orderQuery, con);
            orderCmd.Parameters.AddWithValue("@status", order.Status);
            orderCmd.Parameters.AddWithValue("@total", order.Total);
            orderCmd.Parameters.AddWithValue("@cid", order.CustomerID);
            orderCmd.Parameters.AddWithValue("@pid", order.PharmacistID);

            object result = orderCmd.ExecuteScalar();
            if (result == null)
                throw new Exception("Failed to create order");

            int orderId = (int)result;

            // Insert Order_Medicine
            string omQuery = @"
        INSERT INTO Order_Medicine (Order_ID, Medicine_ID, Quantity)
        VALUES (@oid, @mid, @qty)
    ";

            SqlCommand omCmd = new(omQuery, con);
            omCmd.Parameters.AddWithValue("@oid", orderId);
            omCmd.Parameters.AddWithValue("@mid", medicineId);
            omCmd.Parameters.AddWithValue("@qty", order.Quantity);
            omCmd.ExecuteNonQuery();
            Console.WriteLine("PHARMACIST ID = " + order.PharmacistID);
            //  Notification للصيدلي
            NotificationDAO notificationDAO = new NotificationDAO();
            notificationDAO.Add(
                order.PharmacistID,
                $"New order from customer #{order.CustomerID}",
                "Order"
            );
        }




        // ================= Customer Orders =================
        public List<Order> GetOrdersByCustomer(int customerId)
        {
            List<Order> orders = new();

            string query = @"
        SELECT 
            O.Order_ID,
            M.Name AS MedicineName,
            OM.Quantity,
            O.Total_Amount,
            O.Status,
            O.ORDER_Date
        FROM [Order] O
        JOIN Order_Medicine OM ON O.Order_ID = OM.Order_ID
        JOIN Medicine M ON OM.Medicine_ID = M.Medicine_ID
        WHERE O.CustomerID = @cid
    ";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@cid", customerId);

            con.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                orders.Add(new Order
                {
                    OrderID = reader.GetInt32(0),
                    MedicineName = reader.GetString(1),
                    Quantity = reader.GetInt32(2),
                    Total = reader.GetDecimal(3),
                    Status = reader.GetString(4),
                    OrderDate = reader.GetDateTime(5)
                });
            }

            return orders;
        }





        // ================= Pharmacist (Pending Orders) =================
        public List<Order> GetPendingOrdersForPharmacist()
        {
            List<Order> orders = new();

            string query = @"
        SELECT 
            O.Order_ID,
            M.Name AS MedicineName,
            OM.Quantity,
            O.Total_Amount,
            O.Status,
            O.ORDER_Date
        FROM [Order] O
        JOIN Order_Medicine OM ON O.Order_ID = OM.Order_ID
        JOIN Medicine M ON OM.Medicine_ID = M.Medicine_ID
        WHERE O.Status = 'Pending'
        ORDER BY O.ORDER_Date
    ";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);

            con.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                orders.Add(new Order
                {
                    OrderID = reader.GetInt32(0),
                    MedicineName = reader.GetString(1),
                    Quantity = reader.GetInt32(2),
                    Total = reader.GetDecimal(3),
                    Status = reader.GetString(4),
                    OrderDate = reader.GetDateTime(5)



                });
            }

            return orders;
        }

        // ================= Pharmacist Actions =================
        public void ApproveOrder(int orderId, int pharmacistId)
        {
            // حدّث حالة الأوردر + الصيدلي اللي وافق
            UpdateStatusAndPharmacist(orderId, "Completed", pharmacistId);

            //أضف Payment مربوط بالصيدلي ده
            DashboardDAO dashboardDAO = new DashboardDAO();
            dashboardDAO.AddPaymentForCompletedOrder(orderId);

            // Notification للعميل
            int customerId = GetCustomerIdByOrder(orderId);

            NotificationDAO notificationDAO = new NotificationDAO();
            notificationDAO.Add(
                customerId,
                $"Your order #{orderId} has been approved",
                "Order"
            );
        }




        public void CancelOrderPharmacist(int orderId,int pharmacistId)
        {
            UpdateStatusAndPharmacist( orderId,"Cancelled",  pharmacistId);

            int customerId = GetCustomerIdByOrder(orderId);

            NotificationDAO notificationDAO = new NotificationDAO();
            notificationDAO.Add(
                customerId,
                $"Your order #{orderId} has been cancelled",
                "Order"
            );
        }

        // ================= Customer Cancel =================
        public void CancelOrder(int orderId)
        {
            string query = @"
                UPDATE dbo.[Order]
                SET Status = 'Cancelled'
                WHERE Order_ID = @id AND Status = 'Pending'
            ";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@id", orderId);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        // ================= Shared =================
        
        private void UpdateStatusAndPharmacist(int orderId, string status, int pharmacistId)
        {
            string query = @"
        UPDATE [Order]
        SET Status = @status,
            PharmacistID = @pharmacistId
        WHERE Order_ID = @orderId
    ";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);

            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@pharmacistId", pharmacistId);
            cmd.Parameters.AddWithValue("@orderId", orderId);

            con.Open();
            cmd.ExecuteNonQuery();
        }




        // ================= Supplier Order =================
        public void AddSupplierOrder(Order order)
        {
            string query = @"
        INSERT INTO [Order]
        (Status, ORDER_Date, Total_Amount, PharmacistID)
        VALUES
        (@status, GETDATE(), @total, @pid)
    ";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);

            cmd.Parameters.AddWithValue("@status", order.Status);
            cmd.Parameters.AddWithValue("@total", order.Total);
            cmd.Parameters.AddWithValue("@pid", order.PharmacistID);

            con.Open();
            cmd.ExecuteNonQuery();
        }


        // ================= Admin Order =================

        public List<Order> GetAllOrdersForAdmin()
        {
            List<Order> orders = new();

            string query = @"
        SELECT 
            O.Order_ID,
            U.Name AS CustomerName,
            O.Total_Amount,
            O.Status
        FROM [Order] O
        LEFT JOIN Customer C ON O.CustomerID = C.UserID
        LEFT JOIN [USER] U ON C.UserID = U.UserID
        ORDER BY O.Order_ID DESC
    ";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);

            con.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                orders.Add(new Order
                {
                    OrderID = reader.GetInt32(0),
                    CustomerName = reader.IsDBNull(1) ? "Supplier Order" : reader.GetString(1),
                    Total = reader.GetDecimal(2),
                    Status = reader.GetString(3)
                });
            }

            return orders;
        }


        public void DeleteOrder(int orderId)
        {
            using SqlConnection con = db.GetConnection();
            con.Open();

            // Feedback
            SqlCommand cmdFeedback = new(
                "DELETE FROM Feedback WHERE Order_ID = @id", con);
            cmdFeedback.Parameters.AddWithValue("@id", orderId);
            cmdFeedback.ExecuteNonQuery();

            //  Payment
            SqlCommand cmdPayment = new(
                "DELETE FROM [Payment] WHERE Order_ID = @id", con);
            cmdPayment.Parameters.AddWithValue("@id", orderId);
            cmdPayment.ExecuteNonQuery();

            // Order_Medicine
            SqlCommand cmdOM = new(
                "DELETE FROM Order_Medicine WHERE Order_ID = @id", con);
            cmdOM.Parameters.AddWithValue("@id", orderId);
            cmdOM.ExecuteNonQuery();

            // Notification
            SqlCommand cmdNotif = new(
                "DELETE FROM [NOTIFICATION] WHERE Message LIKE @msg", con);
            cmdNotif.Parameters.AddWithValue("@msg", "%" + orderId + "%");
            cmdNotif.ExecuteNonQuery();

            //  Order
            SqlCommand cmdOrder = new(
                "DELETE FROM [Order] WHERE Order_ID = @id", con);
            cmdOrder.Parameters.AddWithValue("@id", orderId);
            cmdOrder.ExecuteNonQuery();

        }




        public List<Order> GetOrdersForSupplier(int supplierId)
        {
            List<Order> orders = new();

            string query = @"
        SELECT 
    O.Order_ID,
    M.Name,
    OM.Quantity,
    O.Total_Amount,
    O.Status,
    O.ORDER_Date
FROM [Order] O
JOIN Order_Medicine OM ON O.Order_ID = OM.Order_ID
JOIN Medicine M ON OM.Medicine_ID = M.Medicine_ID
WHERE O.PharmacistID = @sid

    ";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@sid", supplierId);

            con.Open();
            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                orders.Add(new Order
                {
                    OrderID = r.GetInt32(0),
                    MedicineName = r.GetString(1),
                    Quantity = r.GetInt32(2),
                    Total = r.GetDecimal(3),
                    Status = r.GetString(4),
                    OrderDate = r.GetDateTime(5)
                });

            }

            return orders;
        }




        private int GetCustomerIdByOrder(int orderId)
        {
            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(
                "SELECT CustomerID FROM [Order] WHERE Order_ID = @id", con);

            cmd.Parameters.AddWithValue("@id", orderId);
            con.Open();

            return (int)cmd.ExecuteScalar();
        }

        private int GetAnyPharmacistId(SqlConnection con)
        {
            SqlCommand cmd = new("SELECT TOP 1 UserID FROM Pharmacist", con);
            return (int)cmd.ExecuteScalar();
        }



        



    }
}
