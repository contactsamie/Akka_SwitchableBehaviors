using Akka.Actor;
using SwitchableBehaviors.Common.Messages;
using System;
using System.Threading.Tasks;

namespace SwitchableBehaviors.Common.Actors
{
    public class FreeBusyActor : ReceiveActor
    {
        public FreeBusyActor()
        {
            // the actor starts as "Free"
            Free();
        }

        private void Busy()
        {
            Receive<IAmFreeOrBusyMessage>(s =>
            {
                // when busy, only accept messages from itself to get free
                if (s is YouAreFreeMessage && Sender.Equals(Self))
                {
                    Context.Sender.Tell(new CurrentlyFreeMessage());
                    Become(Free);
                }
                else if (s is GetBusyMessage )
                {
                    Context.Sender.Tell(new StillBusyMessage());
                }
            });
        }

        private void Free()
        {
            // when free, only "get busy" commands are handled
            Receive<IAmFreeOrBusyMessage>(s =>
            {
                if (!(s is GetBusyMessage)) return;
                Context.Sender.Tell(new WasFreeButWillNowGetBusyMessage());
                // the actor becomes busy, so the next messages are handled differently
                Become(Busy);
                // the actor starts some work in the background
                // when it's done tell itself it's free
                Task.Delay(1000).ContinueWith(_ => new YouAreFreeMessage()).PipeTo(Self, Self);
            });
        }
    }
}