namespace WebKit.Design;

/// <summary>
/// Names the stable token families exposed by the WebKit stylesheet.
/// Applications customize values through CSS variables rather than replacing components.
/// </summary>
public static class DesignTokens
{
    public const string ColorSurface = "--wk-color-surface";
    public const string ColorSurfaceRaised = "--wk-color-surface-raised";
    public const string ColorSurfaceSubtle = "--wk-color-surface-subtle";
    public const string ColorText = "--wk-color-text";
    public const string ColorTextMuted = "--wk-color-text-muted";
    public const string ColorBorder = "--wk-color-border";
    public const string ColorAccent = "--wk-color-accent";
    public const string ColorAccentContrast = "--wk-color-accent-contrast";
    public const string ColorSuccess = "--wk-color-success";
    public const string ColorWarning = "--wk-color-warning";
    public const string ColorDanger = "--wk-color-danger";
    public const string ColorFocus = "--wk-color-focus";
    public const string Space = "--wk-space-";
    public const string Radius = "--wk-radius-";
    public const string Shadow = "--wk-shadow-";
    public const string Font = "--wk-font-";
    public const string ControlHeight = "--wk-control-height";
    public const string ContentMax = "--wk-content-max";
}
