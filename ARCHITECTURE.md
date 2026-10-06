# Architektur

## Leitlinien

WebKit ist ein Application-Kit, kein vollständiges Framework und kein Ersatz für die Domänenlogik einer Anwendung. Die Oberfläche soll mit Razor Pages direkt lesbar bleiben. Abstraktionen werden nur eingeführt, wenn sie in mindestens zwei Features denselben Vertrag stabilisieren.

## Abhängigkeiten

```text
WebKit.Core          (keine ASP.NET-Abhängigkeit)
      ↑
WebKit.Web           (ASP.NET Core Integration)
      ↑
WebKit.ExampleApp    (Feature- und Page-Code)

WebKit.Design  ──────> CSS-Assets
WebKit.UI      ──────> Razor-Partials und UI-Assets
```

`WebKit.Core` enthält nur portable Anwendungsbausteine. `WebKit.Web` kennt ASP.NET Core, aber keine konkrete Domäne. Die Anwendung entscheidet über Datenzugriff, Feature-Reihenfolge und Authentifizierungsschema.

## Startup

Die minimale Reihenfolge ist:

1. `AddRazorPages()` registriert die Seiten.
2. `AddWebKitWeb()` registriert Clock, User Context, Flash-Message-Store, Problem Details und Capability-Handler.
3. `AddWebKitFeatures(...)` registriert die vertikalen Feature-Module.
4. Die Anwendung richtet ihr Authentifizierungsschema und ihre Policies ein.
5. Die Pipeline verwendet Static Files, Routing, Authentication, Authorization und `MapRazorPages()`.

Feature-Module sind mit `IWebKitFeature` absichtlich klein:

```csharp
public sealed class BillingFeature : IWebKitFeature
{
    public void Register(IServiceCollection services)
    {
        services.AddScoped<IInvoiceService, InvoiceService>();
    }
}
```

## Vertikale Features

Ein Feature bündelt Vertrag, Anwendungscode, Pages und Tests. Ein typisches Verzeichnis sieht so aus:

```text
Features/Catalog/
  CatalogFeature.cs
  IProductCatalog.cs
  Product.cs
  ProductValidator.cs
Pages/Products/
  Index.cshtml
  Index.cshtml.cs
  Edit.cshtml
  Edit.cshtml.cs
```

Die PageModel-Schicht orchestriert HTTP und UI. Sie delegiert Geschäftsregeln an einen Feature-Dienst und gibt `Result<T>`-Fehler gezielt als `404`, `403`, Validation-Fehler oder Flash-Message aus.

## Bewusste Nicht-Entscheidungen

WebKit erzwingt weder MediatR noch CQRS noch Hangfire. Für einfache Razor-Features sind ein klar benannter Dienst, `Result<T>` und ein kleiner Feature-Registrar ausreichend. Hintergrundjobs gehören in die jeweilige Anwendung und werden erst als gemeinsame Abstraktion ergänzt, wenn mehrere Anwendungen denselben Bedarf nachweisen.
