# Vorlagen

Die Vorlagen unter `templates/` sind kleine `dotnet new`-Vorlagen ohne versteckte Infrastruktur. Sie dienen als Startpunkt; die Anwendung ergänzt Datenbank, Authentifizierung und Domänenregeln selbst.

## Installieren und verwenden

```powershell
dotnet new install .\templates\webkit-app
dotnet new webkit-app -n Contoso.Portal -o samples/Contoso.Portal
cd samples/Contoso.Portal
dotnet run
```

Die vollständige Sammlung:

```powershell
dotnet new install .\templates\webkit-feature
dotnet new install .\templates\webkit-page
dotnet new install .\templates\webkit-list-page
dotnet new install .\templates\webkit-detail-page
dotnet new install .\templates\webkit-form-page
dotnet new install .\templates\webkit-settings-page
dotnet new install .\templates\webkit-admin-page

dotnet new webkit-feature -n Billing -o Features/Billing
dotnet new webkit-page -n Reports -o Pages/Reports
dotnet new webkit-list-page -n Providers -o Pages/Providers
dotnet new webkit-detail-page -n ProviderDetails -o Pages/ProviderDetails
dotnet new webkit-form-page -n ProviderEdit -o Pages/ProviderEdit
dotnet new webkit-settings-page -n Preferences -o Pages/Preferences
dotnet new webkit-admin-page -n Settings -o Pages/Settings
```

`webkit-app` referenziert die vier WebKit-Pakete. Für einen lokalen Dogfood-Lauf werden sie zuerst nach `artifacts/` gepackt und beim Restore als lokaler Feed verwendet; genau das prüft `scripts/validate.ps1`. Nach der Generierung gehören Feature-Code und PageModels in die Zielanwendung; gemeinsame Contracts bleiben in WebKit.Core/WebKit.Web.
