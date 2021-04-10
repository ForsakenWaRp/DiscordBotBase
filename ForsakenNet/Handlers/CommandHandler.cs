using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ForsakenNet.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;


namespace ForsakenNet.Events
{
    public class CommandHandler
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public CommandHandler(IServiceProvider services)
        {
            //Ok here the Idiot Initalized..
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _commands.CommandExecuted += CommandExecutedAsync;

            _discord.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            //Read stupid Text form command here and pass it through
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            //Checks and parses if the Message id from a valid user else return
            if (!(rawMessage is SocketUserMessage message)) return;
            //Checks if  message is from a User and not a bot
            if (message.Source != MessageSource.User) return;
            //Sets the Position in the message where the Prefix should be.
            var argPos = 0;
            //Checks if the message has the Prefix and in the right spot else return;
            if (!message.HasCharPrefix('!', ref argPos)) return;
            //Sends command through Discord.Net Commands with data needed
            var context = new SocketCommandContext(_discord, message);

            await _commands.ExecuteAsync(context, argPos, _services);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            //Declare Emoji for responce
            var thumbsUp = new Emoji("👍");
            //If Prefix is called but no Command there just return.
            if (!command.IsSpecified) return;
            //When there is Succes.
            if (result.IsSuccess)
            {
                //Log Commands..
                await Log.WriteLog($"User:{context.User} - {context.Message}", LogType.Command);
                //Add Thumbsup when succes don't know why just seems nice
                await context.Message.AddReactionAsync(thumbsUp);
                return;
            }
        }

    }
}
