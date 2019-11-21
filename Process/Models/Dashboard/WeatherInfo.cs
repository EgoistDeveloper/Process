using System;
using System.Collections.Generic;

namespace Process.Models.Dashboard
{
    public class WeatherInfo
    {
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public string Weather { get; set; }
        public Description Description { get; set; }
        public List<List> Temperatures { get; set; } = new List<List>();
    }
}
