using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.UI {
    public class QueueUISystem : MonoBehaviour {

        protected Queue<IEnumerator> operationsQueue = new();
        private bool _isProcessing = false;

        protected Action? OnOperationProcessed;

        public void EnqueueOperation(IEnumerator uiOperation) {
            operationsQueue.Enqueue(uiOperation);

            if (!_isProcessing) {
                StartCoroutine(ProcessQueue());
            }
        }

        private IEnumerator ProcessQueue() {
            _isProcessing = true;

            while (operationsQueue.Count > 0) {
                IEnumerator operation = operationsQueue.Dequeue();
                yield return StartCoroutine(operation);
                OnOperationProcessed?.Invoke();
            }

            _isProcessing = false;
        }
    }
}
