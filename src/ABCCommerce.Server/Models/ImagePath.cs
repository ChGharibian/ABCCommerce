using System.Diagnostics.CodeAnalysis;

public record ImagePath(string Path)
{
    public static implicit operator string(ImagePath path) => path.Path;
    [return: NotNullIfNotNull(nameof(path))]
    public static implicit operator ImagePath?(string? path) => path is null ? null : new ImagePath(path);
}