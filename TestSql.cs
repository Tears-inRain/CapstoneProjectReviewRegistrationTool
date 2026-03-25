using System;
using System.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Server=localhost,1433;Database=CapstoneReviewDb;User=sa;Password=Capstone@PRN232_2026!;TrustServerCertificate=True;";
        string script = System.IO.File.ReadAllText("init_db_mock.sql");
        
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(script, conn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Success!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
