
using ABCCommerce.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace ABCCommerce.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class ImageController : Controller
{
    public IImageService ImageService { get; }
    public FileExtensionContentTypeProvider ContentTypeProvider { get; }

    public ImageController(IImageService imageService, FileExtensionContentTypeProvider contentTypeProvider)
    {
        ImageService = imageService;
        ContentTypeProvider = contentTypeProvider;
    }


    [HttpGet("{**path}")]
    public ActionResult GetImage(string path)
    {
        path = path.Replace("%2F", "\\");
        var image = ImageService.GetImage(path);
        if(image is null)
        {
            return NotFound("Image not found");
        }
        if(!ContentTypeProvider.TryGetContentType(image.Extension, out var contentType))
        {
            contentType = "application/octet-stream";
        }
        return File(image.Bytes, contentType);
    }
}