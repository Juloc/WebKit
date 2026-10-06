namespace WebKit.Design;

/// <summary>
/// Names the stable token families exposed by the WebKit stylesheet.
/// Applications customize values through CSS variables rather than replacing components.
/// </summary>
public static class DesignTokens
{
    public const string Surface = "--wk-surface";
    public const string SurfaceRaised = "--wk-surface-raised";
    public const string Text = "--wk-text";
    public const string TextMuted = "--wk-text-muted";
    public const string Border = "--wk-border";
    public const string Accent = "--wk-accent";
    public const string Focus = "--wk-focus";
}
