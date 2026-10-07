# Page composition patterns

WebKit standardizes structure and behavior, not product branding. Consumers can compose the primitives into a sidebar shell, topbar shell, bottom-navigation shell or minimal document shell.

## List page

```text
Breadcrumbs
PageHeader + primary action
Toolbar: Search / filters / sort
Loading | Error | Empty | Results
Pagination (retains existing query parameters)
```

Use `_Breadcrumbs`, `_PageHeader`, `_EmptyState`, `_ErrorState`, `_LoadingState` and `_Pagination`. Put feature-specific filter controls in the toolbar; keep query binding in the PageModel.

## Detail page

```text
Breadcrumbs
PageHeader + status + actions
Primary content
Facts / metadata
Related or secondary actions
```

Use `_StatusBadge`, `_FactsList` and `_ConfirmDialog` where appropriate. A destructive action must be an explicit server-side POST with antiforgery protection.

## Form page

```text
Breadcrumbs
PageHeader
Validation summary
Sections with labelled fields, hints and errors
Save / Cancel / optional destructive action
```

Forms are server-first. Progressive enhancement may improve interaction but cannot be the only validation or authorization boundary.

## Settings, dashboard and admin

Settings use `_SettingsSection`-style sections in the consumer and show save feedback through typed toasts. Dashboards use `_StatCard`, cards, activity and actions; WebKit does not provide a chart runtime. Admin pages use the same structure and `[Authorize(Policy = "...")]` on the server.

## Shell composition

`WebKit.UI` provides navigation and theme primitives. The application owns the composition and brand: Jularr-like media navigation, FullWorth-like finance navigation and a minimal document shell can all use the same contracts without sharing colors, logos or density.
