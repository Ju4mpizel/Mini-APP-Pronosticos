namespace MiniClimaMvc.Models;

public class ClimaDatos
{
    public string Ciudad { get; set; } = string.Empty;
    public double Temperatura { get; set; }
    public string SimboloUnidad { get; set; } = "°C";
    public int Humedad { get; set; }
    public bool VaALlover { get; set; }
}