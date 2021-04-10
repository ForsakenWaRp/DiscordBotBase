using Newtonsoft.Json;

namespace ForsakenNet.Settings
{
    public class SettingsModel
    {
        //JSON Format for settings with Getters and Setters.
        [JsonProperty("token")] public string BotToken { get; set; }
        [JsonProperty("prefix")] public char Prefix { get; set; }
        [JsonProperty("task")] public int TaskSleep { get; set; }
        [JsonProperty("version")] public string Version { get; set; }
        [JsonProperty("channel")] public ulong BotChannel { get; set; }
    }

    public static class Settings
    {
        //Static Model so the same is loaded all over the program.
        public static SettingsModel BotSettings = new SettingsModel();
    }
}
