using System;
using System.Collections.Generic;
using System.Text;

namespace Weather
{
    // Main class with all information about the weather in given city
    class WeatherSimplified
    {
        public string Name { get; set; }
        public float Temp { get; set; }
        public float Pressure { get; set; }
        public float Humidity { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return String.Format("{0,34} | {1,7}°C | {2,9} | {3,9} | {4,20}", 
                Name, Temp, Pressure, Humidity, Description);
        }
    }
    
    class MainInfo
    {
        public float Temp { get; set; }
        public float Pressure { get; set; }
        public float Humidity { get; set; }

    }

    class SkyInfo
    {
        public string Description { get; set; }
    }

    // class for getting the data from API 
    // Since the data are in different lists and objects in the json, there are 3 classes for matching the json API
    class WeatherModel
    {
        public string Name { get; set; }
        public MainInfo Main { get; set; }
        public List<SkyInfo> Weather { get; set; }

        public WeatherSimplified Simplified()
        {
            return new WeatherSimplified
            {
                Name = this.Name,
                Temp = Main.Temp,
                Pressure = Main.Pressure,
                Humidity = Main.Humidity,
                Description = Weather[0].Description
            };
        }
    }
}
