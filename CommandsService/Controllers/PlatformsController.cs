
using Microsoft.AspNetCore.Mvc;

[Route("api/c/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase 
{
    public PlatformsController()
    {
        
    }

    [HttpPost]
    public ActionResult TextInboundConnection() 
    {
        Console.WriteLine("--> Inbound POST # Command Service");

        return Ok("Inbound test of from Platforms Controller");
    }
}