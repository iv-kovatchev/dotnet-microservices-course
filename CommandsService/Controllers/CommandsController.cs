using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[Route("api/c/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController: ControllerBase {
    private readonly ICommandRepo _repository;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepo repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandCreateDto>> GetCommandsForPlatform(int platformId) {
        Console.WriteLine($"--> Hit GetCommandsForPlatform {platformId}");

        if(!_repository.PlatformExist(platformId)) {
            return NotFound();
        }
        
        var commands = _repository.GetCommandsForPlatform(platformId);

        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
    }
}