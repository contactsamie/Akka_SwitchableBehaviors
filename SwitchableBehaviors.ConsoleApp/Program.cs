using System;
using Akka.Actor;
using System.Threading.Tasks;
using SwitchableBehaviors.Common.Actors;
using SwitchableBehaviors.Common.Messages;

namespace SwitchableBehaviors.ConsoleApp
{
    public class Program
    {
        public static void Main()
        {
            var system = ActorSystem.Create("Sample");
            var consoleActorRef = system.ActorOf<ConsoleActor>();
            var actor = system.ActorOf<FreeBusyActor>();
            Task.Run(async () =>
            {
                for (var i = 0; i < 10; i++)
                {
                    actor.Tell(new GetBusyMessage() , consoleActorRef);
                    await Task.Delay(500);
                }
            }).Wait();

            Console.ReadLine();
        }
    }
}
