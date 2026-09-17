using MiniClimaMvc.Adapters;
using MiniClimaMvc.Models;

namespace MiniClimaMvc.Controllers;

public class ClimaController
{
    private readonly ClimaModel _modelo;
    private readonly IClimaAdapter _adapter;

    public ClimaController(ClimaModel modelo, IClimaAdapter adapter)
    {
        _modelo = modelo;
        _adapter = adapter;
    }

    public void Consultar(PeticionClima peticion)
    {
        // 1. Llama al Adapter
        ClimaDatos datos = _adapter.ObtenerClima(peticion);

        // 2. Notifica al Modelo (lo que actualiza la Vista mediante el Observer)
        _modelo.ActualizarClima(datos);
    }
}