using System.Text.RegularExpressions;
namespace A2_CAB201
{
    /// <summary>
    /// to seperate all UNIQUE interaction in the log in menu, inherits registration as it is so vastly different from traveller and ff
    /// </summary>
    public class flightManager : Registration
    {
        /// <summary>
        /// adds a new variable to the list unique to the FLM (flight manager)
        /// </summary>
        public int Staff_ID { get; set; }

        /// <summary>
        /// Overrides the registrations display details to show staff ID
        /// </summary>
        public override void DisplayDetails()
        {
            base.DisplayDetails(); // Show base info
            Console.WriteLine($"Staff ID: {Staff_ID}");
        }

        /// <summary>
        /// Shows the various choices for the FLM, allows them to create, delay and view ALL FLIGHTS
        /// </summary>
        /// <param name="choice">whatever choice they make turns into a case</param>
        /// <param name="matched_reg">all existing registrations</param>
        /// <param name="controller">allows access to the flight controller</param>
        /// <param name="flights">all existing flights</param>
        /// <param name="bookedFlightsList">all booked flights</param>
        public override void LoginChoices(int choice, Registration matched_reg, flightController controller, List<Flights> flights, List<booked_flights> bookedFlightsList)
        {
            switch (choice)
            {
                case 0:
                case 1:
                    base.LoginChoices(choice, matched_reg, controller, flights, bookedFlightsList);
                    break;
                case 2:
                    CreateFlight(FlightConst.ARRIVALTYPE, flights, controller);
                    break; 
                case 3:
                    CreateFlight(FlightConst.DEPARTURETYPE, flights, controller);
                    break;
                case 4:
                    DelayFlight(FlightConst.ARRIVALTYPE, flights, bookedFlightsList);
                    break;
                case 5:
                    DelayFlight(FlightConst.DEPARTURETYPE, flights, bookedFlightsList);
                    break;
                case 6:
                    Flights.DisplayFlights(flights);
                    // see details all flights
                    break;
                default:
                    break;
            }
            }
        /// <summary>
        /// creates a unique list of the FLM
        /// </summary>
        /// ALL OTHER PARAMS HAVE BEEN DEFINED
        /// <param name="type">The type of user</param>
        /// <param name="name">The name of the user</param>
        /// <param name="age">The age of the user</param>
        /// <param name="mobile">the phone number of the user</param>
        /// <param name="email">The unique email of the user</param>
        /// <param name="password">the password of the user</param>
        /// <param name="staff_ID">The unique ID of the user</param>
        public flightManager(string type, string name, int age, string mobile, string email, string password, int staff_ID)
            : base("FLM", name, age, mobile, email, password)
        {
            this.Type = type;
            this.Name = name;
            this.Age = age;
            this.Mobile = mobile;
            this.Email = email;
            this.Password = password;
            this.Staff_ID = staff_ID;
        }


        /// <summary>
        /// allows FLM to create arrival and departure flights in one method
        /// </summary>
        /// <param name="flightType">The type of flight it is (arrival or departure)</param>
        /// <param name="flights">All existing flights</param>
        /// <param name="controller">The control class</param>
        private void CreateFlight(string flightType, List<Flights> flights, flightController controller)
        {
            const string JET_CHO = "Jetstar";
            const string QAN_CHO = "Qantas";
            const string REG_CHO = "Regional Express";
            const string VIR_CHO = "Virgin";
            const string FLY_CHO = "Fly Pelican";
            List<string> choices = new List<string>();
            choices.Add(JET_CHO);
            choices.Add(QAN_CHO);
            choices.Add(REG_CHO);
            choices.Add(VIR_CHO);
            choices.Add(FLY_CHO);
            const int JET_int = 0, QAN_int = 1, REG_int = 2, VIR_int = 3, FLY_int = 4;
            string airline;
            int choice2 = flightMenu.showChoices("Please enter the airline:", choices);
            switch (choice2)
            {
                case JET_int:
                    airline = "JST";
                    break;
                case QAN_int:
                    airline = "QFA";
                    break;
                case REG_int:
                    airline = "RXA";
                    break;
                case VIR_int:
                    airline = "VOZ";
                    break;
                case FLY_int:
                    airline = "FRE";
                    break;
                default:
                    airline = "";
                    break;
            }
            controller.PROCESS_create_flight(airline, flightType, flights);
        }
        /// <summary>
        /// allows FLM to delay arrival and departure flights in one method
        /// </summary>
        /// <param name="flightType">The type of flight it is (arrival or departure)</param>
        /// <param name="flights">All existing flights</param>
        /// <param name="bookedFlightsList">All existing bookings</param>
        private void DelayFlight(string flightType, List<Flights> flights, List<booked_flights> bookedFlightsList)
        {
            var booked_flight = bookedFlightsList
                    .Where(bookedFlightsList => bookedFlightsList.booked_Type == flightType)
                    .OrderBy(f => f.booked_Time)
                    .ToList();
            if (booked_flight.Count > 0)
            {
                string choice;
                List<string> choices = new List<string>();
                choices.Clear();
                string FLIGHT_str = $"Please enter the {(flightType == FlightConst.ARRIVALTYPE ? "arrival" : "departure")} flight:";
                var nonbooked_flights = flights
                .Where(flight => flight.Type == flightType)
                .OrderBy(f => f.time)
                .ToList();
                string format = FlightConst.DATETIMEFORMAT;
                foreach (var flight in nonbooked_flights)
                {
                    string airlineKey = flight.Airline.ToUpper().Trim();
                    string full_airline = FlightConst.AirlineNames.TryGetValue(flight.Airline, out var name)
                    ? name
                    : "Unknown Airline";
                    string flightInfo = flightType == FlightConst.ARRIVALTYPE
                    ? $"Flight {flight.Airline}{flight.Flight_ID} operated by {full_airline} arriving at {flight.time.ToString(format)} from {flight.City} on plane {flight.Airline}{flight.Plane_ID}{flight.Type}."
                    : $"Flight {flight.Airline}{flight.Flight_ID} operated by {full_airline} departing at {flight.time.ToString(format)} to {flight.City} on plane {flight.Airline}{flight.Plane_ID}{flight.Type}.";
                    choices.Add(flightInfo);

                }
                cmdlineUI.DisplayString(FLIGHT_str);
                for (int i = 0; i < choices.Count; i++)
                {
                    cmdlineUI.DisplayString($"{i + 1}. {choices[i]}");
                }
                do
                {
                    choice = cmdlineUI.InputString($"Please enter a choice between 1 and {choices.Count()}:");
                } while (!Validation.validRange(choice, choices));
                int choice4_int = int.Parse(choice) - 1;
                string chosen = choices[choice4_int];

                string pattern = flightType == FlightConst.ARRIVALTYPE
                ? @"Flight (?<flightCode>\w+) operated by (?<fullAirline>.+?) arriving at (?<time>.+?) from (?<city>.+?) on plane (?<planeId>.+?)\."
                : @"Flight (?<flightCode>\w+) operated by (?<fullAirline>.+?) departing at (?<time>.+?) to (?<city>.+?) on plane (?<planeId>.+?)\.";
                Match m = Regex.Match(chosen, pattern);
                string fullAirline = m.Groups["fullAirline"].Value;
                string flight_code = m.Groups["flightCode"].Value;
                string time = m.Groups["time"].Value;
                string format_3 = FlightConst.DATETIMEFORMAT;
                DateTime time_conv = DateTime.ParseExact(time, format_3, System.Globalization.CultureInfo.InvariantCulture);
                // HH:mm dd/MM/yyyy
                cmdlineUI.DisplayRegister("minutes delayed");
                int delay = cmdlineUI.InputInt();
                DateTime delayed_time = time_conv.AddMinutes(delay);
                var flightUpdate = flights.FirstOrDefault(f => (f.Airline + f.Flight_ID) == flight_code);
                flightUpdate.time = delayed_time;
                var bookedflightUpdate = bookedFlightsList.FirstOrDefault(f => (f.booked_Airline) == flight_code);
                if (bookedflightUpdate != null)
                {
                    bookedflightUpdate.booked_Time = delayed_time.ToString(format_3);
                }
            }
            else
            {
                if (flightType == FlightConst.ARRIVALTYPE)
                {
                    cmdlineUI.DisplayString("The airport does not have any arrival flights.");
                }
                else
                {
                    cmdlineUI.DisplayString("The airport does not have any departure flights.");
                }
            }
        }
    }
}
