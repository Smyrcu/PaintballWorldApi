namespace PaintballWorld.Infrastructure.Interfaces;

public interface IContactService
{
    void SaveContactMessage(string email, string content, string topic);
}