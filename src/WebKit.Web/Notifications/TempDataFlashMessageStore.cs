using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebKit.Web.Notifications;

public sealed class TempDataFlashMessageStore(IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataFactory) : IFlashMessageStore
{
    private const string Key = "WebKit.FlashMessages";

    public void Add(FlashMessage message)
    {
        ITempDataDictionary tempData = tempDataFactory.GetTempData(httpContextAccessor.HttpContext!);
        List<FlashMessage> messages = ReadFrom(tempData);
        messages.Add(message);
        tempData[Key] = JsonSerializer.Serialize(messages);
    }

    public IReadOnlyList<FlashMessage> Read()
    {
        ITempDataDictionary tempData = tempDataFactory.GetTempData(httpContextAccessor.HttpContext!);
        List<FlashMessage> messages = ReadFrom(tempData);
        tempData.Remove(Key);
        return messages;
    }

    private static List<FlashMessage> ReadFrom(ITempDataDictionary tempData)
    {
        return tempData[Key] is string json ? JsonSerializer.Deserialize<List<FlashMessage>>(json) ?? [] : [];
    }
}
