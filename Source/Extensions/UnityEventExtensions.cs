using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine.Events;

namespace BossChallengeMod.Extensions {
    public static class UnityEventExtensions {

        private static readonly FieldInfo UnityEventCallsFieldRef = AccessTools.Field(typeof(UnityEvent), "m_Calls");
        private static readonly FieldInfo InvokableCallListRuntimeCallsFieldRef = AccessTools.Field(typeof(InvokableCallList), "m_RuntimeCalls");
        private static readonly FieldInfo InvokableCallDelegateFieldRef = AccessTools.Field(typeof(InvokableCall), "Delegate");

        public static void AddUniqueListener(this UnityEvent unityEvent, UnityAction method) {
            // Check if the listener is already registered
            if (!unityEvent.HasListener(method)) {
                unityEvent.AddListener(method);
            }
        }

        private static bool HasListener(this UnityEvent unityEvent, UnityAction method) {
            var invokeCall = (InvokableCallList)UnityEventCallsFieldRef.GetValue(unityEvent);
            if (invokeCall == null) return false;

            var runtimeCalls = (List<BaseInvokableCall>)InvokableCallListRuntimeCallsFieldRef.GetValue(invokeCall);
            if (runtimeCalls == null) return false;

            foreach (var call in runtimeCalls) {
                var targetDelegate = (Delegate)InvokableCallDelegateFieldRef.GetValue(call);

                if (targetDelegate != null && targetDelegate?.Method == method.Method && targetDelegate.Target == method.Target) {
                    return true; // Listener already exists
                }
            }

            return false;
        }
    }
}
