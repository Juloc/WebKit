# UI-Komponenten

Die UI-Schicht bietet kleine Razor-Partials mit expliziten View-Modellen:

| Partial | Zweck |
| --- | --- |
| `_PageHeader` | Titel, Beschreibung und optionale Aktionen |
| `_EmptyState` | Keine Daten oder noch nicht konfigurierter Bereich |
| `_ErrorState` | Wiederholbarer oder allgemeiner Fehler |
| `_ForbiddenState` | Fehlende Berechtigung |
| `_StatusBadge` | Textlicher Status mit semantischem Ton |
| `_Pagination` | Vor-/Zurück-Navigation mit sicherem Query-String |
| `_ConfirmDialog` | Native, zugängliche Bestätigung |
| `_Toasts` | Temporäre Flash-Messages |

Beispiel:

```cshtml
<partial name="_PageHeader" model="new PageHeaderModel("Products", "Manage the catalog.")" />
<partial name="_EmptyState" model="new EmptyStateModel("No products", "Create the first product.", "New product", "/Products/Edit")" />
```

Partials enthalten keine Datenbankzugriffe. Sie erhalten fertig aufbereitete Modelle und bleiben damit in Storys, Tests und anderen Pages wiederverwendbar.
