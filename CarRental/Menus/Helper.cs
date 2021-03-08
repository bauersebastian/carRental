using System;
namespace CarRental.Menus
{
    public class Helper
    {
        /// <summary>
        /// Generic helper function for checking if a entered value is an integer.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>An integer value</returns>
        public static int checkInt(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                int integerValue;
                while (!Int32.TryParse(input, out integerValue))
                {
                    Console.WriteLine("Bitte eine gültige Zahl eingeben!");
                    input = Console.ReadLine();
                }
                return integerValue;
            }
            else
            {
                return 0;
            }

        }
    }
}
