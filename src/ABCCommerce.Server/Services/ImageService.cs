using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.VisualBasic;

namespace ABCCommerce.Server.Services;
public class ImageService
{
    string url;
    public ImageService(IHttpContextAccessor contextAccessor)
    {
        url = new Uri(contextAccessor.HttpContext!.Request.GetEncodedUrl()).GetLeftPart(UriPartial.Authority) + "/images/";
    }
    public string GetImageUrl(string path)
    {
        return url + path;
    }
}
