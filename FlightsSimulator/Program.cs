
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Timers;
using TerminalApi.Controllers;
using TerminalApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

HttpClient client = new() { BaseAddress = new Uri("https://localhost:5001") };
Random rnd = new Random();
var timer = new System.Timers.Timer(4000);
timer.Enabled = true;
timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
timer.Start();

async void timer_Elapsed(object? sender, ElapsedEventArgs e)
{
    try
    {
        timer.Stop();
        var newFlight = new FlightDto();
        var response = await client.PostAsJsonAsync("/addflight", newFlight);
        var content = await response.Content.ReadAsStringAsync();
        var flightResponse = JsonConvert.DeserializeObject<Flight>(content);
        timer.Start();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
Console.ReadLine();

