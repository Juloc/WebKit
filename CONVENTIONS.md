# Konventionen

## Benennung und Struktur

- Namespaces folgen dem Projekt und Verzeichnis: `WebKit.ExampleApp.Pages.Products` für Product-Pages.
- Öffentliche Typen sind vollständig benannt; kryptische Abkürzungen werden vermieden.
- Feature-spezifische Typen bleiben im Feature-Verzeichnis. Nur stabile, mehrfach genutzte Verträge wandern nach `WebKit.Core` oder `WebKit.Web`.
- PageModels verwenden `OnGet`, `OnPost` und benannte Handler wie `OnPostDelete` statt generischer Controller-Methoden.

## Fehler und Validierung

Erwartbare Anwendungsfehler werden als `Result<T>` zurückgegeben. Verwende stabile Codes aus `WebKitError` und zeige der UI eine verständliche Nachricht. Exceptions bleiben für unerwartete Infrastrukturfehler.

```csharp
Result<Order> result = service.Create(input);
if (result.IsFailure)
{
    ModelState.AddModelError(string.Empty, result.Error.Message);
    return Page();
}
```

Validierung sitzt nahe am Input-Modell, wird serverseitig ausgeführt und mit `ModelState.AddErrors(...)` an Razor zurückgegeben. Niemals nur HTML- oder JavaScript-Validierung als Sicherheitsgrenze verwenden.

## Sicherheit

- Jede mutierende Operation braucht den passenden Authorization-Check.
- `LocalUrl.IsSafe(...)` vor `LocalRedirect(...)` benutzen.
- Capability-Policies benennen die Fähigkeit, nicht die konkrete UI-Seite: `RequireCapability("billing.read")`.
- Formulare nutzen Anti-Forgery-Token; Razor Pages erzeugt diese für POST-Formulare standardmäßig.
- Ausgaben werden von Razor encodiert. `Html.Raw` nur nach expliziter Prüfung und Dokumentation verwenden.

## UI

Pages bauen aus semantischen Partials und Design-Tokens. Direkte Hex-Farben, zufällige Abstände und eigene Button-Varianten in einzelnen Pages sind zu vermeiden. Jede leere, fehlerhafte oder nicht berechtigte Ansicht erhält einen verständlichen Zustand.

## Codequalität

Nullable Reference Types und Warnings-as-errors bleiben aktiviert. Öffentliche APIs erhalten XML-Kommentare, wenn sie außerhalb der Anwendung verwendet werden. Änderungen sollen mit einem fokussierten Test und einer Dokumentationsanpassung kommen.
