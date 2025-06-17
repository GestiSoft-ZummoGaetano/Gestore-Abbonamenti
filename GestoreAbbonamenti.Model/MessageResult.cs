using GestoreAbbonamenti.Common.Enum;

namespace GestoreAbbonamenti.Model;
public class MessageResult
{
    public ResultType Result { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }

    public object? Command { get; set; }
}
