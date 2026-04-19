namespace A2_CAB201
{
    /// <summary>
    /// THE PURPOSE OF FREQUENT FLYER - to seperate all UNIQUE interaction in the log in menu, inherits Traveller as they contain similar methods.
    /// </summary>
    public class frequentFlyer : Traveller
    {
        /// <summary>
        /// Initializing the new variables, unique to frequent flyer (now referring to as FF)
        /// </summary>
        public int FF_Num { get; set; }
        public int FF_Points { get; set; }

        /// <summary>
        /// overrides the virtual void found in the registration class, adds 2 more lines to display the users FF numbers and FF points
        /// </summary>
        public override void DisplayDetails()
        {
            base.DisplayDetails(); // Show base info
            Console.WriteLine($"Frequent flyer number: {FF_Num}");
            Console.WriteLine($"Frequent flyer points: {FF_Points:N0}");
        }

        /// <summary>
        /// The purpose of this method is to have something happen depending what the user inputs, 
        /// for the first 4 cases its the same as the traveller, but with the 5th case it shows the FF their points
        /// </summary>
        /// <param name="choice">What the user has input as their choice if it isn't within the 0-5 range it will go back to the main menu</param>
        /// <param name="matched_reg">All existing registrations</param>
        /// <param name="controller">The flight controller class</param>
        /// <param name="flights">Existing flights</param>
        /// <param name="bookedFlightsList">Exisiting booked flights</param>
        public override void LoginChoices(int choice, Registration matched_reg, flightController controller, List<Flights> flights, List<booked_flights> bookedFlightsList)
        {
            switch (choice)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    base.LoginChoices(choice, matched_reg, controller, flights, bookedFlightsList);
                    break;
                case 5:
                    // frequent flyer points
                    CalculateFFPoints(this, bookedFlightsList);
                    break;
                default:
                    break;


            }
        }
        /// <summary>
        /// adds more lines to the registration table and allows for a FF to enter FF numbers and points
        /// </summary>
        /// <param name="type">The type of user</param>
        /// <param name="name">The name of the user</param>
        /// <param name="age">The age of the user</param>
        /// <param name="mobile">the phone number of the user</param>
        /// <param name="email">The unique email of the user</param>
        /// <param name="password">the password of the user</param>
        /// <param name="FF_Num">The FF numbers of the user</param>
        /// <param name="FF_Points">The FF points of the user</param>
        public frequentFlyer(string type, string name, int age, string mobile, string email, string password, int FF_Num, int FF_Points) 
            : base ("FRF", name, age, mobile, email, password)
        {
            this.Type = type;
            this.Name = name;
            this.Age = age;
            this.Mobile = mobile;
            this.Email = email;
            this.Password = password;
            this.FF_Num = FF_Num;
            this.FF_Points = FF_Points;
        }

        /// <summary>
        /// A way to calculate what the FF's points will be before and after booking an arrival and departure flight
        /// </summary>
        /// <param name="ff">The specific frequent flyer</param>
        /// <param name="bookedFlightsList">the list of booked flights (their booked flights)</param>
        private void CalculateFFPoints(frequentFlyer ff, List<booked_flights> bookedFlightsList)
        {
            int inital_points = ff.FF_Points;
            int total_points = ff.FF_Points;
            DisplayMsg($"Your current points are: {ff.FF_Points:N0}.");

            var arrCities = bookedFlightsList
                .Where(bf => bf.booked_Type == FlightConst.ARRIVALTYPE && bf.booked_Email == ff.Email)
                .Select(bf => bf.booked_City)
                .Distinct();

            var depCities = bookedFlightsList
                .Where(bf => bf.booked_Type == FlightConst.DEPARTURETYPE && bf.booked_Email == ff.Email)
                .Select(bf => bf.booked_City)
                .Distinct();

            int DisplayPoints(string type, IEnumerable<string> cities)
            {
                int sum = 0;
                foreach (var city in cities)
                {
                    if (FlightConst.CityPoints.TryGetValue(city, out int pts))
                    {
                        sum += pts;
                        DisplayMsg($"Your points from your {type} flight will be : {pts:N0}.");
                    }
                }
                return sum;
            }
            int arrivalPts = DisplayPoints("arrival", arrCities);
            int departurePts = DisplayPoints("departure", depCities);
            total_points += arrivalPts + departurePts;
            if (total_points != inital_points) {
                if (arrivalPts != 0 && departurePts != 0)
                {
                    DisplayMsg($"After completing your flights your new points will be: {total_points:N0}.");
                }
                else
                {
                    DisplayMsg($"After completing your flight your new points will be: {total_points:N0}.");
                }
            }

        }
    }
}
