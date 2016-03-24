using Akka.Actor;
using System;

namespace SwitchableBehaviors.Common.Actors
{
    public class ConsoleActor : ReceiveActor
    {
        public ConsoleActor()
        {
            ReceiveAny(message => Console.WriteLine(message.GetType().Name));
        }
    }
}