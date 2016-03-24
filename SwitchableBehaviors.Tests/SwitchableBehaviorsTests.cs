using System;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwitchableBehaviors.Common.Actors;
using SwitchableBehaviors.Common.Messages;

namespace SwitchableBehaviors.Tests
{
    [TestClass]
    public class SwitchableBehaviorsTests : TestKit
    {
        [TestMethod]
        public void actor_can_become_busy()
        {
            //Arrange
            var actor = ActorOf<FreeBusyActor>();

            //Act
            actor.Tell(new GetBusyMessage());

            //Assert
            ExpectMsg<WasFreeButWillNowGetBusyMessage>();
        }

        [TestMethod]
        public void when_an_actor_is_alredy_busy_it_remians_busy_just_right_after()
        {
            //Arrange
            var actor = ActorOf<FreeBusyActor>();

            //Act
            actor.Tell(new GetBusyMessage());
            ExpectMsg<WasFreeButWillNowGetBusyMessage>();

            actor.Tell(new GetBusyMessage());
            ExpectMsg<StillBusyMessage>();
        }

        [TestMethod]
        public void actor_can_become_free()
        {
            //Arrange
            var actor = ActorOf<FreeBusyActor>();

            //Act
            actor.Tell(new GetBusyMessage());
            ExpectMsg<WasFreeButWillNowGetBusyMessage>();

            //  System.Threading.Thread.Sleep(3000);
            AwaitAssert(() =>
            {
                actor.Tell(new GetBusyMessage());
                ExpectMsg<WasFreeButWillNowGetBusyMessage>();
            }, TimeSpan.FromSeconds(3000), TimeSpan.FromMilliseconds(250));
           
        }

        [TestMethod]
        public void actor_cannot_be_told_to_become_free()
        {
            //Arrange
            var actor = ActorOf<FreeBusyActor>();

            //Act
            actor.Tell(new YouAreFreeMessage());

            //Assert
            ExpectNoMsg();
        }
    }
}
