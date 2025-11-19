namespace RocketMoonApp.Server.Models
{
    public class LaunchDto
    {
        public int Id { get; set; }
        public string RocketName { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
    }

    public class LocationDto
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}


