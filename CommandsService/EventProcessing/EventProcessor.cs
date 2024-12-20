using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

public class EventProcessor : IEventProcessor
{
    IServiceScopeFactory _scopeFactory;
    IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch(eventType) {
            case EventType.PlatformPublished:
                //TO DO
                break;
            default:
                break;
        }
    }

    private EventType DetermineEvent(string notificationMsg) {
        Console.WriteLine("--> Determing Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMsg);

        switch(eventType!.Event) {
            case "Platform_Published":
                Console.WriteLine("--> Platform_Published event detected");
                return EventType.PlatformPublished;
            default:
                Console.WriteLine("--> Could not determine the event type");
                return EventType.Undetermined;
        }
    }

    private void AddPlatform(string platformPublishedMessage) {
        using (var scope = _scopeFactory.CreateScope()) {
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

            var PlatformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

            try {
                var plat = _mapper.Map<Platform>(PlatformPublishedDto);

                if(!repo.ExternalPlatformExist(plat.ExternalId)) {
                    repo.CreatePlatform(plat);
                    repo.SaveChanges();
                }
                else {
                    Console.WriteLine("--> Platform already exists...");
                }
            }
            catch(Exception ex) {
                Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
            }
        }
    }
}

enum EventType
{
    PlatformPublished,
    Undetermined
}