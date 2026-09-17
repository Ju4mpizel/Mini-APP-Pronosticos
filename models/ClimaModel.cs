using System.Collections.Generic;
using MiniClimaMvc.Views;

namespace MiniClimaMvc.Models;

public class ClimaModel
{
    private readonly List<IClimaObserver> _observadores = new();
    public ClimaDatos? EstadoActual { get; private set; }

    public void Suscribir(IClimaObserver observador) => _observadores.Add(observador);
    public void Desuscribir(IClimaObserver observador) => _observadores.Remove(observador);

    public void ActualizarClima(ClimaDatos nuevosDatos)
    {
        EstadoActual = nuevosDatos;
        Notificar();
    }

    private void Notificar()
    {
        if (EstadoActual == null) return;
        foreach (var obs in _observadores)
        {
            obs.Actualizar(EstadoActual);
        }
    }
}