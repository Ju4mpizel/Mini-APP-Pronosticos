namespace MiniClimaMvc.Models;

public class PeticionClimaBuilder
{
    private readonly PeticionClima _peticion = new();

    public PeticionClimaBuilder ConCiudad(string ciudad)
    {
        _peticion.Ciudad = string.IsNullOrWhiteSpace(ciudad) ? "Ciudad desconocida" : ciudad.Trim();
        return this;
    }

    public PeticionClimaBuilder ConUnidad(string unidad)
    {
        _peticion.Unidad = unidad.ToLower() == "fahrenheit" ? "fahrenheit" : "celsius";
        return this;
    }

    public PeticionClimaBuilder ConPrecipitacion(bool incluir)
    {
        _peticion.IncluirPrecipitacion = incluir;
        return this;
    }

    public PeticionClima Build() => _peticion;
}