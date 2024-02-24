using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Infrastructure.Interfaces;

public interface IFileService
{
    string GetAttachmentPathById(AttachmentId attachmentId);
    string SaveFile(string path, Stream fileStream);
}