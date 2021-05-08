using DSharpPlus;
using DSharpPlus.Entities;
using System.Text.RegularExpressions;
using System.Globalization;

namespace BotWithUI
{
    public struct BotGuild
    {
        public DiscordGuild Guild { get; }
        public ulong Id => this.Guild.Id;
        public string Name => this.Guild.Name;
        public string IconUrl => $"{this.Guild.IconUrl}?size=32";

        public BotGuild(DiscordGuild gld)
        {
            this.Guild = gld;
        }

        public override string ToString()
        {
            return this.Guild.Name;
        }
    }

    public struct BotChannel
    {
        public DiscordChannel Channel { get; }
        public ulong Id => this.Channel.Id;
        public string Name => this.Channel.Name;

        public BotChannel(DiscordChannel chn)
        {
            this.Channel = chn;
        }

        public override string ToString()
        {
            return $"#{this.Channel.Name}";
        }
    }

    public struct BotMessage
    {
        public DiscordMessage Message { get; }
        public ulong Id => this.Message.Id;
        public string AuthorName => this.Message.Author.Username;
        public string AuthorAvatarUrl => this.Message.Author.GetAvatarUrl(ImageFormat.Png, 32);
        public string Content => this.Message.Content;

        public BotMessage(DiscordMessage msg)
        {
            this.Message = msg;
        }

        public override string ToString()
        {
            string getContent = @$"{this.Message.Author.Username}: {this.Message.Content}";
            return getContent.ToString();
        }
        
    }
}
