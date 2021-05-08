using System.Threading.Tasks;
using DSharpPlus;
using Microsoft.Extensions.Logging;

namespace BotWithUI
{
    // this class holds the bot itself
    // this is simply used as a sort of container to keep the code organized
    // and partially separated from the UI logic
    public class Bot
    {
        // the client instance, this is initialized with the class
        public DiscordClient Client { get; }

        // this instantiates the container class and the client
        public Bot(string token)
        {
            // create config from the supplied token
            var cfg = new DiscordConfiguration
            {
                Token = token,                   // use the supplied token
                TokenType = TokenType.Bot,       // log in as a bot

                AutoReconnect = true,            // reconnect automatically
                LogLevel = DSharpPlus.LogLevel.Debug,
            };

            // initialize the client
            this.Client = new DiscordClient(cfg);
        }

        // this method logs in and starts the client
        public Task StartAsync()
            => this.Client.ConnectAsync();

        // this method logs out and stops the client
        public Task StopAsync()
            => this.Client.DisconnectAsync();
    }
}
