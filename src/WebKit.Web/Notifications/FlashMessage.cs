namespace WebKit.Web.Notifications;

public enum FlashMessageKind
{
    Information,
    Success,
    Warning,
    Error
}

public sealed record FlashMessage(FlashMessageKind Kind, string Text);

public interface IFlashMessageStore
{
    void Add(FlashMessage message);

    IReadOnlyList<FlashMessage> Read();
}
