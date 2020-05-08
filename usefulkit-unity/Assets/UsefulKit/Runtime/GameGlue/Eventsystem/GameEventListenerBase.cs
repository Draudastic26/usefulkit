using UnityEngine;
using UnityEngine.Events;

namespace GameGlue
{
    public abstract class GameEventListenerBase<T> : MonoBehaviour
    {
        public GameEventBase<T> gameEvent;
        public UnityEvent<T> eventResponse;

        private void OnEnable()
        {
            gameEvent?.AddListener(this);
        }

        private void OnDisable()
        {
            gameEvent?.RemoveListener(this);
        }

        [ContextMenu("Raise Events")]
        public void OnRaiseEvents(T value)
        {
            if (eventResponse != null) eventResponse.Invoke(value);
        }
    }
}