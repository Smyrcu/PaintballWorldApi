namespace PaintballWorld.Core.Interfaces;

public interface IApiKeyService
{
    bool IsApiKeyValid(string apiKey);
}