using System.Collections.Generic;
using UnityEngine;
using Sigtrap.Relays;
using System;

namespace GameGlue
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {        
        public List<T> Items = new List<T>();

        [NonSerialized]
        public Relay OnSetChanged = new Relay();

        [NonSerialized]
        public Relay<T> OnAddedItem = new Relay<T>();

        [NonSerialized]
        public Relay<T> OnRemovedItem = new Relay<T>();
        
        public void Add(T item)
        {
            if (!Items.Contains(item))
            {
                Items.Add(item);
                OnSetChanged.Dispatch();
                OnAddedItem.Dispatch(item);
            }
        }

        public void Remove(T item)
        {
            if (Items.Contains(item))
            {
                Items.Remove(item);
                OnSetChanged.Dispatch();
                OnRemovedItem.Dispatch(item);
            }
        }

        private void OnEnable()
        {
            Items.Clear();
        }

        private void OnDisable()
        {
            Items.Clear();            
        }
    }
}