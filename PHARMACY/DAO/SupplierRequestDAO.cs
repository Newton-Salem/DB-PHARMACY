using System.Data.SqlClient;
using PHARMACY.Model;
using System.Collections.Generic;

namespace PHARMACY.DAO
{
    public class SupplierRequestDAO
    {
        private readonly string cs =
            "Data Source=.;Initial Catalog=PROJECT;Integrated Security=True";

        public List<SupplierRequest> GetAll()
        {
            var list = new List<SupplierRequest>();

            using SqlConnection con = new(cs);
            con.Open();

            string query = @"
        SELECT 
            SR.Request_id,
            U.Name AS PharmacistName,
            M.Name AS MedicineName,
            SR.Quantity,
            SR.Request_Date,
            SR.Status
        FROM Supplier_Request SR
        JOIN Pharmacist P ON SR.PharmacistID = P.UserID
        JOIN [USER] U ON P.UserID = U.UserID
        JOIN SUPPLIER_REQUEST_MEDICINE SRM ON SR.Request_id = SRM.Request_ID
        JOIN Medicine M ON SRM.Medicine_ID = M.Medicine_ID
        ORDER BY SR.Request_Date DESC
    ";

            SqlCommand cmd = new(query, con);
            SqlDataReader r = cmd.ExecuteReader();

            while (r.Read())
            {
                list.Add(new SupplierRequest
                {
                    RequestId = r.GetInt32(0),
                    PharmacistName = r.GetString(1),
                    MedicineName = r.GetString(2),
                    Quantity = r.GetInt32(3),
                    RequestDate = r.GetDateTime(4),
                    Status = r.GetString(5)
                });
            }

            return list;
        }


        public List<SupplierRequest> GetRequestsForSupplier(int supplierId)
        {
            var list = new List<SupplierRequest>();

            using SqlConnection con = new(cs);
            con.Open();

            string query = @"
        SELECT 
            SR.Request_id,
            U.Name AS PharmacistName,
            M.Name AS MedicineName,
            SR.Quantity,
            SR.Request_Date,
            SR.Status
        FROM Supplier_Request SR
        JOIN Pharmacist P ON SR.PharmacistID = P.UserID
        JOIN [USER] U ON P.UserID = U.UserID
        JOIN SUPPLIER_REQUEST_MEDICINE SRM ON SR.Request_id = SRM.Request_ID
        JOIN Medicine M ON SRM.Medicine_ID = M.Medicine_ID
        WHERE SR.SupplierID = @supplierId
        ORDER BY SR.Request_Date DESC
    ";

            SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@supplierId", supplierId);

            SqlDataReader r = cmd.ExecuteReader();

            while (r.Read())
            {
                list.Add(new SupplierRequest
                {
                    RequestId = r.GetInt32(0),
                    PharmacistName = r.GetString(1),
                    MedicineName = r.GetString(2),
                    Quantity = r.GetInt32(3),
                    RequestDate = r.GetDateTime(4),
                    Status = r.GetString(5)
                });
            }

            return list;
        }


        public void UpdateStatus(int requestId, string status)
        {
            using SqlConnection con = new(cs);
            con.Open();

            SqlCommand cmd = new(
                "UPDATE Supplier_Request SET Status = @status WHERE Request_id = @id",
                con);

            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@id", requestId);

            cmd.ExecuteNonQuery();
        }





        // Delete supplier order (Admin)
        public void Delete(int requestId)
        {
            using SqlConnection con = new(cs);
            con.Open();

            //  امسح الربط مع الدوا
            SqlCommand cmdSRM = new(
                "DELETE FROM SUPPLIER_REQUEST_MEDICINE WHERE Request_ID = @id", con);
            cmdSRM.Parameters.AddWithValue("@id", requestId);
            cmdSRM.ExecuteNonQuery();

            // امسح الطلب نفسه
            SqlCommand cmdReq = new(
                "DELETE FROM Supplier_Request WHERE Request_id = @id", con);
            cmdReq.Parameters.AddWithValue("@id", requestId);
            cmdReq.ExecuteNonQuery();
        }


        //  Add new order
        public void Add(int supplierId, int pharmacistId, int medicineId, int quantity, DateTime requestDate)
        {
            using SqlConnection con = new(cs);
            con.Open();

            //  Insert Supplier_Request
            string insertRequest = @"
        INSERT INTO Supplier_Request
        (Request_Date, Status, Quantity, SupplierID, PharmacistID)
        VALUES (@date, 'Pending', @q, @s, @p);

        SELECT CAST(SCOPE_IDENTITY() AS INT);
    ";

            SqlCommand cmd = new(insertRequest, con);
            cmd.Parameters.AddWithValue("@date", requestDate);
            cmd.Parameters.AddWithValue("@q", quantity);
            cmd.Parameters.AddWithValue("@s", supplierId);
            cmd.Parameters.AddWithValue("@p", pharmacistId);

            int requestId = (int)cmd.ExecuteScalar();

            //  Link Medicine
            string linkMedicine = @"
        INSERT INTO SUPPLIER_REQUEST_MEDICINE
        (Request_ID, Medicine_ID)
        VALUES (@r, @m)
    ";

            SqlCommand cmd2 = new(linkMedicine, con);
            cmd2.Parameters.AddWithValue("@r", requestId);
            cmd2.Parameters.AddWithValue("@m", medicineId);
            cmd2.ExecuteNonQuery();

            // 3️⃣ Notification للمورّد
            NotificationDAO notificationDAO = new NotificationDAO();
            notificationDAO.Add(
                supplierId,
                $"New supply request #{requestId} from pharmacist #{pharmacistId}",
                "SupplierRequest"
            );
            Console.WriteLine("NOTIFICATION SENT TO SUPPLIER");
            Console.WriteLine("SUPPLIER REQUEST ADDED");
            Console.WriteLine($"SupplierID = {supplierId}");

        }



        // Cancel order
        public void Cancel(int requestId)
        {
            using SqlConnection con = new(cs);
            con.Open();

            SqlCommand cmd = new(
                "UPDATE Supplier_Request SET Status='Cancelled' WHERE Request_id=@id",
                con);

            cmd.Parameters.AddWithValue("@id", requestId);
            cmd.ExecuteNonQuery();
        }




    }


}
