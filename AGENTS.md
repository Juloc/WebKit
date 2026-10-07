# WebKit agent guide

WebKit is the shared ASP.NET Core/Razor application kit. Treat this file as the short operating contract for coding agents.

## Before changing a page

Choose the composition first:

- `ListPage`: breadcrumbs, page header, toolbar/search/filter area, results, empty/error/loading state and pagination.
- `DetailPage`: breadcrumbs, header with status/actions, primary content, facts/metadata and related actions.
- `FormPage`: breadcrumbs, header, validation summary, labelled fields grouped into sections and explicit save/cancel actions.
- `SettingsPage`: header, optional settings navigation, explained sections and save feedback.
- `AdminPage`: the same composition plus server-side capability authorization.
- `DashboardPage`: generic stats/cards/activity primitives only; domain queries stay in the consumer.

Use the shared Razor partials in `WebKit.UI` before writing local equivalents: page header, breadcrumbs, status badge, empty/error/forbidden/loading states, pagination, validation summary, toast, confirm dialog, navigation, stat card and facts list.

## Do not reinvent

Do not add a new button, pagination, dialog, toast, theme switcher, state layout, form validation layout, search toolbar or shell pattern in a feature without first checking WebKit.UI. Domain-specific markup is fine; shared UX grammar belongs in WebKit.

## CSS and JS

- Use semantic WebKit tokens from `WebKit.Design`; do not add raw hex values for standard UI.
- Page CSS should describe domain presentation only. Load it with `@section Styles`.
- Feature JS must be small, progressive-enhancement code loaded with `@section Scripts`; do not add a global feature bundle.
- Keep the global WebKit script limited to theme and accessible native-dialog behavior.

## Responsive and accessibility

Use the responsive defaults of the shared components. Preserve semantic HTML, labels, visible focus, keyboard operation, reduced-motion behavior, touch-sized controls and meaningful live-region semantics. Do not use color as the only state signal.

## Security and contracts

- Use `ICapabilityEvaluator` for capability visibility and server authorization.
- Use `LocalUrl.IsSafe` only as the thin framework-backed return-url check; never implement a second URL parser.
- Use `Result<T>` for expected application failures and validate input on the server.
- POST forms must retain Razor antiforgery behavior.
- Keep feature/domain types in the consumer. WebKit primitives stay domain-neutral.

## Scope guard

Do not add an ORM, repository framework, event bus, CQRS framework, job framework, chart library, SPA runtime, identity provider, upload platform or database abstraction to WebKit.
