using MiniClimaMvc.Models;

namespace MiniClimaMvc.Views;

public interface IClimaObserver
{
    void Actualizar(ClimaDatos datos);
}