using Microsoft.AspNetCore.SignalR;
using TerminalApi.Models;

namespace TerminalApi.Hubs
{
    public class TerminalHub : Hub
    {
        //public async Task NewFlight(Flight flight)
        //{
        //    await Clients.All.SendAsync("flightReceived", flight);
        //}
        //public async Task RemoveFlight(Flight flight)
        //{
        //    await Clients.All.SendAsync("flightRemove", flight.Id);
        //}
        //public async Task mooveFlight(Flight flight)
        //{
        //    await Clients.All.SendAsync("flightMoove", flight.Id);
        //}
    }
}
