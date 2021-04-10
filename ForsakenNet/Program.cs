using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ForsakenNet.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EventHandler = ForsakenNet.Events.EventHandler;

namespace ForsakenNet
{
    class Program
    {
        //Declare stuff
        public DiscordSocketClient _Client;
        public EventHandler eventHandler;
        public static Util.TimerUtil Uptime;
        public Tasks.StartupTask startupTask = new Tasks.StartupTask();
        //Make it Async baby
        static void Main(string[] args)
            => new Program().BotAsync().GetAwaiter().GetResult();


        public async Task BotAsync()
        {
            //StartupTask for settings and everything.
            await startupTask.Startup();
            //Check if we get Token from settings.json
            if(Settings.Settings.BotSettings.BotToken.Length <= 5)
            {
                await Logging.Log.WriteLog($"Invalid Bot token..", Logging.LogType.Error);
                Console.Read();
                await Task.Delay(10000);
                return;
            }
            //Discord.Net Things
            var discordToken = Settings.Settings.BotSettings.BotToken;//Add Token or from Settings..
            var service = ConfigureServices();
            //Get Uptime as i like that 
            Uptime = new Util.TimerUtil();
            //Get DiscordSocketClient
            _Client = service.GetRequiredService<DiscordSocketClient>();
            //Handles Client Events..
            eventHandler = new EventHandler(service);

            //Bot login Nothing to explain here..
            //Set TokenType for Discord.Net And add the Discord Token.
            await _Client.LoginAsync(TokenType.Bot, discordToken);
            //Make the bot connact and start
            await _Client.StartAsync();
            //Stopwatch for Bot uptime using it like this because Unix compatible and easy.
            Uptime.Start();

            //Commands are handled there, u idiot need to initialize..
            await service.GetRequiredService<CommandHandler>().InitializeAsync();

            //Await to keep it running.
            await Task.Delay(Timeout.Infinite);

        }

        //Service of Discord.Net 
        private ServiceProvider ConfigureServices()
        {
            //So the program can find the Services in Runtime.
            //Making it easy to add command classes etc.
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }

    }
}
