using System.ComponentModel.DataAnnotations;
using Azure.Messaging.WebPubSub;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;
[ApiController]
public class DefaultController : ControllerBase
{
    private readonly WebPubSubServiceClient _client;

    public DefaultController(WebPubSubServiceClient client)
    {
        _client = client;
    }
    [HttpGet("negotiate")]
    public IActionResult Get([Required] string id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("User id is required");
        }
        var accessUri = _client.GetClientAccessUri(userId: id, roles: new []{"webpubsub.joinLeaveGroup.lari","webpubsub.sendToGroup.lari"});
        return Ok(accessUri.AbsoluteUri);
    }
    
    [HttpOptions("eventhandler")]
    public IActionResult Option()
    {
        var allowedOrigin = "izaan.webpubsub.azure.com";
        var requestedOrigin = Request.Headers["WebHook-Request-Origin"].FirstOrDefault();
        if (allowedOrigin != requestedOrigin) return BadRequest("Header origin mismatch");
        Response.Headers[$"WebHook-Allowed-Origin"] = requestedOrigin;
        return Ok();

    }

    [HttpPost("eventhandler")]
    public async Task<IActionResult> Post()
    {
        var sr = new StreamReader(Request.Body);
        var body = await sr.ReadToEndAsync();
        WriteToConsole(Request.Headers, body);
        if (Request.Headers["ce-type"] != "azure.webpubsub.user.messsage")
        {
            var userId = Request.Headers["ce-userid"];
            await _client.SendToAllAsync($"{userId} {body}");
            
        }
        
        return Ok();

    }

    private static void WriteToConsole(IHeaderDictionary requestHeaders, string body)
    {
        Console.WriteLine("=====================================================");
        Console.WriteLine($"Body : {body}");
        Console.WriteLine("============ Headers ================================");
        requestHeaders.ToList().ForEach(h =>
        {
            Console.WriteLine($"   {h.Key} - {h.Value}");
        });
    }
}