# Referenzhierarchie und Bedarfsermittlung

Die Referenzprojekte wurden ausschließlich read-only untersucht. Die Schreibgrenze gilt
weiterhin strikt: Nur `WebKit` darf verändert werden. `Jularr`, `FullWorth` und `Paper`
werden nicht editiert, getestet, refaktoriert oder committed.

## Entscheidungsregel

`Jularr` ist die einzige qualitative Source of Truth. Wenn WebKit entscheidet, wie ein
Pattern umgesetzt wird, gelten zuerst Jularrs Architektur, Page-Struktur, Shell,
Navigation, UX-Grammatik, Design-System-, Responsive- und Accessibility-Prinzipien.

`FullWorth` und `Paper` sind ausschließlich Bedarfsermittlung. Ihre konkreten
Implementierungen werden nicht kopiert. Aus ihnen darf nur ein domain-neutraler Bedarf
abgeleitet werden; die WebKit-Lösung muss danach der Jularr-/WebKit-Qualitätslinie folgen.

Wenn ein Bedarf bereits durch WebKit abgedeckt ist, entsteht keine zusätzliche
Komponente. Wenn ein Bedarf fehlt, wird die kleinste domain-neutrale Primitive ergänzt.

## Jularr — qualitative Source of Truth

Die read-only Analyse von Jularr zeigt folgende Qualitäts- und Strukturprinzipien:

- Die App-Shell ist aus kleinen Verantwortungen zusammengesetzt: `_AppNavigation`,
  `_AppHeader`, `_AppUserMenu`, `_AppThemeControl`, Breadcrumbs und einer eigenen
  Mobile-Navigation.
- Aktive Navigation, Breadcrumbs und `aria-current` werden serverseitig im initialen
  Markup gesetzt; die Seite ist dadurch ohne nachträgliches JavaScript verständlich.
- Admin- und Settings-Bereiche besitzen eigene Navigation und bleiben capability- bzw.
  kontextabhängig. Nicht sichtbare Bereiche werden nicht lediglich deaktiviert angezeigt.
- Wiederverwendbare Fakten-, Metric-, Status-, Empty-, Validation- und Error-Partials
  halten die Page-Struktur domainnah, ohne die Shell mit Fachlogik zu belasten.
- Page-spezifische Styles und Scripts werden über Razor-Sections geladen. Globale Shell-
  Assets bleiben auf globale Aufgaben begrenzt.
- Responsive Verhalten, Skip-Link, sichtbarer Fokus, semantische Navigation,
  Reduced-Motion-Unterstützung und mobile Touch-Navigation sind Teil der Grundqualität.

Diese Prinzipien bestimmen WebKits gemeinsame UI-Grammatik. Branding, konkrete Farben,
Produktnavigation und Fachkarten bleiben im Consumer.

## FullWorth — ausschließlich Bedarfsermittlung

FullWorth zeigt als Bedarf, dass eine dokumentierte Razor-Anwendung zusätzlich folgende
Fälle abdecken können muss:

- eine Shell kann Sidebar, Topbar und eine separate mobile Schnellnavigation benötigen;
- aktive Navigation muss serverseitig im ersten Render korrekt markiert sein;
- Theme-Initialisierung, responsive Layouts, Dialoge und Privacy-/sensitive-value-
  Aktionen brauchen klare Accessibility-Verträge;
- Seiten laden ihre eigenen Feature-Assets über Styles-/Scripts-Sections statt alle
  Feature-CSS-/JS-Dateien global zu laden;
- Listen, Dashboards, Settings, Formulare, Status-/Feedbackzustände und autorisierte
  Admin-Bereiche müssen als wiederkehrende Seitenformen unterstützt werden.

WebKit deckt diese generischen Anforderungen bereits über `_Navigation`, Theme- und
Design-Tokens, Page-Header/Breadcrumbs, Facts-/Stat-/Settings-Partials, Dialog-,
Toast-, State-, Search-/Toolbar- und Pagination-Verträge sowie die Template- und
`@section Styles`/`@section Scripts`-Konvention ab. Eine finanzspezifische Privacy-
Implementierung, Finanzlogik, Coach-UI, Bankzugriff oder FullWorth-Navigation gehört
nicht in WebKit.

## Paper — ausschließlich Bedarfsermittlung

In den verfügbaren Arbeitsverzeichnissen wurde kein Paper-Git-Repository mit einem
verifizierbaren Projektpfad oder Remote gefunden. Die vorhandenen `paper`-Ordner unter
Minecraft-Bibliotheken sind keine Paper-Referenzanwendung und werden nicht als solche
interpretiert.

Damit gibt es für Paper keine behauptete qualitative oder fachliche Analyse. Das ist
kein Abschlussblocker: Paper darf nur zusätzliche Anforderungen liefern, nicht die
Qualitätsentscheidung bestimmen. Die aktuell bekannten generischen Fälle — einfache
Topbar, Liste, Dokument-Detail, Upload-/Form-Seite, Search, Settings, Status und
Pagination — sind durch die vorhandenen WebKit-Seitenformen und UI-Partials abbildbar.

## WebKit-Entscheidung

WebKit standardisiert nur die gemeinsame, domain-neutrale Grammatik:

- Shell- und Navigation-Primitives statt drei fertiger Produktshells;
- List-, Detail-, Form-, Settings-, Admin- und Dashboard-Kompositionen;
- semantische Design-Tokens mit consumer-spezifischen Presets;
- serverseitige Capability-Grenzen, sichere Return-URLs und Antiforgery;
- Search/Filter/Pagination mit erhaltenen Query-Parametern;
- Loading-, Empty-, Error-, Forbidden-, Validation-, Status-, Toast- und Confirm-Zustände;
- responsive und accessible Defaults mit page-lokalen Assets;
- kleine Razor-Templates und Agent-Konventionen für neue Features.

Nicht in WebKit gehören Domainlogik, Finanztransaktionen, Banking-Privacy-Modus,
Dokument-OCR, Shelf-/Media-/Anime-Navigation, Player, Coach, Charts oder
produktspezifische Dashboard-Metriken.
