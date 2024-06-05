namespace ModelLibrary;

public class GameConfig
{
    public GameConfiguration game_configuration { get; set; }
}

public class GameConfiguration
{
    public TribeConfig tribe_b { get; set; }
    public TribeConfig tribe_c { get; set; }
}

public class TribeConfig
{
    public string speakerStyle { get; set; }
}