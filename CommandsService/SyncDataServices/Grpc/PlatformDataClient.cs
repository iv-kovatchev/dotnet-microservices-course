
using AutoMapper;
using Grpc.Net.Client;
using PlatformService;

public class PlatformDataClient : IPlatformDataClient
{
    private IConfiguration _configuration;
    private IMapper _mapper;

    public PlatformDataClient(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }

    public IEnumerable<Platform>? ReturnAllPlatforms()
    {
        Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcPlatform"]}");

        var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]!);
        var client = new GrpcPlatform.GrpcPlatformClient(channel);
        var request = new GetAllRequest();

        try {
            var reply = client.GetAllPlatforms(request);
            return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
        }
        catch(Exception ex) {
            Console.WriteLine($"--> Couldn't call GRPC Server {ex.Message}");
            return null;
        }
    }
}