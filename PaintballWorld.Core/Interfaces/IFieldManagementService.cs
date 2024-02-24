using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Interfaces;

public interface IFieldManagementService
{
    public void CreateField(Field field);
    FieldTypeId GetFieldTypeIdByStringName(string fieldType);
    string SaveRegulationsFile(Stream stream, FieldId fieldId);
}