
public interface ICommandRepo {
    bool SaveChanges();

    //Platforms
    IEnumerable<Platform> GetAllPlatforms();

    void CreatePlatform(Platform plat);

    bool PlatformExist(int platformId);

    bool ExternalPlatformExist(int externalPlatformId);

    //Commands
    IEnumerable<Command> GetCommandsForPlatform(int platformId);

    Command? GetCommand(int platformId, int commandId);

    void CreateCommand(int platformId, Command command);
}