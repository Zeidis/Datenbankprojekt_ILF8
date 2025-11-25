namespace RocketMoonApp.Server.Models.MoonModels
{
    /// <summary>
    /// Repräsentiert geografische Koordinaten bestehend aus Breitengrad (Latitude)
    /// und Längengrad (Longitude).
    /// </summary>
    /// <remarks>
    /// Das Objekt dient zur Speicherung und Ausgabe von Koordinatenwerten.
    /// Die Methode <c>ToString()</c> gibt die Koordinaten im Format
    /// "Latitude,Longitude" unter Verwendung der invariant culture zurück.
    /// </remarks>
    public class Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override string ToString()
        {
            return string.Create(System.Globalization.CultureInfo.InvariantCulture, $"{Latitude},{Longitude}");
        }
    }
}
