using System;
using System.Collections.Generic;
using System.Text;

namespace Weather
{
    class CountryModel
    {
        public string Country { get; set; }
        public List<string> Cities { get; set; }
    }


    class CountriesDataListModel
    {
        public List<CountryModel> Data { get; set; }
     }
}
