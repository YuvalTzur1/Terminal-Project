

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Timers;
using TerminalApi.Controllers;

namespace TerminalApi.Models
{
    public class Flight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int FlightNumber { get; set; }
        public string AirLine { get; set;}
        public bool IsArrival { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CurrentLeg { get; set; }

        public override string ToString()
        {
            return $" Flight number: {FlightNumber},\n AirLine: {AirLine}, \n Is arrival?: {IsArrival}, \n Created date: {CreatedDate}";
        }
    }
}
