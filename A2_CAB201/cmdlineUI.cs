using System.Globalization;
namespace A2_CAB201
{
    /// <summary>
    /// THE PURPOSE OF THE CMDLINEUI IS TO READ AND WRITE USER INTERACTIONS
    /// </summary>
    public class cmdlineUI
    {
        /// <summary>
        /// Display Strings are write lines which can display something to the user
        /// for an empty line
        /// </summary>
        public static void DisplayString()
        {
            Console.WriteLine();
        }
        /// <summary>
        /// to display a message which is unformatted or unique
        /// </summary>
        /// <param name="msg">Any string, sentence or description</param>
        public static void DisplayString(string msg)
        {
            Console.WriteLine(msg);
        }

        /// <summary>
        /// to display an error about a supplied field being invalid and encourage user to try again
        /// </summary>
        /// <param name="msg">A supplied value which is invalid</param>
        public static void DisplayError(string msg)
        {
            Console.WriteLine("#####");
            Console.WriteLine($"# Error - Supplied {msg} is invalid.");
            Console.WriteLine("# Please try again.");
            Console.WriteLine("#####");
        }

        /// <summary>
        /// to display that what the user has entered does not match what has been stored
        /// General Error, when the error is unlike any other error
        /// </summary>
        /// <param name="msg">A supplied value which is invalid</param>
        public static void GeneralError(string msg)
        {
            Console.WriteLine("#####");
            Console.WriteLine($"# Error - {msg}.");
            Console.WriteLine("#####");
        }
        /// <summary>
        /// to display that what the user has entered does not match what has been stored and encourages user to try again
        /// General Error, when the error is unlike any other error
        /// </summary>
        /// <param name="msg">A supplied value which is invalid</param>
        public static void GeneralErrorTry(string msg)
        {
            Console.WriteLine("#####");
            Console.WriteLine($"# Error - {msg}.");
            Console.WriteLine("# Please try again.");
            Console.WriteLine("#####");
        }

        /// <summary>
        /// to display that what the user has entered does not match what has been registered and encourage user to try again only when they are registering an email
        /// General Error, when the error is unlike any other error
        /// </summary>
        /// <param name="msg">A supplied value which is invalid</param>
        public static void DisplayRegError(string msg)
        {
            Console.WriteLine("#####");
            Console.WriteLine($"# Error - {msg} registered.");
            if (msg == FlightConst.EMAILREGISTERED)
            {
                Console.WriteLine("# Please try again.");
            }
            Console.WriteLine("#####");
        }
        /// <summary>
        /// Used when a user logs into their account
        /// </summary>
        /// <param name="msg">A users name</param>
        public static void DisplayLogIn(string msg)
        {
            Console.WriteLine($"Welcome back {msg}.");
        }
        /// <summary>
        /// Used when the program wants a user to enter in something specific
        /// </summary>
        /// <param name="msg">a specified field for a user to fill in</param>
        public static void DisplayRegister(string msg)
        {
            Console.WriteLine($"Please enter in your {msg}:");
        }
        /// <summary>
        /// To display a logged in persons details
        /// </summary>
        /// <param name="matched_reg">A registration that exists</param>
        public static void DisplayDetails(Registration matched_reg)
        {
            matched_reg.DisplayDetails();
        }

        /// <summary>
        /// To request a persons input (in string form)
        /// </summary>
        public static string InputString()
        {
            string input = Console.ReadLine();
            return input;
        }
        /// <summary>
        ///  To request a persons input (in string form) but have a unique prompt before it 
        /// </summary>
        /// <param name="msg">A prompt before the user enters an input</param>
        public static string InputString(string msg)
        {
            Console.WriteLine(msg);
            string input = Console.ReadLine();
            return input;
        }
        /// <summary>
        ///  To request a persons input (in integer form)
        /// </summary>
        public static int InputInt()
        {
            string input_string = Console.ReadLine();
            int input = int.Parse(input_string);
            return input;
        }
        /// <summary>
        /// To request a persons input (in integer form) but having a unique prompt before it.
        /// </summary>
        /// <param name="msg">a message before a user inputs an int</param>
        public static int InputInt(string msg)
        {
            Console.WriteLine(msg);
            string input_string = Console.ReadLine();
            int input = int.Parse(input_string);
            return input;

        }
        /// <summary>
        /// To request a person to enter a date and time in a specific format
        /// </summary>
        public static DateTime GetDateTime()
        {
            string input = Console.ReadLine();
            DateTime result;
            string format = FlightConst.DATETIMEFORMAT; /// Expected format is "HH:mm dd/MM/yyyy"
            bool dtWorked = DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
            if (!dtWorked)
            {
                DisplayError("Incorrect Date Format");
            }
            return result;
        }

        /// <summary>
        /// To display that a flight manager has successfully entered a flight into the system
        /// </summary>
        /// <param name="airline">The code or shortened name of a flight</param>
        /// <param name="flight_id">The Airline specified ID, to make each flight unique and identifiable</param>
        /// <param name="plane_id">The Plane specific ID, makes each plane unique and identifiable</param>
        /// <param name="date">The date and time the flight is set to leave</param>
        /// <param name="type"> The type of flight, whether it is an arrival or departure flight</param>
        public static void DisplayFlight(string airline, int flight_id, int plane_id, DateTime date, string type)
        {
            Console.WriteLine($"Flight {airline}{flight_id} on plane {airline}{plane_id}{type} has been added to the system.");
        }
        /// <summary>
        /// To display All flights to a flight manager
        /// </summary>
        /// <param name="airline"></param>
        /// <param name="flight_id"></param>
        /// <param name="plane_id"></param>
        /// <param name="date"></param>
        /// <param name="type"></param>
        /// <param name="city"></param>
        public static void DisplayAllFlights(string airline, int flight_id, int plane_id, DateTime date, string type, string city)
        {
            string airlineKey = airline.ToUpper().Trim();
            string full_airline = FlightConst.AirlineNames.TryGetValue(airlineKey, out var name)
            ? name
            : "Unknown Airline";
            string format = FlightConst.DATETIMEFORMAT;
            if (type == FlightConst.ARRIVALTYPE)
            {
                Console.WriteLine($"Flight {airline}{flight_id} operated by {full_airline} arriving at {date.ToString(format)} from {city} on plane {airline}{plane_id}{type}.");
            }
            else
            {
                Console.WriteLine($"Flight {airline}{flight_id} operated by {full_airline} departing at {date.ToString(format)} to {city} on plane {airline}{plane_id}{type}.");

            }
        }
        /// <summary>
        /// To display to a user (Frequent Flyer or Traveller) their booked flights
        /// </summary>
        /// <param name="airline">The code or shortened name of a flight</param>
        /// <param name="city">The specified city the plane with be arriving or departing from</param>
        /// <param name="time">The specified time the flight will be leaving</param>
        /// <param name="seat">The seat (1-10) in which a person chooses</param>
        /// <param name="row"> the row (A-D) a person will sit in</param>
        /// <param name="type"> The type of flight, whether it is an arrival or departure flight</param>
        public static void DisplayBookedFlight(string airline, string city, string time, int seat, string row, string type)
        {
            if (type == FlightConst.ARRIVALTYPE)
            {
                DisplayString($"Arrival Flight: Flight {airline} from {city} arriving at {time} in seat {seat}:{row}.");
            }
            if (type == FlightConst.DEPARTURETYPE)
            {
                DisplayString($"Departure Flight: Flight {airline} to {city} departing at {time} in seat {seat}:{row}.");
            }
        }
    }
}