using BotDataBetweenDialogs.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BotDataBetweenDialogs.Dialogs
{
    [Serializable]
    public class Dialog1 : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //Use FormFlow to get details of the Person object. This sampel is not about FormFlow but it is the simplest way to create a complex type with data from the user
            var personDialog = FormDialog.FromType<Person>(FormOptions.PromptInStart);
            context.Call(personDialog, ResumeAfterPersonFormDialog);
        }

        private async Task ResumeAfterPersonFormDialog(IDialogContext context, IAwaitable<Person> result)
        {
            //Grab the Person from FormFlow
            var person = await result;

            //store the Person in bot state
            context.ConversationData.SetValue("person", person);

            //Forward the context to the Dialog2 and pass in the Person. Process the result from Dialog2 in ResumeAfterDialog2
            await context.Forward(new Dialog2(person), this.ResumeAfterDialog2, person, CancellationToken.None);
        }

        private async Task ResumeAfterDialog2(IDialogContext context, IAwaitable<object> result)
        {
            //Grab the resut sentback from Dialog2 when is was closed with context.Done
            var message = await result as Activity;
            var messageText = message.Text;

            //Tell the user what they selected. More typically you'd do something with the result here
            await context.PostAsync($"You sent {messageText} back from Dialog2");

            //Close context passing it back up to RootDialog
            context.Done("");
        }
    }
}