namespace A2_CAB201
{
    /// <summary>
    /// PURPOSE DISPLAY HIGH LEVEL UI TO THE USER
    /// </summary>
    public class flightMenu
    {
        /// <summary>
        /// creates a registration list to allow it to add entries.
        /// </summary>
        private List<Registration> Register;
        /// <summary>
        /// Initilises a new registration list.
        /// </summary>
        /// <param name="registerList">The existing registrations in the list</param>
        public flightMenu(List<Registration> registerList)
        {
            Register = registerList;
        }

        /// <summary>
        /// an easier way to display strings instead of having to write cmdlineUI excessively
        /// </summary>
        /// <param name="str">The string to be displayed</param>
        public void DisplayMsg(string str)
        {
            cmdlineUI.DisplayString(str);
        }
        /// <summary>
        /// A header for welcoming the user initially
        /// </summary>
        public void Header()
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("=  Welcome to Brisbane Domestic Airport  =");
            Console.WriteLine("==========================================");
        }

        /// <summary>
        /// The main menu, where the code returns if all else fails
        /// </summary>
        /// <param name="choices">The list of choices</param>
        /// <param name="main">a string for the main menu.</param>
        /// <returns>a number which is used for switch cases.</returns>
        public int MainMenu(List<string> choices, string main)
        {
            cmdlineUI.DisplayString();
            int choice = showChoices(main, choices);
            return choice;
        } 
        /// <summary>
        /// Registration method, goes through each input and handles any errors through do while
        /// </summary>
        /// <param name="existingRegs">All existing registrations</param>
        /// <param name="type">the type of user</param>
        /// <returns>A new user to add to the list depending on type</returns>
        public Registration RegisterUser(List<Registration> existingRegs, string type)
        {
            string name;
            do
            {
                cmdlineUI.DisplayRegister("name");
                name = cmdlineUI.InputString();
            } while (!ErrorHandling.NameErr(name));
            string age_str;
            do
            {
                cmdlineUI.DisplayRegister("age between 0 and 99");
                age_str = cmdlineUI.InputString();
            } while (!ErrorHandling.AgeErr(age_str));
            int age = int.Parse(age_str);
            string mobile;
            do
            {
                cmdlineUI.DisplayRegister("mobile number");
                mobile = cmdlineUI.InputString();
            } while (!ErrorHandling.MobileErr(mobile));
            string email;
            do
            {
                cmdlineUI.DisplayRegister("email");
                email = cmdlineUI.InputString();
            } while (!ErrorHandling.EmailErr(email) || !ErrorHandling.EmailUsed(email, existingRegs));
            string password;
            do
            {
                cmdlineUI.DisplayRegister("password");
                DisplayMsg("Your password must:");
                DisplayMsg("-be at least 8 characters long");
                DisplayMsg("-contain a number");
                DisplayMsg("-contain a lowercase letter");
                DisplayMsg("-contain an uppercase letter");
                password = cmdlineUI.InputString();
            } while (!ErrorHandling.PasswordErr(password));
            if (type == "TRA")
            {
                DisplayMsg($"Congratulations {name}. You have registered as a traveller.");
                return new Traveller(type, name, age, mobile, email, password);
            }
            else if (type == "FRF")
            {
                int FF_Num;
                do
                {
                    cmdlineUI.DisplayRegister($"frequent flyer number between {FlightConst.FFNUM_MIN} and {FlightConst.FFNUM_MAX}");
                    FF_Num = cmdlineUI.InputInt();
                } while (!ErrorHandling.FFnumErr(FF_Num));
                int FF_Points;
                do
                {
                    cmdlineUI.DisplayRegister($"current frequent flyer points between {FlightConst.FFPOINTS_MIN} and {FlightConst.FFPOINTS_MAX}");
                    FF_Points = cmdlineUI.InputInt();
                } while (!ErrorHandling.FFpointsErr(FF_Points));
                DisplayMsg($"Congratulations {name}. You have registered as a frequent flyer.");
                return new frequentFlyer(type, name, age, mobile, email, password, FF_Num, FF_Points);
            }
            else
            {
                type = "FLM";
                int Staff_ID;
                do
                {
                    cmdlineUI.DisplayRegister($"staff id between {FlightConst.FMSTAFFID_MIN} and {FlightConst.FMSTAFFID_MAX}");
                    Staff_ID = cmdlineUI.InputInt();
                } while (!ErrorHandling.FMstaffID(Staff_ID));
                DisplayMsg($"Congratulations {name}. You have registered as a flight manager.");
                return new flightManager(type, name, age, mobile, email, password, Staff_ID);
            }
        }

        /// <summary>
        /// A method to show choices to the user
        /// </summary>
        /// <param name="title">An initial prompt or message for the user</param>
        /// <param name="choices">the various options that a user can choose from</param>
        /// <returns>the users inputted number and minuses 1 to compensate for the code counting from 0</returns>
        public static int showChoices(string title, List<string> choices)
        {
            if (choices.Count <= 0)
            {
                return -1;
            }
            if (!string.IsNullOrEmpty(title))
            {
                cmdlineUI.DisplayString(title);
            }
            int Digits = choices.Count;
            for (int i = 0; i < Digits; i++) {
                cmdlineUI.DisplayString($"{i + 1}. {choices[i]}");
            }

            int choice = cmdlineUI.InputInt($"Please enter a choice between 1 and {choices.Count}:");

            return choice - 1;
        }

        /// <summary>
        /// Perform the log in and handles any errors through do whiles
        /// </summary>
        /// <param name="existingRegs">All existing registrations</param>
        /// <param name="controller">the flight controller class</param>
        /// <param name="flights">all existing flights</param>
        /// <param name="bookedFlightsList">all exisiting booked flights</param>
        public void PerformLogin(List<Registration> existingRegs, flightController controller, List<Flights> flights, List<booked_flights> bookedFlightsList)
        {
            string email_entered;
            string password_entered;

            do
            {
                cmdlineUI.DisplayRegister("email");
                email_entered = cmdlineUI.InputString();
            } while (!ErrorHandling.EmailErr(email_entered) || !ErrorHandling.Log_in_email(email_entered, existingRegs));

            do
            {
                cmdlineUI.DisplayRegister("password");
                password_entered = cmdlineUI.InputString();
            } while (!ErrorHandling.PasswordErr(password_entered) || !ErrorHandling.Log_in_pword(password_entered, existingRegs));

            var matched_reg = existingRegs.FirstOrDefault(r => r.Email == email_entered && r.Password == password_entered);
            if (matched_reg != null)
            {
                cmdlineUI.DisplayLogIn(matched_reg.Name);
                controller.PROCESS_Login_Menu(matched_reg, flights, bookedFlightsList);
            }
            else
            {
            }
        }
    }
}
