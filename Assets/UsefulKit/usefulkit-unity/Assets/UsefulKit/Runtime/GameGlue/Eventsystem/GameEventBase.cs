using System;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;

namespace GameGlue
{
    public abstract class GameEventBase<T> : ScriptableObject
    {
        private List<GameEventListenerBase<T>> eventListeners = new List<GameEventListenerBase<T>>();

        private Relay<T> onRaiseRelay = new Relay<T>();

        public virtual void Raise(T value)
        {
            onRaiseRelay.Dispatch(value);

            for (int i = eventListeners.Count - 1; i >= 0; i--)
            {
                eventListeners[i].OnRaiseEvents(value);
            }
        }

        public virtual void AddListener(GameEventListenerBase<T> listener)
        {
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }

        public virtual void RemoveListener(GameEventListenerBase<T> listener)
        {
            if (eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);
            }
        }
        public virtual void AddListener(Action<T> listener)
        {
            onRaiseRelay.AddListener(listener);
        }

        public virtual void RemoveListener(Action<T> listener)
        {
            onRaiseRelay.RemoveListener(listener);
        }

        public virtual void AddOnce(Action<T> listener)
        {
            onRaiseRelay.AddOnce(listener);
        }
    }
}