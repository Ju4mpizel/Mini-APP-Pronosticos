using MiniClimaMvc.Models;

namespace MiniClimaMvc.Adapters;

public interface IClimaAdapter
{
    ClimaDatos ObtenerClima(PeticionClima peticion);
}