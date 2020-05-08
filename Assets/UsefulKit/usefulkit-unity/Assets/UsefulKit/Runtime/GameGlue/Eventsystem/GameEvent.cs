using System;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;

namespace GameGlue
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameGlue/GameEvent", order = 0)]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> eventListeners = new List<GameEventListener>();

        private Relay onRaiseRelay = new Relay();

        public virtual void Raise()
        {
            onRaiseRelay.Dispatch();

            for (int i = eventListeners.Count - 1; i >= 0; i--)
            {
                eventListeners[i].OnRaiseEvents();
            }
        }

        public virtual void AddListener(GameEventListener listener)
        {
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }

        public virtual void RemoveListener(GameEventListener listener)
        {
            if (eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);
            }
        }
        public virtual void AddListener(Action listener)
        {
            onRaiseRelay.AddListener(listener);
        }

        public virtual void RemoveListener(Action listener)
        {
            onRaiseRelay.RemoveListener(listener);
        }

        public virtual void AddOnce(Action listener)
        {
            onRaiseRelay.AddOnce(listener);
        }
    }
}