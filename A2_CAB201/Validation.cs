using System.Text.RegularExpressions;
namespace A2_CAB201
{

    /// <summary>
    /// To validate all user inputs where necessary
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// A way to verify that the password the user enters matches the one stored
        /// </summary>
        /// <param name="old_pword">the users inputted password</param>
        /// <param name="matched_reg">all existing users</param>
        
        public static bool MatchPass(string old_pword, Registration matched_reg)
        {
            if (old_pword != matched_reg.Password)
            {
                cmdlineUI.GeneralErrorTry("Entered password does not match existing password");
                return true;
            }
            return false;
        }
        /// <summary>
        /// A way to verify if the email is registered or not
        /// </summary>
        /// <param name="email">The email the user has entered</param>
        /// <param name="Register">The list of existing registrations</param>
        
        public static bool EmailNotReg(string email, List<Registration> Register)
        {
            if (Register.FirstOrDefault(r => r.Email == email) == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// A way to verify if the password is stored or if the password doesn't exist
        /// </summary>
        /// <param name="password">The entered password from the user.</param>
        /// <param name="Register">The list of existing registrations</param>
        
        public static bool IncorrectPword(string password, List<Registration> Register)
        {
            if (Register.FirstOrDefault(r => r.Password == password) == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// A way to check if the name isnt empty and has only letters
        /// </summary>
        /// <param name="name">The inputted name of the user</param>
        
        public static bool validationName(string name)
        {
            string pattern = @"^(?=.*[a-zA-Z'-])[a-zA-Z\s'-]+$";
            return string.IsNullOrEmpty(name) || Regex.IsMatch(name, pattern) == false;

        }
        /// <summary>
        /// A way to check if the user has inputted a number
        /// </summary>
        /// <param name="age">The users inputted age</param>
        
        public static bool validationAge(string age)
        {
            int age_entered;
            return int.TryParse(age, out age_entered) == false;
        }
        /// <summary>
        /// A way to check if the users age is within the valid range
        /// </summary>
        /// <param name="age">The users entered age</param>
        
        public static bool validationAgeRange(string age)
        {
            int age_entered;
            int.TryParse(age, out age_entered);
            return age_entered < FlightConst.AGEMIN || age_entered > FlightConst.AGEMAX;
        }
        /// <summary>
        /// A way to check that the phone number starts with a 0, is at least 10 characters and is actually a number
        /// </summary>
        /// <param name="mobile">the users entered phone number</param>
        
        public static bool validationMobile(string mobile)
        {
            string pattern_phone = @"^0+\d+";
            int phone_entered;
            return mobile.Length < 10 || Regex.IsMatch(mobile, pattern_phone) == false || int.TryParse(mobile, out phone_entered) == false;
        }
        /// <summary>
        /// a way to check that the email contains an @ and a letter before and after
        /// </summary>
        /// <param name="email">The email the user has entered</param>
        
        public static bool validationEmail(string email)
        {
            string pattern_email = @"^[^@]+@[^@]+$";
            return Regex.IsMatch(email, pattern_email) == false;
        }
        /// <summary>
        /// Checks if the entered email is like any stored emails
        /// </summary>
        /// <param name="email">The email the user has entered</param>
        /// <param name="travellers">existing registrations</param>
        
        public static bool validationDuplicate(string email, List<Registration> travellers)
        {
            return travellers.Any(t => t.Email == email);
        }

        /// <summary>
        /// Checks that the password has at least 1 capital letter, 1 lower case, is 8 characters long and has a number
        /// </summary>
        /// <param name="password">The entered password from the user.</param>
        public static bool validationPassword(string password)
        {
            string pattern_password = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";
            return Regex.IsMatch(password, pattern_password) == false;
        }
        /// <summary>
        /// Ensures that users choice is with the given range
        /// </summary>
        /// <param name="range_str">The inputted number</param>
        /// <param name="chosenRange">The chosen range</param>
        public static bool validRange(string range_str, List<string> chosenRange)
        {
            int range;
            if (int.TryParse(range_str, out range))
            {
                range = int.Parse(range_str) - 1;
                if (range < 0 || range >= chosenRange.Count)
                {
                    cmdlineUI.GeneralErrorTry("Supplied value is out of range");
                    return false;
                }
            }
            else
            {
                cmdlineUI.GeneralErrorTry("Supplied value is out of range");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Checks that the departure time is after the arrival time
        /// </summary>
        /// <param name="arrival">the arrival time</param>
        /// <param name="choice_time">the chosen flight</param>
        /// <param name="chosenTime">The chosen list flight</param>
        
        public static bool validTime(List<DateTime> arrival, string choice_time, List<string> chosenTime)
        {
            if (arrival == null || arrival.Count == 0)
            {
                return true;
            }
            string pattern2 = @"Flight (?<flightCode>\w+) operated by (?<fullAirline>.+?) departing at (?<time>.+?) to (?<city>.+?) on plane (?<planeId>.+?)\.";
            int choice_time_int = int.Parse(choice_time) - 1;
            bool success = int.TryParse(choice_time, out choice_time_int);
            choice_time_int -= 1;

            if (!success || choice_time_int < 0 || choice_time_int >= chosenTime.Count)
            {
                return false;
            }
            string chosen_departure = chosenTime[choice_time_int];
            Match md = Regex.Match(chosen_departure, pattern2);
            string Dtime = md.Groups["time"].Value;
            DateTime departure_time = DateTime.ParseExact(Dtime, FlightConst.DATETIMEFORMAT, System.Globalization.CultureInfo.InvariantCulture);
            DateTime earliestArr = arrival.Min();
            if (departure_time > earliestArr)
            {
                return true;
            }
            else
            {
                // if departure time is before arrival
                cmdlineUI.GeneralErrorTry("The departure time must be after the arrival time");
                return false;
            }
        }
        /// <summary>
        /// Checks that the arrival time is before the departure time
        /// </summary>
        /// <param name="departure">the departure time</param>
        /// <param name="choice_time">the chosen flight</param>
        /// <param name="chosenTime">The chosen list flight</param>

        public static bool ValidTime_DEP(List<DateTime> departure, string choice_time, List<string> chosenTime)
        {
            if (departure == null || departure.Count == 0)
            {
                return true;
            }
            string pattern2 = @"Flight (?<flightCode>\w+) operated by (?<fullAirline>.+?) arriving at (?<time>.+?) from (?<city>.+?) on plane (?<planeId>.+?)\.";
            int choice_time_int = int.Parse(choice_time) - 1;
            bool success = int.TryParse(choice_time, out choice_time_int);
            choice_time_int -= 1;

            if (!success || choice_time_int < 0 || choice_time_int >= chosenTime.Count)
            {
                return false;
            }
            string chosen_arrival = chosenTime[choice_time_int];
            Match md = Regex.Match(chosen_arrival, pattern2);
            string Atime = md.Groups["time"].Value;
            DateTime arrival_time = DateTime.ParseExact(Atime, FlightConst.DATETIMEFORMAT, System.Globalization.CultureInfo.InvariantCulture);
            DateTime earliestDep = departure.Min();
            if (earliestDep > arrival_time)
            {
                return true;
            }
            else
            {
                // if departure time is before arrival
                cmdlineUI.GeneralErrorTry("The arrival time must be before the departure time");
                return false;
            }
        }
        /// <summary>
        /// Checks that the users row is within the valid range
        /// </summary>
        /// <param name="row">the chosen row</param>
        public static bool validColumn(int row)
        {
            if (row <= 0 || row > 10)
            {
                cmdlineUI.DisplayError("seat row");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Checks that the users column is within the valid range
        /// </summary>
        /// <param name="column_choice">the chosen column</param>

        public static bool validSeat(string column_choice)
        {
            if (column_choice != "A" && column_choice != "B" && column_choice != "C" && column_choice != "D")
            {
                cmdlineUI.DisplayError("seat column");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Checks that the seat chosen isn't occupied - NOT FINISHED
        /// </summary>
        /// <param name="booked_flight">all booked flights</param>
        /// <param name="chosen_seat">users chosen seat</param>
        /// <param name="chosen_column">user chosen column</param>
        /// <param name="flight_code">flight codes</param>
        /// <param name="matched_reg">all existing registrations</param>
        
        public static bool OccupiedSeat(List<booked_flights> booked_flight, int chosen_seat, string chosen_column, string flight_code, Registration matched_reg)
        {
            var Booked = booked_flight
            .OrderBy(f => f.booked_Time)
            .ToList();
            foreach (var booked in Booked)
            {
                if (matched_reg.Type == "FRF")
                {
                    return true;
                }
                else
                {
                    if (booked.booked_Seat == chosen_seat && booked.booked_Row == chosen_column && booked.booked_Airline == flight_code)
                    {
                        cmdlineUI.GeneralErrorTry("Seat is already occupied");
                        return false;
                    }
                }
            }
            return true;

        }
    }
}
