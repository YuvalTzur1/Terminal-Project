using TerminalApi.Controllers;

namespace TerminalApi.Models
{
    public class FlightDto
    {
        public int FlightNumber { get; set; }
        public string AirLine { get; set; }
        public bool IsArrival { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CurrentLeg { get; set; }
        
        public FlightDto()
        {
            CreatedDate = DateTime.Now;
            string[] Airlines =
          { "America Airlines", "Delta Airlines", "EL AL", "Frontier","Korean Air",
            "Lion Airlines", "Wizzair", "United", "Turkish Airlines","Ryanair",
            "Qatar Airways", "Philippine Airlines", "EVA Air", "SunExpress"};
            Random rnd = new Random();
            FlightNumber = rnd.Next(10000, 99999);

            AirLine = Airlines[rnd.Next(0, Airlines.Length)];

            int arrival = rnd.Next(1, 3);
            if (arrival == 1)
                IsArrival = true;
            else
                IsArrival = false;
        }
    }
}
