using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MiniClimaMvc.Adapters;
using MiniClimaMvc.Controllers;
using MiniClimaMvc.Models;
using MiniClimaMvc.Views;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

string EjecutarFlujo(string ciudad, string unidad, bool lluvia)
{
    var modelo = new ClimaModel();
    var vista = new ClimaHtmlView();

    // OBSERVER: La vista se suscribe al modelo
    modelo.Suscribir(vista);
    var adapter = new OpenMeteoAdapter();
    var controlador = new ClimaController(modelo, adapter);

    // BUILDER: Construye la petición con lo enviado desde el formulario
    PeticionClima peticion = new PeticionClimaBuilder()
        .ConCiudad(ciudad)
        .ConUnidad(unidad)
        .ConPrecipitacion(lluvia)
        .Build();

    // CONTROLADOR: Ejecuta la consulta y notifica al modelo
    controlador.Consultar(peticion);

    return vista.HtmlGenerado;
}

// Carga inicial al entrar al sitio
app.MapGet("/", () =>
{
    string html = EjecutarFlujo("Santa Cruz", "celsius", true);
    return Results.Content(html, "text/html");
});

// Responde cuando pulsas el botón "Consultar Clima"
app.MapGet("/consultar", (string? ciudad, string? unidad, bool? lluvia) =>
{
    string html = EjecutarFlujo(
        string.IsNullOrWhiteSpace(ciudad) ? "Santa Cruz" : ciudad,
        unidad ?? "celsius",
        lluvia ?? true
    );
    return Results.Content(html, "text/html");
});

app.Run("http://localhost:5000");