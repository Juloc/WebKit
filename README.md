# WebKit

WebKit ist ein kleines, bewusst langweiliges ASP.NET-Core/Razor-Fundament für interne Webanwendungen. Es liefert wiederverwendbare Bausteine für Ergebnisse, Validierung, Pagination, Authentifizierung/Autorisierung, Flash-Messages, Razor-Partials und ein zugängliches Design-System.

## Schnellstart

Voraussetzung ist das im Repository festgelegte .NET SDK (`global.json`).

```powershell
dotnet restore src/WebKit.Core/WebKit.Core.csproj
dotnet restore src/WebKit.Design/WebKit.Design.csproj
dotnet restore src/WebKit.Web/WebKit.Web.csproj
dotnet restore src/WebKit.UI/WebKit.UI.csproj
dotnet restore samples/WebKit.ExampleApp/WebKit.ExampleApp.csproj
dotnet restore tests/WebKit.Tests/WebKit.Tests.csproj
dotnet build WebKit.sln -m:1 --no-restore
dotnet run --project samples/WebKit.ExampleApp/WebKit.ExampleApp.csproj
```

Danach ist die Beispielanwendung unter der von ASP.NET Core ausgegebenen URL erreichbar. Die Beispielanwendung zeigt eine geschützte Produktliste, Form-Validierung, Pagination, Flash-Messages, Login, Capability-Autorisierung und die UI-Partials.

## Aufbau

| Projekt | Aufgabe |
| --- | --- |
| `WebKit.Core` | Framework-unabhängige Fehler, `Result<T>`, Validierung, Zeit und Pagination |
| `WebKit.Web` | ASP.NET-Core-Integration, User Context, Capability-Policy, Flash-Messages und URL-Sicherheit |
| `WebKit.Design` | Semantische CSS-Tokens und responsives Grund-Stylesheet |
| `WebKit.UI` | Razor-Partials, View-Modelle und UI-JavaScript |
| `WebKit.ExampleApp` | Ausführbare Referenz für eine vertikale Feature-Struktur |
| `WebKit.Tests` | Schnelle, paketfreie Smoke-/Unit-Tests für die Kernbausteine |

Neue Features liegen in der Anwendung unter `Features/<FeatureName>` und werden über ein kleines `IWebKitFeature`-Modul registriert. Die komplette Architektur ist in [ARCHITECTURE.md](ARCHITECTURE.md) beschrieben.

## Wiederverwendung

```csharp
builder.Services.AddWebKitWeb(options => options.CapabilityClaimType = "capability");
builder.Services.AddWebKitFeatures(new BillingFeature(), new CatalogFeature());
```

In einer Razor-Anwendung werden die UI- und Design-Assets über die Static-Web-Asset-Pfade eingebunden:

```html
<link rel="stylesheet" href="~/_content/WebKit.Design/css/webkit-design.css" />
<link rel="stylesheet" href="~/_content/WebKit.UI/css/webkit-ui.css" />
<script src="~/_content/WebKit.UI/js/webkit-ui.js" defer></script>
```

Die Vorlagen unter `templates/` sind bewusst klein gehalten und können mit `dotnet new` installiert oder als Startpunkt kopiert werden. Details stehen in [TEMPLATES.md](TEMPLATES.md).

## Prüfen

Der vollständige lokale Prüfpfad ist:

```powershell
.\scripts\validate.ps1
```

Die Konventionen, das Design-System und der Testansatz sind in [CONVENTIONS.md](CONVENTIONS.md), [DESIGN_SYSTEM.md](DESIGN_SYSTEM.md), [COMPONENTS.md](COMPONENTS.md) und [TESTING.md](TESTING.md) dokumentiert.
