using Microsoft.AspNetCore.Mvc.RazorPages;
// using System.Data.SqlClient;
using MySql.Data.MySqlClient;
namespace jokiso.Pages.Clients;

public class Index : PageModel
{
    public List<ClientInfo> listClients = new List<ClientInfo>();
    public void OnGet()
    {
        try
        {
            String connectionString = "Server=localhost;Port=3306;Database=jokesapp;Uid=don;Pwd=12345678;connect timeout=100;default command timeout=200;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            
            {
                connection.Open();
                String sql = "SELECT * FROM clients;";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ClientInfo clientInfo = new ClientInfo();
                            clientInfo.id = "" + reader.GetInt32(0);
                            clientInfo.name = reader.GetString(1);
                            clientInfo.email = reader.GetString(2);
                            clientInfo.phone = reader.GetString(3);
                            clientInfo.address = reader.GetString(4);
                            clientInfo.created_at = reader.GetDateTime(5).ToString();
                            
                            listClients.Add(clientInfo);
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.ToString()}");
        }
    }
}

public class ClientInfo
{
    public String id;
    public String name;
    public String email;
    public String phone;
    public String address;
    public String created_at;
}