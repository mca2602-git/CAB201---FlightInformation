namespace A2_CAB201
{
    /// <summary>
    /// THE PURPOSE OF ERROR HANDLING IS TO TAKE THE VALIDATIONS AND OUTPUT ERROR MESSAGES - NO USER INTERACTION
    /// </summary>
    public class ErrorHandling
    {
        /// <summary>
        /// A shortened and easier way to display errors from the cmdlineUI class
        /// </summary>
        /// <param name="errStr">A specific value that is incorrect</param>
        private static void DisplayErr(string errStr)
        {
            cmdlineUI.DisplayError(errStr);
        }
        /// <summary>
        /// A way to check if a users entered email matches any other emails, if it doesm't an error occurs and they cannot log in
        /// </summary>
        /// <param name="email">A users entered email from a prompt</param>
        /// <param name="existingRegs">All existing registrations</param>
        public static bool Log_in_email(string email, List<Registration> existingRegs)
        {
            if (Validation.EmailNotReg(email, existingRegs))
            {
                cmdlineUI.DisplayRegError("Email is not");
                return false;
            }
            return true;
        }
        /// <summary>
        /// A way to check if a person has entered the correct password and if not, send error message
        /// </summary>
        /// <param name="password">The users entered password (to log in)</param>
        /// <param name="existingRegs">Existing passwords in the system</param>
        public static bool Log_in_pword(string password, List<Registration> existingRegs)
        {
            if (Validation.IncorrectPword(password, existingRegs))
            {
                cmdlineUI.GeneralError("Incorrect Password");
                return false;
            }
            return true;
        }
        /// <summary>
        /// A way to check if a person is following the naming conventions and pass an error
        /// </summary>
        /// <param name="name">The users entered name</param>
        public static bool NameErr(string name)
        {
            if (Validation.validationName(name))
            {
                DisplayErr("name");
                return false;
            }
            return true;
        }
        /// <summary>
        /// A way to check if a person age is within the range given or if it is even an integer and pass an error
        /// </summary>
        /// <param name="age">The users entered age.</param>
        public static bool AgeErr(string age)
        {
            if (Validation.validationAge(age))
            {
                DisplayErr("value");
                return false;
            }
            else if (Validation.validationAgeRange(age))
            {
                DisplayErr("age");
                return false;
            }
            return true;
        }
        /// <summary>
        /// a way to check if the entered phone number follows the traditional mobile phone structure and pass an error
        /// </summary>
        /// <param name="mobile">The users entered phone number</param>
        public static bool MobileErr(string mobile)
        {
            if (Validation.validationMobile(mobile))
            {
                DisplayErr("mobile number");
                return false;
            }
            return true;
        }
        /// <summary>
        /// A way to check if the email has an @ and something before and after, if not return a error
        /// </summary>
        /// <param name="email">The email string to validate</param>
        public static bool EmailErr(string email)
        {
            if (Validation.validationEmail(email))
            {
                DisplayErr("email");
                return false;
            }
            return true;
        }
        /// <summary>
        /// A way to check if the email has been used already when register, if it has return an error.
        /// </summary>
        /// <param name="email">The email string to check for duplicates</param>
        /// <param name="Register">List of existing registrations</param>
        public static bool EmailUsed(string email, List<Registration> Register)
        {
            if (Validation.validationDuplicate(email, Register))
            {
                cmdlineUI.DisplayRegError(FlightConst.EMAILREGISTERED);
                return false;
            }
            return true;
        }
        /// <summary>
        /// To check if the user is following the given structure for a password.
        /// </summary>
        /// <param name="password">The users entered password for registration and login</param>
        public static bool PasswordErr(string password)
        {
            if (Validation.validationPassword(password))
            {
                DisplayErr("password");
                return false;
            }
            return true;
        }
        /// <summary>
        /// A way to check if the entered numbers is within a valid range for the system.
        /// </summary>
        /// <param name="number">The entered frequent flyer numbers</param>
        public static bool FFnumErr(int number)
        {
            if (number < FlightConst.FFNUM_MIN || number > FlightConst.FFNUM_MAX)
            {
                DisplayErr("frequent flyer number");
                return false;
            }
            return true;
        }
        /// <summary>
        /// To check if the entered points is within a valid range/possible
        /// </summary>
        /// <param name="points">The entered value of a frequent flyers points</param>
        public static bool FFpointsErr(int points)
        {
            if (points < FlightConst.FFPOINTS_MIN || points > FlightConst.FFPOINTS_MAX)
            {
                DisplayErr("current frequent flyer points");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Checking if the ID of a flight manager is within the given parameters
        /// </summary>
        /// <param name="ID">The entered value of a flight managers ID</param>
        public static bool FMstaffID(int ID)
        {
            if (ID < FlightConst.FMSTAFFID_MIN || ID > FlightConst.FMSTAFFID_MAX)
            {
                DisplayErr("staff id");
                return false;
            }
            return true;
        }
    }

}
