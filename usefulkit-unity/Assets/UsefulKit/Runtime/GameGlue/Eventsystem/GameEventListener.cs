using UnityEngine;
using UnityEngine.Events;

namespace GameGlue
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent gameEvent;
        public UnityEvent eventResponse;

        private void OnEnable()
        {
            gameEvent?.AddListener(this);
        }

        private void OnDisable()
        {
            gameEvent?.RemoveListener(this);
        }

        [ContextMenu("Raise Events")]
        public void OnRaiseEvents()
        {
            if (eventResponse != null) eventResponse.Invoke();
        }
    }
}