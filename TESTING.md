# Teststrategie

## Ebenen

1. **Core-Tests:** reine Tests für `Result<T>`, Fehlercodes, Pagination, Clock und Validierung.
2. **Web-Tests:** Tests für URL-Sicherheit, den zentralen `ICapabilityEvaluator`, Capability-Policies, Flash-Messages und ModelState-Adapter.
3. **Feature-Tests:** Tests der Domänen- und Anwendungsdienste mit Fakes oder In-Memory-Repositories.
4. **Package-Dogfood:** Packen aller vier Pakete, Installieren aller Templates und Build einer externen App aus dem Local Feed.
5. **HTTP-Smoke:** Start der externen App und Prüfung von `/` sowie der Design-/UI-Static-Web-Assets.

Das Repository enthält absichtlich einen paketfreien Test-Runner in `tests/WebKit.Tests`. Dadurch bleibt der Prüfpfad auch in eingeschränkten Umgebungen ohne NuGet-Zugriff ausführbar. Sobald eine Anwendung eine Testplattform nutzt, können die gleichen Assertions in xUnit, NUnit oder MSTest übernommen werden.

## Befehle

```powershell
dotnet build WebKit.sln -m:1
dotnet run --project tests/WebKit.Tests/WebKit.Tests.csproj
pwsh -File .\scripts\validate.ps1
```

Der Validator ist der kanonische Qualitätsweg und wird in GitHub Actions ausgeführt; die vier `.nupkg`-Dateien werden als CI-Artefakte hochgeladen. Neue Regeln sollen mindestens einen positiven und einen negativen Fall testen. Sicherheitsrelevante Tests prüfen insbesondere externe Redirects, fehlende Capabilities, ungültige Eingaben und nicht vorhandene Ressourcen.
