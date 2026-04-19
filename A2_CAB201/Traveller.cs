using System.Text.RegularExpressions;
namespace A2_CAB201
{
    /// <summary>
    /// PURPOSE to create methods unique to the traveller and store anything unique to traveller users
    /// </summary>
    public class Traveller : Registration
    {
        public Traveller(string type, string name, int age, string mobile, string email, string password):
            base("TRA", name, age, mobile, email, password)
        {
        }

        /// <summary>
        /// The unique choices for the user which will be inherited by the FF class
        /// </summary>
        /// <param name="choice">what choice the traveller makes</param>
        /// <param name="matched_reg">existing registrations</param>
        /// <param name="controller">the flight controller class</param>
        /// <param name="flights">existing flights to book</param>
        /// <param name="bookedFlightsList">existing booked flights</param>
        public override void LoginChoices(int choice, Registration matched_reg, flightController controller, List<Flights> flights, List<booked_flights> bookedFlightsList)
        {
            switch (choice)
            {
                case 0:
                case 1:
                    base.LoginChoices(choice, matched_reg, controller, flights, bookedFlightsList);
                    break;
                case 2: // book arrival
                    BookFlight(FlightConst.ARRIVALTYPE, flights, bookedFlightsList, matched_reg);
                    break;
                case 3:// book departure
                    BookFlight(FlightConst.DEPARTURETYPE, flights, bookedFlightsList, matched_reg);
                    break;
                case 4:
                    SeeBookedFlights(bookedFlightsList, matched_reg);
                    break;
                default:
                    break;

            }
        }
        /// <summary>
        /// a method for the traveller to book an arrival and departure type
        /// </summary>
        /// <param name="flightType">The type of flight they wish to book (departure or arrival)</param>
        /// <param name="flights">all existing flights to book</param>
        /// <param name="bookedFlightsList">all existing booked flights</param>
        /// <param name="matched_reg">all existing registrations</param>
        private void BookFlight(string flightType, List<Flights> flights, List<booked_flights> bookedFlightsList, Registration matched_reg)
        {
            var oppositeType = (flightType == FlightConst.ARRIVALTYPE) ? FlightConst.DEPARTURETYPE : FlightConst.ARRIVALTYPE;
            var oppositeBooked = bookedFlightsList
                .Where(bf => bf.booked_Type == oppositeType && bf.booked_Email == matched_reg.Email)
                .OrderBy(bf => bf.booked_Time)
                .ToList();
            var oppositeTimes = oppositeBooked
                .Select(bf => DateTime.ParseExact(bf.booked_Time, FlightConst.DATETIMEFORMAT, System.Globalization.CultureInfo.InvariantCulture))
                .Distinct()
                .ToList();
            var currentBooked = bookedFlightsList
                .Where(bf => bf.booked_Type == flightType && bf.booked_Email == matched_reg.Email)
                .ToList();
            if (currentBooked.Count != 0)
            {
                cmdlineUI.GeneralError($"You already have {(flightType == FlightConst.ARRIVALTYPE ? "an arrival" : "a departure")} flight. You can not book another");
                return;
            }

            List<string> choices = new List<string>();
            bool validTime = false;
            string choice;
            do
            {
                choices.Clear();
                string FLIGHT_str = $"Please enter the {(flightType == FlightConst.ARRIVALTYPE ? "arrival" : "departure")} flight:";
                var availableFlights = flights
                    .Where(flight => flight.Type == flightType)
                    .OrderBy(f => f.time)
                    .ToList();
                string format = FlightConst.DATETIMEFORMAT;
                foreach (var flight in availableFlights)
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

                if (flightType == FlightConst.ARRIVALTYPE)
                {
                    validTime = Validation.ValidTime_DEP(oppositeTimes, choice, choices);
                }
                else
                {
                    validTime = Validation.validTime(oppositeTimes, choice, choices);
                }

            } while (!validTime);
            int choiceIndex = int.Parse(choice) - 1;
            string selectedFlightStr = choices[choiceIndex];

            string pattern = flightType == FlightConst.ARRIVALTYPE
            ? @"Flight (?<flightCode>\w+) operated by (?<fullAirline>.+?) arriving at (?<time>.+?) from (?<city>.+?) on plane (?<planeId>.+?)\."
            : @"Flight (?<flightCode>\w+) operated by (?<fullAirline>.+?) departing at (?<time>.+?) to (?<city>.+?) on plane (?<planeId>.+?)\.";

            Match match = Regex.Match(selectedFlightStr, pattern);
            string flightCode = match.Groups["flightCode"].Value;
            string flightTime = match.Groups["time"].Value;
            string city = match.Groups["city"].Value;

            int seatChoice;
            string columnChoice;
            // Seat selection loop
            do
            {
                do
                {
                    cmdlineUI.DisplayRegister($"seat row between {FlightConst.SEATMIN} and {FlightConst.SEATMAX}");
                    seatChoice = cmdlineUI.InputInt();
                } while (!Validation.validColumn(seatChoice));

                do
                {
                    cmdlineUI.DisplayRegister("seat column between A and D");
                    columnChoice = cmdlineUI.InputString().ToUpper();
                } while (!Validation.validSeat(columnChoice));
            } while (!Validation.OccupiedSeat(bookedFlightsList, seatChoice, columnChoice, flightCode, matched_reg));
            DisplayMsg($"Congratulations. You have booked flight {flightCode} {(flightType == FlightConst.ARRIVALTYPE ? "from" : "to")} {city} {(flightType == FlightConst.ARRIVALTYPE ? "arriving at" : "departing at")} {flightTime} and are seated in {seatChoice}:{columnChoice}.");

            var newBooking = new booked_flights(
            booked_Airline: flightCode,
            booked_City: city,
            booked_Time: flightTime,
            booked_Seat: seatChoice,
            booked_Row: columnChoice,
            booked_Type: flightType,
            booked_Email: matched_reg.Email);

            bookedFlightsList.Add(newBooking);
        }
        /// <summary>
        /// a way to see the details of their booked flights
        /// </summary>
        /// <param name="bookedFlightsList">all existing booked flights</param>
        /// <param name="matched_reg">all existing registrations</param>
        private void SeeBookedFlights(List<booked_flights> bookedFlightsList, Registration matched_reg)
        {
            if (bookedFlightsList.Any(bf => bf.booked_Type == FlightConst.ARRIVALTYPE && bf.booked_Email == matched_reg.Email))
            {
                DisplayMsg($"Showing flight details for {matched_reg.Name}:");
                var bookedflights = bookedFlightsList
                .Where(bf => bf.booked_Type == FlightConst.ARRIVALTYPE && bf.booked_Email == matched_reg.Email)
                .OrderBy(bf => bf.booked_Time)
                .ToList();
                foreach (var bf in bookedflights)
                {
                    cmdlineUI.DisplayBookedFlight(bf.booked_Airline, bf.booked_City, bf.booked_Time, bf.booked_Seat, bf.booked_Row, bf.booked_Type);
                }
            }
            if (bookedFlightsList.Any(bf => bf.booked_Type == FlightConst.DEPARTURETYPE && bf.booked_Email == matched_reg.Email))
            {
                if (!bookedFlightsList.Any(bf => bf.booked_Type == FlightConst.ARRIVALTYPE && bf.booked_Email == matched_reg.Email))
                {
                    DisplayMsg($"Showing flight details for {matched_reg.Name}:");
                }
                var bookedflights2 = bookedFlightsList
                .Where(bf => bf.booked_Type == FlightConst.DEPARTURETYPE && bf.booked_Email == matched_reg.Email)
                .OrderBy(bf => bf.booked_Time)
                .ToList();
                foreach (var bf in bookedflights2)
                {
                    cmdlineUI.DisplayBookedFlight(bf.booked_Airline, bf.booked_City, bf.booked_Time, bf.booked_Seat, bf.booked_Row, bf.booked_Type);
                }
            }
        }
    }
}
