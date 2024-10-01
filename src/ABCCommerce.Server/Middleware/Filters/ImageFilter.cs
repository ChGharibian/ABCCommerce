using ABCCommerce.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections;
using System.Reflection;

class ImageFilter : IActionFilter
{
    public ImageService ImageService { get; }
    public ImageFilter(ImageService imageService)
    {
        ImageService = imageService;
    }


    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult objectResult)
        {
            if (objectResult.Value is null) return;
            if (objectResult.Value is IEnumerable i)
            {
                foreach (var obj in i)
                {
                    NewMethod(obj);
                }
            }
            else
            {
                NewMethod(objectResult.Value);
            }
        }
    }

    private void NewMethod(object value)
    {
        var objectType = value.GetType();
        foreach (var property in objectType.GetProperties().Where(p => p.PropertyType == typeof(string) && p.SetMethod is not null && p.GetCustomAttribute<ImageAttribute>() is not null))
        {
            string? imagePath = (string?)property.GetValue(value);
            if (string.IsNullOrWhiteSpace(imagePath)) continue;
            property.SetValue(value, ImageService.GetImageUrl(imagePath));
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }
}