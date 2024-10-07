using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonImageConverter))]
public record ImagePath(string Path)
{
    [return: NotNullIfNotNull(nameof(path))]
    public static implicit operator ImagePath?(string? path) => path is null ? null : new ImagePath(path);
}