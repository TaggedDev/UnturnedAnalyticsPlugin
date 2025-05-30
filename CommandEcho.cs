using System;
using System.Threading.Tasks;
using OpenMod.Core.Commands;

namespace Scitalis.Analytics
{
    [Command("echo")]
    [CommandDescription("Echo a message")]
    [CommandSyntax("<message>")]
    public class CommandEcho : Command
    {
        public CommandEcho(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            
        }

        protected override async Task OnExecuteAsync()
        {        
            // This gets us the text that the user wants to echo.
            string text = string.Join(" ", Context.Parameters);
            
            await Context.Actor.PrintMessageAsync(text);
        }
    }
}