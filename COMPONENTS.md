# UI-Komponenten

Die UI-Schicht bietet kleine Razor-Partials mit expliziten View-Modellen:

| Partial | Zweck |
| --- | --- |
| `_PageHeader` | Titel, Beschreibung und optionale Aktionen |
| `_Breadcrumbs` | Semantische Seitenhierarchie |
| `_Navigation` | Shell-Navigation mit Gruppen und aktiver Route |
| `_EmptyState` | Keine Daten oder noch nicht konfigurierter Bereich |
| `_ErrorState` | Wiederholbarer oder allgemeiner Fehler |
| `_ForbiddenState` | Fehlende Berechtigung |
| `_LoadingState` | Zugänglicher Ladezustand |
| `_Alert` / `_ValidationSummary` | Inline-Hinweise und ModelState-Fehler |
| `_StatusBadge` | Textlicher Status mit semantischem Ton |
| `_Pagination` | Vor-/Zurück-Navigation mit sicherem Query-String |
| `_FactsList` / `_StatCard` | Detail-Metadaten und Dashboard-Kennzahlen |
| `_SettingsSection` | Erklärte Settings-Gruppe |
| `_ThemeControl` | Persistenter Theme-Preset-Schalter |
| `_ConfirmDialog` | Native, zugängliche Bestätigung |
| `_Toasts` | Typisierte, temporäre Flash-Messages mit Live-Region |

Beispiel:

```cshtml
<partial name="_PageHeader" model="new PageHeaderModel("Products", "Manage the catalog.")" />
<partial name="_EmptyState" model="new EmptyStateModel("No products", "Create the first product.", "New product", "/Products/Edit")" />
```

Partials enthalten keine Datenbankzugriffe. Sie erhalten fertig aufbereitete Modelle und bleiben damit in Storys, Tests und anderen Pages wiederverwendbar. Dialoge und POST-Formulare behalten Antiforgery-Verhalten; IDs und `aria-*`-Beziehungen werden pro Instanz erzeugt.
