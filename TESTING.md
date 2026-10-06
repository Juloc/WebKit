# Teststrategie

## Ebenen

1. **Core-Tests:** reine Tests für `Result<T>`, Fehlercodes, Pagination, Clock und Validierung.
2. **Web-Tests:** Tests für URL-Sicherheit, Capability-Policies, Flash-Messages und ModelState-Adapter.
3. **Feature-Tests:** Tests der Domänen- und Anwendungsdienste mit fakes oder In-Memory-Repositories.
4. **Smoke-Test:** Start der Beispielanwendung und Prüfung der wichtigsten Razor-Pages über HTTP.

Das Repository enthält absichtlich einen paketfreien Test-Runner in `tests/WebKit.Tests`. Dadurch bleibt der Prüfpfad auch in eingeschränkten Umgebungen ohne NuGet-Zugriff ausführbar. Sobald eine Anwendung eine Testplattform nutzt, können die gleichen Assertions in xUnit, NUnit oder MSTest übernommen werden.

## Befehle

```powershell
dotnet build WebKit.sln -m:1
dotnet run --project tests/WebKit.Tests/WebKit.Tests.csproj
.\scripts\validate.ps1
```

Neue Regeln sollen mindestens einen positiven und einen negativen Fall testen. Sicherheitsrelevante Tests prüfen insbesondere externe Redirects, fehlende Capabilities, ungültige Eingaben und nicht vorhandene Ressourcen.
