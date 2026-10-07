# Quality audit

Der kanonische Qualitätsweg ist `scripts/validate.ps1` und wird lokal sowie in GitHub Actions ausgeführt.

Die automatisierte Contract-Prüfung deckt ab:

- sichtbaren Fokus, reduzierte Bewegung und responsive Breakpoints im Design-Stylesheet;
- semantische Dialog-Beschriftung und Antiforgery im Confirm-Dialog;
- `role`/`aria-live` für typisierte Toasts;
- gemeinsame Button-, Form- und Dialog-Primitives;
- kleines globales UI-JavaScript ohne globale Netzwerkaufrufe;
- HTTP-Smoke und Static-Web-Assets der ExampleApp und einer externen Package-App.

Responsive und Accessibility bleiben zusätzlich Consumer-Verantwortung: fachliche Inhalte brauchen korrekte Labels, sinnvolle Überschriften, ausreichenden Kontrast und Tastaturbedienung. Page-spezifisches CSS/JS wird über `@section Styles` und `@section Scripts` geladen; globale Bundles für einzelne Features sind nicht vorgesehen.

Der Audit ist bewusst ein statischer und reproduzierbarer Contract-Check, kein Ersatz für einen späteren Browser-/Screenreader-Test mit den konkreten Consumer-Daten.
