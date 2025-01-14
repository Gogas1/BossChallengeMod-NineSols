using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.BossPatches {
    public record TransitionWrapper {
        public RCGEventReceiveTransition transitionComponent;
        public RCGEventReceiver eventReceiver;

        public TransitionWrapper(RCGEventReceiveTransition transitionComponent, RCGEventReceiver eventReceiver) {
            this.transitionComponent = transitionComponent;
            this.eventReceiver = eventReceiver;
        }
    }
}
