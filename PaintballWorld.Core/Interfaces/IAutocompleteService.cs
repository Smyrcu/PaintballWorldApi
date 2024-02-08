namespace PaintballWorld.Core.Interfaces;

public interface IAutocompleteService
{
    IEnumerable<string?> GetCityAutocomplete(string cityName);
}