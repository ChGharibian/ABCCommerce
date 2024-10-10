using ABCCommerceDataAccess;
using ABCCommerceDataAccess.Models;
using Microsoft.AspNetCore.StaticFiles;
using System.Text;
using Image = ABCCommerce.Server.Services.Image;
using DbImage = ABCCommerceDataAccess.Models.Image;
using Microsoft.Extensions.Hosting;
namespace ABCCommerce.Server.Services;
public interface IImageService
{
    Image? GetImage(string path);
    ImagePath AddImage(string base64, string type, string? basePath = null);
}
public class DbImageService : IImageService
{
    public ABCCommerceContext AbcDb { get; }

    public DbImageService(ABCCommerceContext abcDb)
    {
        AbcDb = abcDb;
    }

    public Image? GetImage(string path)
    {
        var image = AbcDb.Images.FirstOrDefault(i => i.Key == path);
        if (image is null) return null;
        return new Image(Convert.FromBase64String(image.Base64), image.Type);
    }

    public ImagePath AddImage(string base64, string type, string? basePath = null)
    {
        string imageName = $"{Convert.ToBase64String(Encoding.ASCII.GetBytes(DateTime.Now.ToString("O")))}{type}";
        string path = basePath is null ? imageName : Path.Combine(basePath, imageName);
        var image = new DbImage()
        {
            Key = path,
            Base64 = base64,
            Type = type
        };
        return new ImagePath(path);
    }
}
public class FileSystemImageService : IImageService
{
    public IHostEnvironment HostEnvironment { get; }
    public FileSystemImageService(IHostEnvironment hostEnvironment)
    {
        HostEnvironment = hostEnvironment;
    }

    public ImagePath AddImage(string base64, string type, string? basePath = null)
    {
        var bytes = Convert.FromBase64String(base64);

        string name = $"{Convert.ToBase64String(Encoding.ASCII.GetBytes(DateTime.Now.ToString("O")))}{type}";
        if(basePath is not null)
        {
            name = Path.Combine(basePath, name);
        }
        string pathstart = Path.Combine(HostEnvironment.ContentRootPath, "images");
        File.WriteAllBytes(Path.Combine(pathstart, name), bytes);
        return new ImagePath(name);
    }

    public Image? GetImage(string path)
    {
        string pathstart = Path.Combine(HostEnvironment.ContentRootPath, "images");
        string fullPath = Path.GetFullPath(Path.Combine(pathstart, path));
        if (!fullPath.StartsWith(pathstart) || !Path.Exists(fullPath))
        {
            return null;
        }
        var bytes = File.ReadAllBytes(fullPath);
        return new Image(bytes, Path.GetExtension(path));
    }
}
public class Image
{
    public byte[] Bytes { get; }
    public string Extension { get; }
    public Image(byte[] bytes, string extension)
    {
        Bytes = bytes;
        Extension = extension;
    }
}