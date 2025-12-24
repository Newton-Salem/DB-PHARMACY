using Microsoft.Data.SqlClient;
using PHARMACY.Data;
using PHARMACY.Model;
using System.Collections.Generic;

namespace PHARMACY.DAO
{
    public class NotificationDAO
    {
        DB db = DB.Instance;

        // Add Notification
        public void Add(int userId, string message, string type)
        {
            string query = @"
        INSERT INTO [NOTIFICATION]
        (Status, Date, Message, Type, UserID)
        VALUES ('Unread', GETDATE(), @msg, @type, @uid)
    ";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);

            cmd.Parameters.AddWithValue("@msg", message);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@uid", userId);

            con.Open();
            cmd.ExecuteNonQuery();
        }


        // Get Notifications for User
        public List<Notification> GetByUser(int userId)
        {
            List<PHARMACY.Model.Notification> list = new();

            string query = @"
                SELECT Notification_ID, Status, Date, Message, Type
                FROM [NOTIFICATION]
                WHERE UserID = @uid
                ORDER BY Date DESC
            ";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@uid", userId);

            con.Open();
            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new Notification
                {
                    NotificationID = r.GetInt32(0),
                    Status = r.GetString(1),
                    Date = r.GetDateTime(2),
                    Message = r.GetString(3),
                    Type = r.GetString(4)
                });
            }

            return list;
        }

        // Mark as Read
        public void MarkAsRead(int notificationId)
        {
            string query = @"
                UPDATE [NOTIFICATION]
                SET Status = 'Read'
                WHERE Notification_ID = @id
            ";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@id", notificationId);

            con.Open();
            cmd.ExecuteNonQuery();
        }



        // Count Unread Notifications
        public int CountUnread(int userId)
        {
            string query = @"
        SELECT COUNT(*)
        FROM [NOTIFICATION]
        WHERE UserID = @uid AND Status = 'Unread'
    ";

            using SqlConnection con = db.GetConnection();
            SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@uid", userId);

            con.Open();
            return (int)cmd.ExecuteScalar();
        }

    }
}
