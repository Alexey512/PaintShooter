using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    [Serializable]
	public abstract class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [Serializable]
        private struct KeyValueData
        {
            public TKey Key;
            public TValue Value;
        }
        
        [SerializeField, HideInInspector]
        private readonly List<KeyValueData> _keyValueData = new List<KeyValueData>();

        public void OnBeforeSerialize()
        {
            _keyValueData.Clear();
            foreach (var item in this)
            {
                _keyValueData.Add(new KeyValueData { Key = item.Key, Value = item.Value });
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();
            foreach (var item in _keyValueData)
            {
                this[item.Key] = item.Value;
            }
        }
    }
}
