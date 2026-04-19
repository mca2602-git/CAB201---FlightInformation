namespace A2_CAB201
{
/// <summary>
/// The purpose of this class is to store all flight related information
/// </summary>
public class Flights
    {
        public string Airline { get; set; }
        public int Flight_ID { get; set; }
        public int Plane_ID { get; set; }
        public DateTime time { get; set; }
        public string City { get; set; }
        public string Type { get; set; } 

        public Flights(string airline, int flight_ID, int plane_ID, DateTime time, string city, string type)
        {
            this.Airline = airline;
            this.Flight_ID = flight_ID;
            this.Plane_ID = plane_ID;
            this.time = time;
            this.City = city;
            this.Type = type;
        }
        public static void DisplayFlights(List<Flights> flights_available)
        {
            cmdlineUI.DisplayString("Arrival Flights:");
            var arrivalFlights = flights_available
                .Where(flight => flight.Type == FlightConst.ARRIVALTYPE)
                .OrderBy(f => f.time)
                .ToList();
            if (arrivalFlights.Count > 0)
            {
                foreach (var flight in arrivalFlights)
                {
                    cmdlineUI.DisplayAllFlights(flight.Airline, flight.Flight_ID, flight.Plane_ID, flight.time, flight.Type, flight.City);
                }
            }
            else
            {
                cmdlineUI.DisplayString("There are no arrival flights.");
            }
            cmdlineUI.DisplayString("Departure Flights:");
            var departureFlights = flights_available
                .Where(flight => flight.Type == FlightConst.DEPARTURETYPE)
                .OrderBy(f => f.time)
                .ToList();
            if (departureFlights.Count > 0)
            {
                foreach (var flight in departureFlights)
                {
                    cmdlineUI.DisplayAllFlights(flight.Airline, flight.Flight_ID, flight.Plane_ID, flight.time, flight.Type, flight.City);
                }
            }
            else
            {
                cmdlineUI.DisplayString("There are no departure flights.");
            }
        }
    }
}

/// <summary>
/// creates a seperate table to allow for users to have unique bookings as no email is the same
/// </summary>
public class booked_flights
{
    public string booked_Airline { get; set; }
    public string booked_City { get; set; }
    public string booked_Time { get; set; }
    public int booked_Seat { get; set; }
    public string booked_Row { get; set; }
    public string booked_Type { get; set; }
    public string booked_Email { get; set; }


    public booked_flights(string booked_Airline, string booked_City, string booked_Time, int booked_Seat, string booked_Row, string booked_Type, string booked_Email)
    {
        this.booked_Airline = booked_Airline;
        this.booked_City = booked_City;
        this.booked_Time = booked_Time;
        this.booked_Seat = booked_Seat;
        this.booked_Row = booked_Row;
        this.booked_Type = booked_Type;
        this.booked_Email = booked_Email;
    }
}
