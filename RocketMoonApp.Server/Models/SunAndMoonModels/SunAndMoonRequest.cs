namespace RocketMoonApp.Server.Models.SunAndMoonModels
{
    public class SunAndMoonRequest
    {
        public DateTime Date { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public float? TimeZone { get; set; }
        public bool? DST { get; set; }
    }
}
