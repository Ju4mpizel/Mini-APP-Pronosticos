namespace MiniClimaMvc.Models;

public class PeticionClima
{
    public string Ciudad { get; set; } = string.Empty;
    public string Unidad { get; set; } = "celsius";
    public bool IncluirPrecipitacion { get; set; } = true;
}