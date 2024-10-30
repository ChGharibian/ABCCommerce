using Microsoft.AspNetCore.Http.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

class JsonImageConverter : JsonConverter<ImagePath>
{
    IHttpContextAccessor HttpContextAccessor { get; }
    public JsonImageConverter()
    {
        HttpContextAccessor = new HttpContextAccessor();
    }
    string Url => new Uri(HttpContextAccessor.HttpContext!.Request.GetEncodedUrl()).GetLeftPart(UriPartial.Authority) + "/Image/";


    public override ImagePath? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, ImagePath value, JsonSerializerOptions options)
    {
        var url = Url + value.Path;
        writer.WriteStringValue(url);
    }
}
