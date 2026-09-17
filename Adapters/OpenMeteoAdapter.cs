using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using MiniClimaMvc.Models;

namespace MiniClimaMvc.Adapters;

public class OpenMeteoAdapter : IClimaAdapter
{
    // Reutilizamos un único HttpClient según las buenas prácticas de .NET
    private static readonly HttpClient _httpClient = new();

    public ClimaDatos ObtenerClima(PeticionClima peticion)
    {
        try
        {
            // -------------------------------------------------------------
            // 1. LLAMADA A LA API DE GEOCODIFICACIÓN
            // Open-Meteo requiere latitud y longitud. Primero buscamos las
            // coordenadas de la ciudad que el usuario escribió en el input.
            // -------------------------------------------------------------
            string urlGeo = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(peticion.Ciudad)}&count=1&language=es&format=json";
            
            string jsonGeo = _httpClient.GetStringAsync(urlGeo).GetAwaiter().GetResult();
            using var docGeo = JsonDocument.Parse(jsonGeo);
            var rootGeo = docGeo.RootElement;

            // Verificamos si la API reconoció la ciudad
            if (!rootGeo.TryGetProperty("results", out var resultados) || resultados.GetArrayLength() == 0)
            {
                return new ClimaDatos
                {
                    Ciudad = $"{peticion.Ciudad} (No encontrada)",
                    Temperatura = 0,
                    SimboloUnidad = peticion.Unidad == "fahrenheit" ? "°F" : "°C",
                    Humedad = 0,
                    VaALlover = false
                };
            }

            var primerResultado = resultados[0];
            double latitud = primerResultado.GetProperty("latitude").GetDouble();
            double longitud = primerResultado.GetProperty("longitude").GetDouble();
            string nombreOficial = primerResultado.GetProperty("name").GetString() ?? peticion.Ciudad;

            // -------------------------------------------------------------
            // 2. LLAMADA A LA API DE CLIMA (Forecast API)
            // Pedimos temperatura actual, humedad y precipitación en tiempo real.
            // -------------------------------------------------------------
            string unidadUrl = peticion.Unidad == "fahrenheit" ? "fahrenheit" : "celsius";
            string sLat = latitud.ToString(CultureInfo.InvariantCulture);
            string sLon = longitud.ToString(CultureInfo.InvariantCulture);

            string urlClima = $"https://api.open-meteo.com/v1/forecast?latitude={sLat}&longitude={sLon}&current=temperature_2m,relative_humidity_2m,precipitation&temperature_unit={unidadUrl}";

            string jsonClima = _httpClient.GetStringAsync(urlClima).GetAwaiter().GetResult();
            using var docClima = JsonDocument.Parse(jsonClima);
            var actual = docClima.RootElement.GetProperty("current");

            // -------------------------------------------------------------
            // 3. EL PATRÓN ADAPTER EN ACCIÓN:
            // Traduce el JSON externo (con nombres técnicos en inglés) 
            // al formato limpio que usa nuestro sistema (ClimaDatos).
            // -------------------------------------------------------------
            double tempApi = actual.GetProperty("temperature_2m").GetDouble();
            int humedadApi = actual.GetProperty("relative_humidity_2m").GetInt32();
            double precipitacionMm = actual.GetProperty("precipitation").GetDouble();

            return new ClimaDatos
            {
                Ciudad = nombreOficial,
                Temperatura = Math.Round(tempApi, 1),
                SimboloUnidad = peticion.Unidad == "fahrenheit" ? "°F" : "°C",
                Humedad = humedadApi,
                // Si llueve más de 0.1 mm y el usuario pidió incluir lluvia, activamos la alerta
                VaALlover = peticion.IncluirPrecipitacion && (precipitacionMm > 0.0)
            };
        }
        catch (Exception)
        {
            // Manejo en caso de fallo de red o problema de conexión
            return new ClimaDatos
            {
                Ciudad = $"{peticion.Ciudad} (Error de conexión)",
                Temperatura = 0,
                SimboloUnidad = peticion.Unidad == "fahrenheit" ? "°F" : "°C",
                Humedad = 0,
                VaALlover = false
            };
        }
    }
}