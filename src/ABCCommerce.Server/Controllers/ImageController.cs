
using Microsoft.AspNetCore.Mvc;

namespace ABCCommerce.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class ImageController : Controller
{
    public ImageController(IHostEnvironment hostEnvironment)
    {
        HostEnvironment = hostEnvironment;
    }

    public IHostEnvironment HostEnvironment { get; }

    [HttpGet("{**path}")]
    public ActionResult GetImage(string path)
    {
        path = path.Replace("%2F", "\\");
        string pathstart = Path.Combine(HostEnvironment.ContentRootPath, "images");
        string fullPath = Path.GetFullPath(Path.Combine(pathstart, path));
        if (!fullPath.StartsWith(pathstart) || !Path.Exists(fullPath))
        {
            return NotFound("Image not found");
        }
        var bytes = System.IO.File.ReadAllBytes(fullPath);
        return File(bytes, $"image/{Path.GetExtension(path)}");
    }
}