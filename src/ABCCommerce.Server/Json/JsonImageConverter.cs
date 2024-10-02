using ABCCommerce.Server.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

class JsonImageConverter : JsonConverter<ImagePath>
{
    public JsonImageConverter(ImageService imageService)
    {
        ImageService = imageService;
    }

    public ImageService ImageService { get; }

    public override ImagePath? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, ImagePath value, JsonSerializerOptions options)
    {
        var url = ImageService.GetImageUrl(value);
        writer.WriteStringValue(url);
    }
}
