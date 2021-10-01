using System;
using System.Collections.Generic;

namespace Weather
{
    class Program
    {
        private static string OUTPUT_FILENAME = "./output.xlsx";
        static void Main(string[] args)
        {

            WeatherManager manager = new WeatherManager();

            // Gets all cities and the current weather for 100 randomly choosen cities
            manager.GetWeatherForRandomCities();

            // prints the weather table
            manager.ShowWeather();

            // saves weather info as a xlsx file
            // the output file should be in Weather/bin/Debug/ (same as exe file)
            manager.SaveWeather(OUTPUT_FILENAME);
        }
    }
}
