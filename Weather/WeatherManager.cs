using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather
{
    class WeatherManager
    {
        private static string WEATHER_API_KEY = "5b74487769c14b7a7eee866bb9087117";

        private static ApiHandler handler = new ApiHandler();
        public List<string> Cities { get; set; } = new List<string>();
        public List<WeatherSimplified> WeatherList { get; set; } = new List<WeatherSimplified>();

        public WeatherManager()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void GetCities()
        {
            var data = handler.GetAllCitiesAsync();
            data.Wait();
            Cities = data.Result;
        }

        private void ShuffleCities()
        {
            Random rng = new Random();
            Cities = Cities.OrderBy(a => rng.Next()).ToList();
        }

        private bool TryToGetWeather(int startIndex)
        {
            List<Task<WeatherModel>> WeatherModelsTasks = new List<Task<WeatherModel>>();

            for(var i = startIndex; i < startIndex+150; i++)
            {
                WeatherModelsTasks.Add(handler.GetCurrentWeather(Cities[i], WEATHER_API_KEY));
            }

            TimeSpan ts = TimeSpan.FromMilliseconds(500);
            foreach(var task in WeatherModelsTasks)
            {
                task.Wait(ts);
            }

            int notNullCounts = 0;
            foreach(var weather in WeatherModelsTasks)
            {
                if(weather.Result != null)
                {
                    WeatherList.Add(weather.Result.Simplified());
                    notNullCounts++; 
                }

                if(WeatherList.Count == 100)
                {
                    return true;
                }
            }
            return false;
        }

        /**
        approx 10% of cities do not have info about weather
        this function downloads data from 150 cities at the same time in a loop
        there are 150 cities in one batch to make sure that
        its almost certain that it will find 100 cities with weather info in the first iteration
        the process repeats until it finds the first 100 cities
        */
        public void GetWeatherForRandomCities()
        {
            if(Cities.Count == 0)
            {
                GetCities();
            }
            // shuffle list and take first 100 elements
            WeatherList.Clear();
            ShuffleCities();

            int startIndex = 0;

            while(!TryToGetWeather(startIndex))
            {
                startIndex++;
            }
        }

        public void ShowWeather()
        {
            if(WeatherList.Count < 100)
            {
                Console.WriteLine("Not enough data");
                return;
            }

            Console.WriteLine(String.Format(" {0,3} {1,34} | {2,9} | {3,9} | {4,9} | {5,20}",
                "No.", "Name", "Temp", "Pressure", "Humidity", "Description"));
            Console.WriteLine(new String('-', 97));
            for(var i = 0; i < 100; i++)
            {
                Console.WriteLine(String.Format("{0,3}. ",i+1) + WeatherList[i]);
            }
        }

        public void SaveWeather(string filename)
        {
            if(WeatherList.Count == 0)
            {
                Console.WriteLine("No data to be saved");
                return;
            }

            // creates a handler for an output file and delete it if exists
            FileInfo file = new FileInfo(filename);
            if (file.Exists) file.Delete();

            using var excelPackage = new ExcelPackage(file);

            var sheet = excelPackage.Workbook.Worksheets.Add("WeatherReport");

            var range = sheet.Cells["A1"].LoadFromCollection(WeatherList, true);
            range.AutoFitColumns();

            excelPackage.Save();
        }


    }
}
