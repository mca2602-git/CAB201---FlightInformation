namespace A2_CAB201
{
    /// <summary>
    /// PURPOSE IS TO CONNECT MENUS TABLES AND CMDLINE TOGETHER AND RUN PROCESSES
    /// </summary>
    public class flightController
    {
        /// <summary>
        /// Initialises the various menus and lists needed
        /// </summary>
        private flightMenu menu;
        private List<booked_flights> bookedflight = new List<booked_flights>();
        public List<Registration> Register = new List<Registration>();
        private List<Flights> flights = new List<Flights>();

        /// <summary>
        /// Creates a new flight menu when initialising
        /// </summary>
        public flightController()
        {
            menu = new flightMenu(Register);
        }

        /// <summary>
        /// Is run straight away when program begins, and continues running until told to stop.
        /// </summary>
        public void Run()
        {
            menu.Header();
            bool running = true;
            while (running)
            {
                running = RunMainMenu();
            }
        }


        /// <summary>
        /// The base menu before finishing running, returns false to stop the program and true to continue.
        /// </summary>
        public bool RunMainMenu()
        {
            const string MainMenu = "Please make a choice from the menu below:";
            const string LOG_str = "Login as a registered user.";
            const string REG_str = "Register as a new user.";
            const string EXIT_str = "Exit.";

            List<string> choices = new List<string>();
            choices.Add(LOG_str);
            choices.Add(REG_str);
            choices.Add(EXIT_str);

            const int LOG_int = 0, REG_int = 1, EXIT_int = 2;

            int choice = menu.MainMenu(choices, MainMenu);
            
            switch (choice)
            {
                case LOG_int:
                    PROCESS_log_in();
                    break;
                case REG_int:
                    PROCESS_registration();
                    break;
                case EXIT_int:
                    menu.DisplayMsg("Thank you. Safe travels.");
                    return false;
                    break;
                default:
                    return false;
                    break;
            }
            return true;
        }

        /// <summary>
        /// Runs the log in menu and double checks if there is even any logins to log in to.
        /// </summary>
        private void PROCESS_log_in() 
        {
            const string PRO_LOG_str = "Login Menu.";
            cmdlineUI.DisplayString(PRO_LOG_str);
            if (Register.Count == 0)
            {
                cmdlineUI.DisplayRegError("There are no people");
            }
            else
            {
                menu.PerformLogin(Register, this, flights, bookedflight);
            }
        }

        /// <summary>
        /// Begins the log in for ALL users and initialises their menu depending on the type of user they are.
        /// </summary>
        /// <param name="matched_reg">All existing registrations</param>
        /// <param name="flights">All existing flights available.</param>
        /// <param name="booked_Flights">All existing booked flights</param>
        public void PROCESS_Login_Menu(Registration matched_reg, List<Flights> flights, List<booked_flights> booked_Flights)
        {
            bool running = true;
            while (running)
            {
                cmdlineUI.DisplayString();
                if (matched_reg is Traveller && matched_reg is not frequentFlyer)
                {
                    cmdlineUI.DisplayString("Traveller Menu.");
                }
                if (matched_reg is frequentFlyer)
                {
                    cmdlineUI.DisplayString("Frequent Flyer Menu.");
                }
                if (matched_reg is flightManager)
                {
                    cmdlineUI.DisplayString("Flight Manager Menu.");
                }
                const string msg = "Please make a choice from the menu below:";
                List<string> choices = new List<string>()
                {
                    "See my details.",
                    "Change password."
                };
                if (matched_reg is Traveller or frequentFlyer)
                {
                    choices.Add("Book an arrival flight.");
                    choices.Add("Book a departure flight.");
                    choices.Add("See flight details.");
                }

                if (matched_reg is frequentFlyer)
                {
                    choices.Add("See frequent flyer points.");
                }

                if (matched_reg is flightManager)
                {
                    choices.AddRange(new List<string>
                {
                "Create an arrival flight.",
                "Create a departure flight.",
                "Delay an arrival flight.",
                "Delay a departure flight.",
                "See the details of all flights."
                });
                }
                choices.Add("Logout.");

                int choice = menu.MainMenu(choices, msg);
                if (choice == choices.Count - 1)
                {
                    running = false;
                }
                else
                {
                    matched_reg.LoginChoices(choice, matched_reg, this, flights, booked_Flights);
                }
            }
        }

        /// <summary>
        /// Runs the registration process and stores it in the registration table.
        /// </summary>
        private void PROCESS_registration() 
        {
            const string PRO_TYPE_str = "Which user type would you like to register?";
            const string PRO_TRA_str = "A standard traveller.";
            const string PRO_FF_str = "A frequent flyer.";
            const string PRO_FLM_str = "A flight manager.";
            List<string> choices = new List<string>() { PRO_TRA_str, PRO_FF_str, PRO_FLM_str }; 

            const int PRO_TRA_int = 0, PRO_FF_int = 1, PRO_FLM_int = 2;

            int choice = menu.MainMenu(choices, PRO_TYPE_str);
            Registration newReg;

            switch (choice)
            {
                case PRO_TRA_int:
                    menu.DisplayMsg("Registering as a traveller.");
                    newReg = menu.RegisterUser(Register, "TRA");
                    Register.Add(newReg);
                    break;
                case PRO_FF_int:
                    menu.DisplayMsg("Registering as a frequent flyer.");
                    newReg = menu.RegisterUser(Register, "FRF");
                    Register.Add(newReg);
                    break;
                case PRO_FLM_int:
                    menu.DisplayMsg("Registering as a flight manager.");
                    newReg = menu.RegisterUser(Register, "FLM");
                    Register.Add(newReg);
                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// Creates a flight and stores it in the flight list.
        /// </summary>
        /// <param name="airline">The shortened airline code</param>
        /// <param name="type">The type of flight, departure or arrival (CONST)</param>
        /// <param name="flights">the already existing flights.</param>
        public void PROCESS_create_flight(string airline, string type, List<Flights> flights)
        {
            string City;
            if (type == FlightConst.ARRIVALTYPE)
            {
                City = "Please enter the departing city:";
            }
            else
            {
                City = "Please enter the arrival city:";
            }

            List<string> choices = new List<string>();
            choices.Add(FlightConst.SYD);
            choices.Add(FlightConst.MEL);
            choices.Add(FlightConst.ROC);
            choices.Add(FlightConst.ADE);
            choices.Add(FlightConst.PER);

            const int SYD_int = 0, MEL_int = 1, ROC_int = 2, ADE_int = 3, PER_int = 4;

            int choice = flightMenu.showChoices(City, choices);

            string city_choice;
            switch (choice)
            {
                case SYD_int:
                    city_choice = FlightConst.SYD;
                    break;
                case MEL_int:
                    city_choice = FlightConst.MEL;
                    break;
                case ROC_int:
                    city_choice = FlightConst.ROC;
                    break;
                case ADE_int:
                    city_choice = FlightConst.ADE;
                    break;
                case PER_int:
                    city_choice = FlightConst.PER;
                    break;
                default:
                    city_choice = "";
                    break;
            }
            cmdlineUI.DisplayRegister("flight id between 100 and 900");
            int flight_ID = cmdlineUI.InputInt();
            cmdlineUI.DisplayRegister("plane id between 0 and 9");
            int plane_ID = cmdlineUI.InputInt();
            if (type == FlightConst.ARRIVALTYPE)
            {
                cmdlineUI.DisplayString($"Please enter in the arrival date and time in the format {FlightConst.DATETIMEFORMAT}:");
            }
            else
            {
                cmdlineUI.DisplayString($"Please enter in the departure date and time in the format {FlightConst.DATETIMEFORMAT}:");

            }
            DateTime arrival_time = cmdlineUI.GetDateTime();
            bool flightExists = flights.Any(flight => flight.Airline == airline && flight.Plane_ID == plane_ID && flight.Type == type);
            if (!flightExists)
            {
                cmdlineUI.DisplayFlight(airline, flight_ID, plane_ID, arrival_time, type);
                Flights newFlight = new Flights(airline, flight_ID, plane_ID, arrival_time, city_choice, type);
                flights.Add(newFlight);
            }
            else
            {
                if (type == FlightConst.ARRIVALTYPE)
                {
                    cmdlineUI.GeneralError($"Plane {airline}{plane_ID}{type} has already been assigned to an arrival flight");
                }
                if (type == FlightConst.DEPARTURETYPE)
                {
                    cmdlineUI.GeneralError($"Plane {airline}{plane_ID}{type} has already been assigned to a departure flight");
                }
            }
        }
    }
}
