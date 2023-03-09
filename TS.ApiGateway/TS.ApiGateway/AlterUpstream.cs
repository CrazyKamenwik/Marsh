using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TS.ApiGateway;

public class AlterUpstream
{
    public static string AlterUpstreamSwaggerJson(HttpContext context, string swaggerJson)
    {
        var swagger = JObject.Parse(swaggerJson);

        return swagger.ToString(Formatting.Indented);
    }
}