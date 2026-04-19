namespace A2_CAB201
{
    /// <summary>
    /// STORE ALL CONSTANTS OR VALUES THAT DO NOT CHANGE, THIS IS DONE FOR READABILITY AND EASY ACCESS
    /// </summary>
    internal class FlightConst
    {
        public const string DATETIMEFORMAT = "HH:mm dd/MM/yyyy";

        public static readonly Dictionary<string, string> AirlineNames = new()
        {
            {"JST", "Jetstar"},
            {"QFA", "Qantas"},
            {"RXA", "Regional Express"},
            {"VOZ", "Virgin"},
            {"FRE", "Fly Pelican"}
        };
        public const string SYD = "Sydney";
        public const string MEL = "Melbourne";
        public const string ROC = "Rockhampton";
        public const string ADE = "Adelaide";
        public const string PER = "Perth";

        public static readonly Dictionary<string, int> CityPoints = new Dictionary<string, int>
        {
        { SYD, 1200 },
        { MEL, 1750 },
        { ROC, 1400 },
        { ADE, 1950 },
        { PER, 3375 }
        };


        public const string ARRIVALTYPE = "A";
        public const string DEPARTURETYPE = "D";

        public const string EMAILREGISTERED = "Email already";

        public const int FFNUM_MIN = 100000;
        public const int FFNUM_MAX = 999999;

        public const int FFPOINTS_MIN = 0;
        public const int FFPOINTS_MAX = 1000000;


        public const int FMSTAFFID_MIN = 1000;
        public const int FMSTAFFID_MAX = 9000;

        public const int SEATMIN = 1;
        public const int SEATMAX = 10;

        public const int AGEMIN = 0;
        public const int AGEMAX = 99;
    }
}
