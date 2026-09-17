using MiniClimaMvc.Models;

namespace MiniClimaMvc.Views;

public class ClimaHtmlView : IClimaObserver
{
    public string HtmlGenerado { get; private set; } = string.Empty;

    public void Actualizar(ClimaDatos datos)
    {
        string badgeLluvia = datos.VaALlover
            ? "<span style='display:inline-block; font-size:0.8rem; font-weight:600; padding:6px 12px; border-radius:9999px; background:#fef2f2; color:#dc2626;'>🌧️ Lluvia prevista</span>"
            : "<span style='display:inline-block; font-size:0.8rem; font-weight:600; padding:6px 12px; border-radius:9999px; background:#f0fdf4; color:#16a34a;'>☀️ Despejado</span>";

        HtmlGenerado = $$"""
        <!DOCTYPE html>
        <html lang="es">
        <head>
          <meta charset="UTF-8">
          <meta name="viewport" content="width=device-width, initial-scale=1.0">
          <title>Mini Clima MVC</title>
        </head>
        <body style="margin:0; padding:40px 16px; background-color:#f1f5f9; font-family:-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; display:flex; justify-content:center; align-items:center; min-height:85vh;">
          <div style="width:100%; max-width:380px; background:#ffffff; border-radius:20px; padding:28px; box-shadow:0 10px 30px rgba(0,0,0,0.06); border:1px solid #e2e8f0; box-sizing:border-box;">
            <h1 style="font-size:1.3rem; margin:0 0 20px 0; color:#0f172a; font-weight:700;">🌦️ Pronóstico Minimalista</h1>

            <form action="/consultar" method="GET">
              <div style="margin-bottom:16px;">
                <label style="display:block; font-size:0.82rem; font-weight:600; color:#64748b; margin-bottom:6px;">Ciudad</label>
                <input type="text" name="ciudad" value="{{datos.Ciudad}}" placeholder="Ej. La Paz, Santa Cruz, Cochabamba" required style="width:100%; box-sizing:border-box; padding:11px 14px; border:1px solid #cbd5e1; border-radius:10px; font-size:0.95rem; outline:none;" />
              </div>

              <details open style="border:1px solid #e2e8f0; border-radius:10px; padding:10px 14px; margin-bottom:16px; background:#f8fafc;">
                <summary style="font-size:0.85rem; font-weight:600; color:#0f172a; cursor:pointer;">Unidad de temperatura</summary>
                <div style="display:flex; gap:16px; margin-top:10px; font-size:0.85rem;">
                  <label><input type="radio" name="unidad" value="celsius" {{(datos.SimboloUnidad == "°C" ? "checked" : "")}}> Celsius (°C)</label>
                  <label><input type="radio" name="unidad" value="fahrenheit" {{(datos.SimboloUnidad == "°F" ? "checked" : "")}}> Fahrenheit (°F)</label>
                </div>
              </details>

              <div style="display:flex; justify-content:space-between; align-items:center; margin-bottom:20px;">
                <label style="font-size:0.85rem; color:#475569;">¿Incluir pronóstico de lluvia?</label>
                <select name="lluvia" style="padding:6px 12px; border-radius:8px; border:1px solid #cbd5e1;">
                <option value="true" {{(datos.VaALlover ? "selected" : "")}}>Sí</option>
                <option value="false" {{(!datos.VaALlover ? "selected" : "")}}>No</option>
                </select>
              </div>

              <button type="submit" style="width:100%; background:#2563eb; color:#ffffff; border:none; padding:12px; font-size:0.95rem; font-weight:600; border-radius:10px; cursor:pointer;">Consultar Clima</button>
            </form>

            <div style="margin-top:24px; padding-top:20px; border-top:1px solid #e2e8f0;">
              <div style="display:flex; justify-content:space-between; align-items:baseline;">
                <span style="font-weight:700; font-size:1.2rem; color:#0f172a;">{{datos.Ciudad}}</span>
                {{badgeLluvia}}
              </div>
              <div style="font-size:3.2rem; font-weight:800; color:#2563eb; margin:10px 0; letter-spacing:-0.04em;">{{datos.Temperatura}}{{datos.SimboloUnidad}}</div>
              <div style="font-size:0.85rem; color:#64748b;">💧 Humedad relativa: <strong style="color:#1e293b;">{{datos.Humedad}}%</strong></div>
            </div>
          </div>
        </body>
        </html>
        """;
    }
}