using System.Diagnostics;
using System.Runtime.InteropServices;

// ============================================================================
// SECTION 1: Service Configuration (Dependency Injection Container)
// ============================================================================

/// <summary>
/// Erstellt den WebApplication Builder mit Standardkonfiguration.
/// Der Builder sammelt alle Services, die die App später braucht.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Registriert OpenAPI/Swagger für die API-Dokumentation.
/// Ermöglicht das automatische Generieren einer API-Beschreibung.
/// </summary>
builder.Services.AddOpenApi();

/// <summary>
/// Aktiviert Controller-basierte API-Endpoints.
/// Damit können wir [ApiController]-Klassen verwenden.
/// </summary>
builder.Services.AddControllers();

/// <summary>
/// Registriert den HttpClient-Service für ausgehende HTTP-Anfragen.
/// Wird gebraucht, wenn das Backend selbst APIs aufrufen muss.
/// </summary>
builder.Services.AddHttpClient();

/// <summary>
/// Konfiguriert CORS (Cross-Origin Resource Sharing).
/// Erlaubt dem Frontend (React auf Port 56142), das Backend anzusprechen.
/// Ohne CORS würde der Browser Anfragen von anderen Origins blockieren.
/// </summary>
builder.Services.AddCors(options => {
    options.AddPolicy("AllowLocalDev", p =>
        p.WithOrigins("https://localhost:56142")
         .AllowAnyHeader()
         .AllowAnyMethod());
});


// ============================================================================
// SECTION 2: Application Build & Browser Auto-Start
// ============================================================================

/// <summary>
/// Baut die WebApplication aus den konfigurierten Services.
/// Ab hier ist die Konfiguration abgeschlossen.
/// </summary>
var app = builder.Build();

/// <summary>
/// Konfiguriert den automatischen Browser-Start.
/// IHostApplicationLifetime gibt uns Events für den App-Lebenszyklus.
/// ApplicationStarted feuert, sobald der Server bereit ist, Requests zu empfangen.
/// </summary>
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
var url = "https://localhost:5001"; // Port anpassen!

lifetime.ApplicationStarted.Register(() =>
{
    OpenBrowser(url);
});


// ============================================================================
// SECTION 3: Middleware Pipeline Configuration
// ============================================================================

/// <summary>
/// Aktiviert Static File Serving mit optimiertem Asset-Handling.
/// Liefert CSS, JS, Bilder etc. aus dem wwwroot-Ordner.
/// </summary>
app.MapStaticAssets();

/// <summary>
/// Aktiviert OpenAPI nur in der Entwicklungsumgebung.
/// In Production wollen wir die API-Doku nicht öffentlich zeigen.
/// </summary>
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


// ============================================================================
// SECTION 4: API Endpoints
// ============================================================================

/// !!! Tatsächliche API Endpoints werden im definierten Controller stehen !!!


/// <summary>
/// Beispiel-Daten für den Weather-Endpoint.
/// </summary>
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", 
    "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

/// <summary>
/// Minimal API Endpoint: GET /weatherforecast
/// Gibt eine Liste von 5 zufälligen Wettervorhersagen zurück.
/// Dies ist ein Beispiel-Endpoint aus dem .NET Template.
/// </summary>
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");


// ============================================================================
// SECTION 5: Middleware Finalization & App Start
// ============================================================================

/// <summary>
/// Aktiviert die CORS-Policy für alle nachfolgenden Requests.
/// Muss vor MapControllers() aufgerufen werden.
/// </summary>
app.UseCors("AllowLocalDev");

/// <summary>
/// Mappt alle Controller-Endpoints (aus [ApiController]-Klassen).
/// Scannt automatisch alle Controller und registriert deren Routen.
/// </summary>
app.MapControllers();

/// <summary>
/// Fallback-Route für SPA (Single Page Application).
/// Alle nicht gematchten Routen liefern index.html zurück.
/// Das ermöglicht Client-Side Routing in React.
/// </summary>
app.MapFallbackToFile("/index.html");

/// <summary>
/// Startet den Kestrel-Webserver und blockiert bis zum Shutdown.
/// Ab hier nimmt die App Requests entgegen.
/// </summary>
app.Run();


// ============================================================================
// SECTION 6: Helper Methods & Models
// ============================================================================

/// <summary>
/// Öffnet die Standard-Browser-Anwendung mit der angegebenen URL.
/// Funktioniert plattformübergreifend (Windows, macOS, Linux).
/// </summary>
/// <param name="url">Die URL, die im Browser geöffnet werden soll.</param>
static void OpenBrowser(string url)
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        // Windows: Nutzt den "start"-Befehl der CMD
        Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") 
        { 
            CreateNoWindow = true  // Verhindert CMD-Fenster-Popup
        });
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
        // macOS: Nutzt den "open"-Befehl
        Process.Start("open", url);
    }
    else
    {
        // Linux: Nutzt xdg-open (Standard auf den meisten Distros)
        Process.Start("xdg-open", url);
    }
}

/// <summary>
/// Record für Wettervorhersage-Daten.
/// Records sind immutable Datentypen mit automatischer Equality.
/// </summary>
/// <param name="Date">Datum der Vorhersage</param>
/// <param name="TemperatureC">Temperatur in Celsius</param>
/// <param name="Summary">Beschreibung des Wetters</param>
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    /// <summary>
    /// Berechnet die Temperatur in Fahrenheit aus Celsius.
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}