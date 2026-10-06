# Design-System

`WebKit.Design` enthält die semantischen CSS-Tokens. `WebKit.UI` verwendet diese Tokens für die wiederverwendbaren Partials. Anwendungen dürfen die Werte überschreiben, sollen aber die Token-Namen stabil halten.

## Import-Reihenfolge

```html
<link rel="stylesheet" href="~/_content/WebKit.Design/css/webkit-design.css" />
<link rel="stylesheet" href="~/_content/WebKit.UI/css/webkit-ui.css" />
<link rel="stylesheet" href="~/css/site.css" />
```

Die Anwendung kommt zuletzt und enthält nur markenspezifische Anpassungen.

## Token-Regeln

- Farben werden über Rollen wie `--wk-color-surface`, `--wk-color-text` und `--wk-color-primary` verwendet, nicht über einzelne Hex-Werte.
- Abstände stammen aus `--wk-space-*`.
- Rundungen und Schatten verwenden `--wk-radius-*` und `--wk-shadow-*`.
- `prefers-color-scheme`, `data-theme="light"` und `data-theme="dark"` werden unterstützt.
- Fokusindikatoren bleiben sichtbar; `prefers-reduced-motion` deaktiviert dekorative Bewegungen.

## Komponenten

Verwende semantische Elemente (`header`, `main`, `nav`, `section`, `button`, `dialog`) und beschrifte Formfelder. Eine Statusfarbe darf nie die einzige Information sein; Text oder ein zugängliches Label muss den Zustand ebenfalls ausdrücken.

Neue Komponenten beginnen als Partial in `WebKit.UI/Pages/Shared`. Wenn sich nur ein einzelnes Produktfeature ändert, gehört die Darstellung in die Anwendung und nicht in das Kit.
