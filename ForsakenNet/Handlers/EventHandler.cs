using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ForsakenNet.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ForsakenNet.Events
{
    public class EventHandler
    {
        //Only need to Read the data no need to write info Moron
        private readonly DiscordSocketClient _Client;
        private readonly CommandService _Commands;

        public EventHandler(IServiceProvider service)
        {
            //Declare the Services here like the _Client and _CommandHandler
            _Client = service.GetRequiredService<DiscordSocketClient>();
            _Commands = service.GetRequiredService<CommandService>();
            //Events and where to Handle them like Client.WhatEvent += void to handle it in
            _Client.Ready += ClientReady;
            _Client.UserJoined += UserJoined;
            _Client.UserLeft += UserLeft;
            _Commands.Log += Commands;
            _Client.JoinedGuild += GuildJoin;


        }
        //Called when bot get's invited to a Guild.
        private Task GuildJoin(SocketGuild guild)
        {
            Log.WriteLog(guild.ToString(), LogType.Log);
            return Task.CompletedTask;

        }
        //Called when a User uses a Command.
        private Task Commands(LogMessage log)
        {
            Log.WriteLog($"{log.Message}", LogType.Command);
            return Task.CompletedTask;
        }
        //Called when a user Leaves a Guild.
        private Task UserLeft(SocketGuildUser arg)
        {
            Log.WriteLog($"User: {arg.Nickname} left the Guild.", LogType.Log);
            return Task.CompletedTask;
        }
        //Called when a user Joines a Guild
        private Task UserJoined(SocketGuildUser arg)
        {
            Log.WriteLog($"User: {arg.Nickname} Joined the guild.", LogType.Log);
            return Task.CompletedTask;
        }
        //Called when Client is fully conneccted to Discord and all Guilds it's in(Usefull to wait for when this is ready.)
        private Task ClientReady()
        {
            Log.WriteLog($"Client Ready with: {_Client.Latency}MS.", LogType.Log);
            return Task.CompletedTask;
        }
    }
}
