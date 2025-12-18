using System.Data.SqlClient;
using PHARMACY.Model;
using System.Collections.Generic;

namespace PHARMACY.DAO
{
    public class SupplierOrderDAO
    {
        private readonly string cs =
            "Data Source=.;Initial Catalog=PROJECT;Integrated Security=True";

        // 🔹 Get all supplier orders
        public List<SupplierOrder> GetAll()
        {
            var list = new List<SupplierOrder>();

            using SqlConnection con = new(cs);
            con.Open();

            string query = @"
                SELECT 
                    SR.Request_id,
                    S.Company_Name,
                    M.Name,
                    SR.Quantity,
                    SR.Request_Date,
                    SR.Status
                FROM Supplier_Request SR
                JOIN Supplier S ON SR.SupplierID = S.UserID
                JOIN SUPPLIER_REQUEST_MEDICINE SRM ON SR.Request_id = SRM.Request_ID
                JOIN Medicine M ON SRM.Medicine_ID = M.Medicine_ID
            ";

            SqlCommand cmd = new(query, con);
            SqlDataReader r = cmd.ExecuteReader();

            while (r.Read())
            {
                list.Add(new SupplierOrder
                {
                    RequestId = r.GetInt32(0),
                    SupplierName = r.GetString(1),
                    MedicineName = r.GetString(2),
                    Quantity = r.GetInt32(3),
                    RequestDate = r.GetDateTime(4),
                    Status = r.GetString(5)
                });
            }

            return list;
        }

        // 🔹 Add new order
        public void Add(int supplierId, int pharmacistId, int medicineId, int quantity)
        {
            using SqlConnection con = new(cs);
            con.Open();

            string insertRequest = @"
                INSERT INTO Supplier_Request
                (Request_Date, Status, Quantity, supplierID, PharmacistID)
                VALUES (GETDATE(), 'Pending', @q, @s, @p);

                SELECT SCOPE_IDENTITY();
            ";

            SqlCommand cmd = new(insertRequest, con);
            cmd.Parameters.AddWithValue("@q", quantity);
            cmd.Parameters.AddWithValue("@s", supplierId);
            cmd.Parameters.AddWithValue("@p", pharmacistId);

            int requestId = Convert.ToInt32(cmd.ExecuteScalar());

            string linkMedicine = @"
                INSERT INTO SUPPLIER_REQUEST_MEDICINE
                (Request_ID, Medicine_ID)
                VALUES (@r, @m)
            ";

            SqlCommand cmd2 = new(linkMedicine, con);
            cmd2.Parameters.AddWithValue("@r", requestId);
            cmd2.Parameters.AddWithValue("@m", medicineId);
            cmd2.ExecuteNonQuery();
        }

        // 🔹 Cancel order
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
