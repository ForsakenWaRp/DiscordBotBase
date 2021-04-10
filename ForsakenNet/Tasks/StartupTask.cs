using ForsakenNet.Logging;
using ForsakenNet.Settings;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace ForsakenNet.Tasks
{
    public class StartupTask
    {
        //Combine the Dir where we load the program with Filename as Windows uses \ and Unix //
        public string pathToFile = Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), "settings.json");

        public async Task Startup()
        {
            //Check if file exists if not then create and write base settings to it
            if(!File.Exists(pathToFile))
            {
                await Log.WriteLog("First run detected.", LogType.Warning);
                Settings.Settings.BotSettings = new SettingsModel() { BotToken = "", Prefix = '!', TaskSleep = 10, BotChannel = 0, Version = "0.0.1" };
                await saveSettings();
                await Log.WriteLog("Created Settings file", LogType.Warning);
                await Log.WriteLog("Edit the BotToken and BotChannel", LogType.Warning);
                await Log.WriteLog("Close me please...", LogType.Warning);
            } else
            {
                await Log.WriteLog("Starting ForsakenBot.", LogType.Log);
                await loadSettings();
                await Log.WriteLog($"Bot version: {Settings.Settings.BotSettings.Version}, Bot prefix {Settings.Settings.BotSettings.Prefix}, Task Timer: {Settings.Settings.BotSettings.TaskSleep} Seconds", LogType.Log);
            }                
        }

        //Save the JSON File Windows and Unix and Serialize it to JSON
        public async Task saveSettings()
        {
            using (StreamWriter fileWriter = File.CreateText(pathToFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(fileWriter, Settings.Settings.BotSettings);
                await Task.CompletedTask;
            }
        }

        //Load it and Desiserialize it
        public async Task loadSettings()
        {
            string jsonText = await File.ReadAllTextAsync(pathToFile);
            Settings.Settings.BotSettings = JsonConvert.DeserializeObject<SettingsModel>(jsonText);
            await Task.CompletedTask;
        }
    }
}
