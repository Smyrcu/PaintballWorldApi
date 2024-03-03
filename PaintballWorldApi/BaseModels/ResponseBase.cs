using System.Security.Policy;

namespace PaintballWorld.API.BaseModels;

public class ResponseBase
{
    public bool IsSuccess { get; set; }
    public IList<string> Errors { get; set; }
    public string Message { get; set; }
}

