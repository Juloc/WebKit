# Referenz-Pattern aus den bestehenden Anwendungen

Die Analyse erfolgte read-only. `Paper` war in den verfügbaren Pfaden (`E:\git`, `E:\git\FW`) nicht vorhanden; deshalb gibt es für Paper keine behauptete Analyse. Zusätzlich wurde `E:\git\Finance` als verwandte Finanz-Referenz gesichtet. Die Lücke bleibt im Abschlussbericht offen, bis ein gültiger Paper-Pfad bereitsteht.

## Jularr

- Serverseitige ASP.NET-Anwendung mit vielen fokussierten Page-/Render-Tests.
- Admin-, Dashboard-, Detail-, Listen- und Settings-Seiten als wiederkehrende Formen.
- Capability- und Accessibility-Tests als eigene Qualitätsgrenzen.
- Medienprodukt mit dichter Navigation, daher Shell-Komposition statt einer universellen Shell.

## FullWorth

- Große, fachlich getrennte Feature-Asset-Struktur unter `wwwroot/features`.
- Globale UI-Module unter `wwwroot/ui` und getrennte Theme-/Appearance-Dateien.
- Eigene Dialog-, Confirm-, Mobile-Review-, Passkey- und Capability-UI-Patterns.
- Starke Sicherheits-, Antiforgery-, Header- und Rate-Limit-Grenzen.

## Finance

- Gleiche serverseitige Finanz-Problemform wie FullWorth.
- Feature-spezifische CSS-/JS-Dateien statt eines erzwungenen globalen Feature-Bundles.
- Tests für Accessibility, Responsive Layout, Theme-Parität, Auth-UI und HTTP-Smoke.

## WebKit-Standardisierung

WebKit übernimmt nur die gemeinsame Grammatik: Shell-Primitives, Page-Komposition, Form-/Validation-Struktur, Status-/State-Komponenten, Search/Filter/Pagination, typed Toasts, Theme-Tokens, responsive/accessibility Defaults und eine klare Page-Asset-Konvention. Domain-Karten, Finanzlogik, Media-Player, Charts und Produktnavigation bleiben Consumer-Code.
