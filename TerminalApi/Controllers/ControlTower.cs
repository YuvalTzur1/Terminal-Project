using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NLog;
using System.Diagnostics.Metrics;
using TerminalApi.DataBase;
using TerminalApi.Hubs;
using TerminalApi.Models;
using LogLevel = NLog.LogLevel;

namespace TerminalApi.Controllers
{
    public class ControlTower
    {
        private static readonly Logger logger = LogManager.GetLogger("FlightLogger");
        public static event EventHandler Leg4BecameNullEvent;
        public static event EventHandler Leg6BecameNullEvent;
        public static event EventHandler Leg7BecameNullEvent;
        public static List<Flight> Leg1 { get; set; } = new List<Flight>();
        public static List<Flight> Leg2 { get; set; } = new List<Flight>();
        public static List<Flight> Leg3 { get; set; } = new List<Flight>();
        private static Flight _leg4;
        public static Flight Leg4
        {
            get { return _leg4; }
            set
            {


                _leg4 = value;
                if (_leg4 == null)
                {
                    OnLeg4BecameNullEvent();
                }



            }
        }
        protected static void OnLeg4BecameNullEvent()
        {
            Leg4BecameNullEvent?.Invoke(null, EventArgs.Empty);
        }
        private async void Leg4BecameNullHandler(object sender, EventArgs e)
        {
            if (Wait4.Count != 0)
            {
                Leg4 = Wait4.Dequeue();
                Leg4.CurrentLeg = 4;
                await UpdateFlight(Leg4);
                await _hubContext.Clients.All.SendAsync("flightMoove", Leg4, Leg4.CurrentLeg);
            }
        }
        public static List<Flight> Leg5 { get; set; } = new List<Flight>();

        private static Flight _leg6;
        public static Flight Leg6
        {
            get { return _leg6; }
            set
            {


                _leg6 = value;
                if (_leg6 == null)
                {
                    OnLeg6BecameNullEvent();
                }



            }
        }
        protected static void OnLeg6BecameNullEvent()
        {
            Leg6BecameNullEvent?.Invoke(null, EventArgs.Empty);
        }
        private async void Leg6BecameNullHandler(object sender, EventArgs e)
        {
            if (Wait67.Count != 0)
            {
                Leg6 = Wait67.Dequeue();
                Leg6.CurrentLeg = 6;
                await UpdateFlight(Leg6);
                await _hubContext.Clients.All.SendAsync("flightMoove", Leg6, Leg6.CurrentLeg);
            }

        }
        private static Flight _leg7;
        public static Flight Leg7
        {
            get { return _leg7; }
            set
            {


                _leg7 = value;
                if (_leg7 == null)
                {
                    OnLeg7BecameNullEvent();
                }



            }
        }
        protected static void OnLeg7BecameNullEvent()
        {
            Leg7BecameNullEvent?.Invoke(null, EventArgs.Empty);
        }
        private async void Leg7BecameNullHandler(object sender, EventArgs e)
        {
            if (Wait67.Count != 0)
            {
                Leg7 = Wait67.Dequeue();
                Leg7.CurrentLeg = 7;
                await UpdateFlight(Leg7);
                await _hubContext.Clients.All.SendAsync("flightMoove", Leg7, Leg7.CurrentLeg);
            }

        }
        public static List<Flight> Leg8 { get; set; } = new List<Flight>();
        public static List<Flight> Leg9 { get; set; } = new List<Flight>();
        public static Queue<Flight> Wait4 { get; set; } = new Queue<Flight>();
        public static Queue<Flight> Wait67 { get; set; } = new Queue<Flight>();

        public static Queue<Flight> Deathqueue { get; set; } = new Queue<Flight>();
        private readonly FlightsDbContext _context;
        private readonly IHubContext<TerminalHub> _hubContext;
        public ControlTower(FlightsDbContext context, IHubContext<TerminalHub> hubContext)
        {
            _hubContext = hubContext;
            _context = context;
            Leg7 = null;
            Leg6 = null;
            Leg4 = null;
            Leg4BecameNullEvent += Leg4BecameNullHandler;
            Leg6BecameNullEvent += Leg6BecameNullHandler;
            Leg7BecameNullEvent += Leg7BecameNullHandler;
        }

        private object _lock = new object();

        public async Task UpdateFlight(Flight updatedFlight)
        {
            // Retrieve the existing flight from the database
            var existingFlight = await _context.Flights!.FindAsync(updatedFlight.Id);

            if (existingFlight != null)
            {
                // Update the properties of the existing flight
                existingFlight.AirLine = updatedFlight.AirLine;
                existingFlight.CurrentLeg = updatedFlight.CurrentLeg;
                existingFlight.FlightNumber = updatedFlight.FlightNumber;
                existingFlight.CreatedDate = updatedFlight.CreatedDate;
                existingFlight.IsArrival = updatedFlight.IsArrival;
                // Save the changes to the database
                //lock (_lock)
                //{
                   _context.SaveChangesAsync();
                //}
            }
        }

        public async Task<Flight> NextLeg(Flight flight)
        {
            System.Timers.Timer timer = new System.Timers.Timer(10000);
            timer.Elapsed += async (o, s) =>
            {
                switch (flight.CurrentLeg)
                {
                    case 0:
                        if (!flight.IsArrival == true)
                        {
                            Wait67.Enqueue(flight);
                            if (Leg6 == null)
                            {
                                Leg6 = Wait67.Dequeue();
                                Leg6.CurrentLeg = 6;
                                await UpdateFlight(Leg6);
                                await _hubContext.Clients.All.SendAsync("flightMoove", Leg6, flight.CurrentLeg);
                            }
                            else if (Leg7 == null)
                            {
                                Leg7 = Wait67.Dequeue();
                                Leg7.CurrentLeg = 7;
                                await UpdateFlight(Leg7);
                                await _hubContext.Clients.All.SendAsync("flightMoove", Leg7, flight.CurrentLeg);
                            }
                        }
                        else
                        {
                            Leg1.Add(flight);
                            flight.CurrentLeg = 1;
                            await UpdateFlight(flight);
                            await _hubContext.Clients.All.SendAsync("flightMoove", flight, flight.CurrentLeg);
                        }
                        break;
                    case 1:
                        Console.WriteLine($"flight number: {flight.FlightNumber} \n current leg: {flight.CurrentLeg}");
                        Leg1.Remove(flight);
                        Leg2.Add(flight);
                        flight.CurrentLeg = 2;
                        await UpdateFlight(flight);
                        await _hubContext.Clients.All.SendAsync("flightMoove", flight, flight.CurrentLeg);
                        break;

                    case 2:
                        Console.WriteLine($"flight number: {flight.FlightNumber} \n current leg: {flight.CurrentLeg}");
                        Leg2.Remove(flight);
                        Leg3.Add(flight);
                        Wait4.Enqueue(flight);
                        flight.CurrentLeg = 3;
                        await UpdateFlight(flight);
                        await _hubContext.Clients.All.SendAsync("flightMoove", flight, flight.CurrentLeg);
                        break;

                    case 3:
                        Console.WriteLine($"flight number: {flight.FlightNumber} \n current leg: {flight.CurrentLeg}");
                        if (Leg4 == null)
                        {
                            Leg4 = null;
                        }
                        break;

                    case 4:
                        Console.WriteLine($"flight number: {flight.FlightNumber} \n current leg: {flight.CurrentLeg}");
                        if (flight.IsArrival)
                        {
                            Leg5.Add(flight);
                            flight.CurrentLeg = 5;
                            Wait67.Enqueue(flight);
                        }
                        else
                        {
                            flight.CurrentLeg = 9;
                            Leg9.Add(flight);
                        }
                        await UpdateFlight(flight);
                        await _hubContext.Clients.All.SendAsync("flightMoove", flight, flight.CurrentLeg);
                        Leg4 = null!;

                        break;

                    case 5:
                        Console.WriteLine($"flight number: {flight.FlightNumber} \n current leg: {flight.CurrentLeg}");
                        if(Leg6 == null)
                        {
                            Leg6 = null;
                            break;
                        }
                        else if(Leg7== null)
                        {
                            Leg7 = null;
                            break;
                        }
                        break;
                       
                    case 6:
                        Console.WriteLine($"flight number: {flight.FlightNumber} \n current leg: {flight.CurrentLeg}");
                        if (flight.IsArrival)
                        {
                            Leg6 = null!;
                            Deathqueue.Enqueue(flight);
                            timer.Stop();
                            _context.Flights!.Remove(flight);
                            await _hubContext.Clients.All.SendAsync("flightRemove", flight.Id);
                            var logEvent2 = new LogEventInfo(LogLevel.Info, logger.Name, "Flight landed");
                            logEvent2.Properties["Object"] = flight;
                            logger.Log(logEvent2);
                        }
                        else
                        {
                            Leg8.Add(flight);
                            Leg6 = null!;
                            Wait4.Enqueue(flight);
                            flight.CurrentLeg = 8;
                            await UpdateFlight(flight);
                            await _hubContext.Clients.All.SendAsync("flightMoove", flight, flight.CurrentLeg);
                        }

                        break;

                    case 7:
                        Console.WriteLine($"flight number: {flight.FlightNumber} \n current leg: {flight.CurrentLeg}");
                        if (flight.IsArrival)
                        {
                            Leg7 = null!;
                            Deathqueue.Enqueue(flight);
                            timer.Stop();
                            _context.Flights!.Remove(flight);
                            await _hubContext.Clients.All.SendAsync("flightRemove", flight.Id);
                            var logEvent1 = new LogEventInfo(LogLevel.Info, logger.Name, "Flight landed");
                            logEvent1.Properties["Object"] = flight;
                            logger.Log(logEvent1);
                        }
                        else
                        {
                            Leg8.Add(flight);
                            Leg7 = null!;
                            Wait4.Enqueue(flight);
                            flight.CurrentLeg = 8;
                            await UpdateFlight(flight);
                            await _hubContext.Clients.All.SendAsync("flightMoove", flight, flight.CurrentLeg);
                        }

                        break;

                    case 8:
                        Console.WriteLine($"flight number: {flight.FlightNumber} \n current leg: {flight.CurrentLeg}");
                        if (Leg4 == null)
                        {
                            Leg4 = null!;
                        }
                        await _hubContext.Clients.All.SendAsync("flightMoove", flight, flight.CurrentLeg);
                        break;

                    case 9:
                        Console.WriteLine($"flight number: {flight.FlightNumber} \n current leg: {flight.CurrentLeg}");
                        Leg9.Remove(flight);
                        Deathqueue.Enqueue(flight);
                        timer.Stop();
                        _context.Flights!.Remove(flight);
                        await _hubContext.Clients.All.SendAsync("flightRemove", flight.Id);
                        var logEvent = new LogEventInfo(LogLevel.Info, logger.Name, flight.IsArrival ? "Flight landed \n" : "flight taken off \n");
                        logEvent.Properties["Object"] = flight;
                        logger.Log(logEvent);
                        break;
                }
            };
            timer.Start();
            return flight;

        }
    }
}
