namespace A2_CAB201
{
    /// <summary>
    /// PURPOSE: place for registering users and where all similar information is store for all users
    /// </summary>
    public class Registration
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// an easier way to display a string
        /// </summary>
        /// <param name="str">the string to be display</param>
        public void DisplayMsg(string str)
        {
            cmdlineUI.DisplayString(str);
        }

        /// <summary>
        /// the default display details that will always display for every user
        /// </summary>
        public virtual void DisplayDetails()
        {
            Console.WriteLine("Your details.");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Age: {Age}");
            Console.WriteLine($"Mobile phone number: {Mobile}");
            Console.WriteLine($"Email: {Email}");
        }
        /// <summary>
        /// The default list parameters for the user
        /// </summary>
        /// <param name="type">The type of user</param>
        /// <param name="name">The name of the user</param>
        /// <param name="age">The age of the user</param>
        /// <param name="mobile">the phone number of the user</param>
        /// <param name="email">The unique email of the user</param>
        /// <param name="password">the password of the user</param>
        public Registration(string type, string name, int age, string mobile, string email, string password)
        {
            this.Type = type;
            this.Name = name;
            this.Age = age;
            this.Mobile = mobile;
            this.Email = email;
            this.Password = password;

        }
        /// <summary>
        /// The default log in choices that every user has
        /// </summary>
        /// <param name="choice">What the user chooses</param>
        /// <param name="matched_reg">exisiting registrations</param>
        /// <param name="controller">the flight controller class</param>
        /// <param name="flights">all existing flights</param>
        /// <param name="bookedFlightsList">all existing booked flights</param>
        public virtual void LoginChoices(int choice, Registration matched_reg, flightController controller, List<Flights> flights, List<booked_flights> bookedFlightsList)
        {
            switch (choice)
            {
                case 0:
                    cmdlineUI.DisplayDetails(matched_reg);
                    break;
                case 1:
                    ChangePassword(matched_reg);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// method for the user to change their password
        /// </summary>
        /// <param name="matched_reg">All existing registrations</param>
        public void ChangePassword(Registration matched_reg)
        {
            string old_pword;
            do
            {
                cmdlineUI.DisplayString("Please enter your current password.");
                old_pword = cmdlineUI.InputString();
            } while (Validation.MatchPass(old_pword, matched_reg));
            cmdlineUI.DisplayString("Please enter your new password.");
            string new_pword = cmdlineUI.InputString();
            matched_reg.Password = new_pword;
        }
    }
}