using BotDataBetweenDialogs.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BotDataBetweenDialogs.Dialogs
{
    [Serializable]
    public class Dialog2 : IDialog<object>
    {
        private readonly string _name;
        private Person _personConstructor;
        private Person _personBotState;

        public Dialog2(string name, Person person)
        {
            //Grab the data sent with the constructor and store it in a private variable for use throughout the dialog
            _name = name;
            _personConstructor = person;
        }

        public async Task StartAsync(IDialogContext context)
        {
            //Grab the person object from bot state and store it in a private variable for use throughout the dialog
            context.ConversationData.TryGetValue("person", out _personBotState);

            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.PostAsync($"The Name passed via the dialog constructor is: {_name}");
            await context.PostAsync($"The Person object passed via the dialog constructor is: {_personConstructor.Name}, {_personConstructor.Age}");
            await context.PostAsync($"The Person object from bot state is: {_personBotState.Name}, {_personBotState.Age}");

            await context.PostAsync($"What message would you like to send back?");

            //await user's response and process it in ResponseReceivedAsync
            context.Wait(ResponseReceivedAsync);
        }

        public virtual async Task ResponseReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            //Grab the message the user sent.
            var message = await result;

            //Close context passing it back up to Dialog1
            context.Done(message);
        }
    }
}