namespace PaintballWorld.Core.Interfaces;

public interface IAutocompleteService
{
    IEnumerable<object?> GetCityAutocomplete(string cityName);
}