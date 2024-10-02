using ABCCommerce.Server.Services;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

public class ConfigureJsonOptions : IConfigureOptions<JsonOptions>
{
    private readonly ImageService _fooService;

    public ConfigureJsonOptions(ImageService fooService)
    {
        _fooService = fooService;
    }

    public void Configure(JsonOptions options)
    {
        options.SerializerOptions.Converters
            .Add(new JsonImageConverter(_fooService));
    }
}
