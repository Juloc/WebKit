# WebKit

WebKit ist ein wiederverwendbares ASP.NET-Core/Razor Application Kit für interne Webanwendungen. Es liefert eine gemeinsame Grammatik für Shell, Navigation, List-, Detail-, Form-, Settings- und Admin-Seiten sowie Bausteine für Ergebnisse, Validierung, Suche/Filter, Pagination, Authentifizierung/Autorisierung, Flash-/Toast-Messages, responsive UI und ein zugängliches Design-System.

## Schnellstart

Voraussetzung ist das im Repository festgelegte .NET SDK (`global.json`).

```powershell
dotnet restore WebKit.sln -p:RestoreDisableParallel=true -m:1
dotnet build WebKit.sln -m:1 --no-restore
dotnet run --project samples/WebKit.ExampleApp/WebKit.ExampleApp.csproj
```

Die Beispielanwendung zeigt eine geschützte Produktliste, Form-Validierung, Pagination, Flash-Messages, Login, Capability-Autorisierung, Theme-Presets und die UI-Partials.

## Aufbau

| Projekt | Aufgabe |
| --- | --- |
| `Juloc.WebKit.Core` | Framework-unabhängige Fehler, `Result<T>`, Validierung, Zeit und Pagination |
| `Juloc.WebKit.Web` | ASP.NET-Core-Integration, User Context, Capability-Policy, Flash-Messages und URL-Sicherheit |
| `Juloc.WebKit.Design` | Semantische CSS-Tokens, Themes und responsives Grund-Stylesheet |
| `Juloc.WebKit.UI` | Razor-Partials, View-Modelle und UI-JavaScript |
| `WebKit.ExampleApp` | Ausführbare Referenz für vertikale Features und kanonische Page-Patterns |
| `WebKit.Tests` | Schnelle, paketfreie Smoke-/Unit-Tests für die Kernbausteine |

Neue Features liegen in der Anwendung unter `Features/<FeatureName>` und werden über ein kleines `IWebKitFeature`-Modul registriert. Die Page-Komposition ist in [PAGE_PATTERNS.md](PAGE_PATTERNS.md), die Architektur in [ARCHITECTURE.md](ARCHITECTURE.md) und die Agent-Regeln in [AGENTS.md](AGENTS.md) beschrieben.

## Wiederverwendung

Eine externe Razor-Anwendung verwendet die veröffentlichten Pakete `Juloc.WebKit.Core`, `Juloc.WebKit.Web`, `Juloc.WebKit.Design` und `Juloc.WebKit.UI`:

```xml
<PackageReference Include="Juloc.WebKit.Web" Version="0.2.0" />
<PackageReference Include="Juloc.WebKit.UI" Version="0.2.0" />
<PackageReference Include="Juloc.WebKit.Design" Version="0.2.0" />
```

Die UI- und Design-Assets werden über Static-Web-Asset-Pfade eingebunden:

```html
<link rel="stylesheet" href="~/_content/Juloc.WebKit.Design/css/webkit-design.css" />
<link rel="stylesheet" href="~/_content/Juloc.WebKit.UI/css/webkit-ui.css" />
<script src="~/_content/Juloc.WebKit.UI/js/webkit-ui.js" defer></script>
```

Die Vorlagen unter `templates/` sind bewusst klein gehalten und können mit `dotnet new` installiert oder als Startpunkt kopiert werden. Details und der externe Local-Feed-Dogfood-Test stehen in [TEMPLATES.md](TEMPLATES.md).

## Prüfen

Der vollständige lokale Prüfpfad restauriert, baut, testet, packt, installiert alle Templates, kompiliert eine externe Anwendung aus dem Local Feed und prüft deren HTTP- sowie Static-Web-Assets:

```powershell
pwsh -File .\scripts\validate.ps1
# Windows PowerShell ohne angepasste Execution Policy:
powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\scripts\validate.ps1
```

Der Pfad läuft in GitHub Actions; die vier `.nupkg`-Dateien werden als CI-Artefakte hochgeladen. Konventionen, Design-System, Komponenten, Testansatz und Quality Audit stehen in [CONVENTIONS.md](CONVENTIONS.md), [DESIGN_SYSTEM.md](DESIGN_SYSTEM.md), [COMPONENTS.md](COMPONENTS.md), [TESTING.md](TESTING.md) und [QUALITY_AUDIT.md](QUALITY_AUDIT.md).
