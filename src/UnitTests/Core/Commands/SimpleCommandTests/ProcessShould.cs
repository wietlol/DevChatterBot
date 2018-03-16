﻿using System;
using System.Threading.Tasks;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Events;
using Xunit;

namespace UnitTests.Core.Commands.SimpleCommandTests
{
    public class ProcessShould
    {
        [Fact]
        public void SendStaticMessage_GivenNoTokensInMessage()
        {
            var simpleCommand = new SimpleCommand(null, "Hello");

            var fakeChatClient = new FakeChatClient();

            simpleCommand.Process(fakeChatClient, new CommandReceivedEventArgs());

            Assert.Equal("Hello", fakeChatClient.SentMessage);
        }

        [Fact]
        public void SendHydratedMessage_GivenTokensInMessage()
        {
            var simpleCommand = new SimpleCommand(null, "[UserDisplayName] says hello!");
            var fakeChatClient = new FakeChatClient();
            var commandReceivedEventArgs = new CommandReceivedEventArgs {ChatUser = {DisplayName = "Brendan"}};

            simpleCommand.Process(fakeChatClient, commandReceivedEventArgs);

            Assert.Equal("Brendan says hello!", fakeChatClient.SentMessage);
        }
    }

    public class FakeChatClient : IChatClient
    {
        public Task Connect()
        {
            throw new NotImplementedException();
        }

        public Task Disconnect()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string message)
        {
            SentMessage = message;
        }

        public string SentMessage { get; set; }

        public event EventHandler<CommandReceivedEventArgs> OnCommandReceived;
        public event EventHandler<NewSubscriberEventArgs> OnNewSubscriber;
    }
}