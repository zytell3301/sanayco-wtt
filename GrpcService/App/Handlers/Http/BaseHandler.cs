using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace GrpcService1.App.Handlers.Http;

public class BaseHandler : ControllerBase
{
    private string ParsePayload()
    {
        return new StreamReader(Request.BodyReader.AsStream()).ReadToEnd();
    }

    protected T DecodePayloadJson<T>()
    {
        return JsonSerializer.Deserialize<T>(ParsePayload());
    }
}