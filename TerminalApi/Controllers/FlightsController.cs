using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TerminalApi.DataBase;
using TerminalApi.Hubs;
using TerminalApi.Models;

namespace TerminalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly ILogger<FlightsController> _logger;
        private readonly FlightsDbContext _context;
        private readonly ControlTower _ct;
        private readonly IHubContext<TerminalHub> _hubContext;
        Random rnd= new Random();
        public FlightsController(ILogger<FlightsController> logger, FlightsDbContext context, ControlTower ct, IHubContext<TerminalHub> hubContext)
        {
            _logger = logger;
            _context = context;
            _ct = ct;
            _hubContext = hubContext;
        }

        [HttpPost("/addflight")]
        public async Task<ActionResult> AddFlights([FromBody] FlightDto flightDto)
        {
            var flight = new Flight() { CreatedDate = flightDto.CreatedDate, AirLine = flightDto.AirLine, IsArrival = flightDto.IsArrival, FlightNumber = flightDto.FlightNumber };

            try
            {
                _context.AddAsync(flight);
                await _context.SaveChangesAsync(); // Save changes to generate the flight ID
                await _hubContext.Clients.All.SendAsync("flightReceived", flight); // Send flight to all connected clients
                _ct.NextLeg(flight);
                return Ok(flight);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}