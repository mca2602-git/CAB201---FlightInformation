namespace A2_CAB201
{
    /// <summary>
    /// RESPONSIBILITY: The entry point for an application. 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method and entry point into the program.
        /// </summary>
        /// <param name="args">List of command line arguments</param>
        static void Main(string[] args)
        {
            flightController app = new flightController();
            app.Run();
        }
    }
}
