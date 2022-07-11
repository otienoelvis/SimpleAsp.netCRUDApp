using jokiso.Pages.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
namespace jokiso.Pages.Clients;

public class Create : PageModel
{
    public ClientInfo clientInfo = new ClientInfo();
    public String errorMessage = "";
    public String successMessage = "";
    public void OnGet()
    {
        
    }

    public void OnPost()
    {
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
        // save to database
        try
        {
            String connectionString = MyConstants.connectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                String sql = "INSERT INTO clients " +
                             "(name, email, phone, address) VALUES " +
                             "(@name, @email, @phone, @address);";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", clientInfo.name);
                    command.Parameters.AddWithValue("@email", clientInfo.email);
                    command.Parameters.AddWithValue("@phone", clientInfo.phone);
                    command.Parameters.AddWithValue("@address", clientInfo.address);

                    command.ExecuteNonQuery();
                }
                
            }
        }
        catch (Exception e)
        {
            errorMessage = e.Message;
            return;
        }
        
        
        clientInfo.name = "";
        clientInfo.email = "";
        clientInfo.phone = "";
        clientInfo.address = "";

        successMessage = "New client added successfully";
        
        // Redirect to Clients List Page
        Response.Redirect("/Clients/Index");
    }
}