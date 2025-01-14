using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BossChallengeMod.Extensions {
    public static class RCGEventReceiverExtensions {
        private static FieldInfo transitionReceiverIReceiversRef = AccessTools.Field(typeof(RCGEventReceiver), "iReceivers");

        public static void AddSubscriber(this RCGEventReceiver receiver, IRCGArgEventReceiver subscriber) {
            var receiverSubscribers = (IRCGArgEventReceiver[])transitionReceiverIReceiversRef.GetValue(receiver);
            transitionReceiverIReceiversRef.SetValue(receiver, receiverSubscribers.Append(subscriber).ToArray());
        }
    }
}
