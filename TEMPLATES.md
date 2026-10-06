# Vorlagen

Die Vorlagen unter `templates/` sind kleine `dotnet new`-Vorlagen ohne versteckte Infrastruktur. Sie dienen als Startpunkt; die Anwendung ergänzt Datenbank, Authentifizierung und Domänenregeln selbst.

## Installieren und verwenden

```powershell
dotnet new install .\templates\webkit-app
dotnet new webkit-app -n Contoso.Portal -o samples/Contoso.Portal
cd samples/Contoso.Portal
dotnet run
```

Für einzelne Bausteine:

```powershell
dotnet new install .\templates\webkit-feature
dotnet new webkit-feature -n BillingFeature

dotnet new install .\templates\webkit-page
dotnet new webkit-page -n Reports

dotnet new install .\templates\webkit-admin-page
dotnet new webkit-admin-page -n Settings
```

Die Vorlagen halten die Namen absichtlich generisch. Nach der Generierung gehören Feature-Code und PageModels in die Zielanwendung; die gemeinsamen Contracts bleiben in WebKit.Core/WebKit.Web.
