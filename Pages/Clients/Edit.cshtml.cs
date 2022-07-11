using jokiso.Pages.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
namespace jokiso.Pages.Clients;

public class Edit : PageModel
{
    public ClientInfo clientInfo = new ClientInfo();
    public String errorMessage = "";
    public String successMessage = "";
    public void OnGet()
    {
        String id = Request.Query["id"];

        try
        {
            String connectionString = MyConstants.connectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                String sql = "SELECT * FROM clients WHERE id=@id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            clientInfo.id = "" + reader.GetInt32(0);
                            clientInfo.name = reader.GetString(1);
                            clientInfo.email = reader.GetString(2);
                            clientInfo.phone = reader.GetString(3);
                            clientInfo.address = reader.GetString(4);
                        }
                    }
                    
                    command.ExecuteNonQuery();
                }
                
            }
        }
        catch (Exception e)
        {
            errorMessage = e.Message;
        }
    }
    
    public void OnPost()
    {
        clientInfo.id = Request.Form["id"];
        clientInfo.name = Request.Form["name"];
        clientInfo.email = Request.Form["email"];
        clientInfo.phone = Request.Form["phone"];
        clientInfo.address = Request.Form["address"];
        
        if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 ||
            clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
        {
            errorMessage = "All fields are required";
            return;
        }
        
        // save to db
        try
        {
            String connectionString = MyConstants.connectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                String sql = "UPDATE clients " +
                             "SET name=@name, email=@email, phone=@phone, address=@address " +
                             "WHERE id=@id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", clientInfo.name);
                    command.Parameters.AddWithValue("@email", clientInfo.email);
                    command.Parameters.AddWithValue("@phone", clientInfo.phone);
                    command.Parameters.AddWithValue("@address", clientInfo.address);
                    command.Parameters.AddWithValue("@id", clientInfo.id);

                    command.ExecuteNonQuery();
                }
                
            }
        }
        catch (Exception e)
        {
            errorMessage = e.Message;
            return;
        }
        
        Response.Redirect("/Clients/Index");
    }
}