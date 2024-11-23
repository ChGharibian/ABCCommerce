
using ABCCommerce.Server.Services;
using Microsoft.AspNetCore.Cors;
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
    /// <summary>
    /// The request to get an image.
    /// </summary>
    /// <param name="path">The server path of the image.</param>
    /// <returns></returns>

    [HttpGet("{**path}")]
    [DisableCors]
    public ActionResult GetImage(string path)
    {
        path = Uri.UnescapeDataString(path);
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