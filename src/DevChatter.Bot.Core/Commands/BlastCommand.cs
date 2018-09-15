using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using System;
using System.Linq;

namespace DevChatter.Bot.Core.Commands
{
    public class BlastCommand : BaseCommand
    {
        private readonly IRepository _repository;
        private readonly IAnimationDisplayNotification _animationDisplayNotification;

        public BlastCommand(IRepository repository,
            IAnimationDisplayNotification animationDisplayNotification)
            : base(repository, UserRole.Everyone)
        {
            _repository = repository;
            _animationDisplayNotification = animationDisplayNotification;
            Cooldown = TimeSpan.FromMinutes(2);
            string choiceText = "\"hype\" or \"derp\""; // create from operations later.
            HelpText = $"Use \"!blast [choice]\". Replace [choice] with one of: {choiceText} ";
        }

        protected override void HandleCommand(IChatClient chatClient,
            CommandReceivedEventArgs eventArgs)
        {
            string blastName = eventArgs?.Arguments?.ElementAtOrDefault(0);
            var blastType = _repository.Single(BlastTypeEntityPolicy.ByName(blastName));
            if (blastType != null)
            {
                chatClient.SendMessage(blastType.Message);
                _animationDisplayNotification.Blast(blastType.ImagePath);
            }
            else
            {
                chatClient.SendMessage(HelpText);
            }
        }
    }
}
