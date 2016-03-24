using Akka.Actor;
using SwitchableBehaviors.Common.Messages;
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
            Receive<YouAreFreeMessage>(message =>
            {
                //allow only itself to determine when to become free
                if (!Sender.Equals(Self)) return;
                Context.Sender.Tell(new CurrentlyFreeMessage());
                Become(Free);
            });

            Receive<GetBusyMessage>(message =>
            {
                Context.Sender.Tell(new StillBusyMessage());
            });
        }

        private void Free()
        {
            // when free, only "get busy" commands are handled
            Receive<GetBusyMessage>(s =>
            {
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
