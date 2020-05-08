using System;
using UnityEngine;
using Sigtrap.Relays;

namespace GameGlue
{
    public class SharedVariable<T> : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        private T initialValue = default(T);

        public T Value
        {
            get
            {
                return runtimeValue;
            }
            set
            {
                runtimeValue = value;
                onValueChanged.Dispatch(runtimeValue);
            }
        }

        private Relay<T> onValueChanged = new Relay<T>();

        [SerializeField]
        private T runtimeValue;

        public void OnAfterDeserialize()
        {
            // Debug.Log("OnAfterDeserialize");
            runtimeValue = initialValue;
        }
        public void OnBeforeSerialize()
        {
            // Debug.Log("OnBeforeSerialize");
        }

        public virtual void ChangeValue(T newValue)
        {
            Value = newValue;
        }

        public void AddListener(Action<T> listener)
        {
            onValueChanged.AddListener(listener);
        }
        public void AddOnce(Action<T> listener)
        {
            onValueChanged.AddOnce(listener);
        }
        public void RemoveListener(Action<T> listener)
        {
            onValueChanged.RemoveListener(listener);
        }
    }
}