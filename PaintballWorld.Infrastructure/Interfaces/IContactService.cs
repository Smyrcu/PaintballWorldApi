namespace PaintballWorld.Infrastructure.Interfaces;

public interface IContactService
{
    Task SaveContactMessage(string email, string content, string topic);
}