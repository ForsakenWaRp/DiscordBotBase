using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ForsakenNet.Commands
{
    public class ChatCommands : ModuleBase<SocketCommandContext>
    {
		Tasks.HTTPClientTask clientTask = new Tasks.HTTPClientTask();

		[Command("test")]
		[Summary("testing new intern shit")]
		public async Task Test()
		{
			//Checks if bot is responding with Emote
			await Task.CompletedTask;
		}

		[Command("uptime")]
		[Summary("Bot Uptime")]
		public async Task uptime()
		{
			//Get TickCount64 as TickCount doesn't go further then 25 day's
			var timeSpan = TimeSpan.FromMilliseconds(Environment.TickCount64);
			EmbedBuilder builder = new EmbedBuilder();
			builder.WithTitle($"Time Running");
			builder.WithCurrentTimestamp();
			builder.WithThumbnailUrl("https://media.giphy.com/media/psIw9yvUL8rR3AsJwj/giphy.gif");
			//Parses the TickCOunt to Day's/Hours/Minutes/Seconds
			builder.AddField($"Server Uptime:", $"{timeSpan:%d\\:hh\\:mm\\:ss}");
			//Get's the Uptime we declared in Program.cs
			builder.AddField($"Bot Uptime:", $"{Program.Uptime.getTime()}");
			builder.WithFooter($"ForsakenBot V0.0.1");
			//Replies the Embed
			await ReplyAsync($"", false, builder.Build());
		}



	}
}
