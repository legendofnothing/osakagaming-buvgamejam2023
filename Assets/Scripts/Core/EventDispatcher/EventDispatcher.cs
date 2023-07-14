using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Scripts.Core.EventDispatcher {
    public class EventDispatcher : Singleton<EventDispatcher> {
        private Dictionary<EventType, Action<object>> _listeners = new(); //Store listeners and correspond event type

        private void OnDestroy() {
            ClearAllListeners();
        }

        /// <summary>
        /// Subscribe listeners w/ event type
        /// </summary>
        /// <param name="type">Event Type</param>
        /// <param name="callback">Invoked callback</param>
        public void SubscribeListener(EventType type, Action<object> callback) {
            
            //Check if key exist in existing dictionary
            if (_listeners.ContainsKey(type)) _listeners[type] += callback;
            else {
                _listeners.Add(type, null);
                _listeners[type] += callback;
            }
        }
        
        /// <summary>
        /// Unsubscribe listeners w/ event type
        /// </summary>
        /// <param name="type">Event Type</param>
        /// <param name="callback">Invoked callback</param>
        public void UnsubscribeListener(EventType type, Action<object> callback) {
            if (_listeners.ContainsKey(type)) _listeners[type] -= callback;
        }

        /// <summary>
        /// Unsubscribe all listeners 
        /// </summary>
        public void ClearAllListeners() {
            _listeners.Clear();
        }
        
        /// <summary>
        /// Send event messages to all listeners with event type key
        /// </summary>
        /// <param name="type">Event Type</param>
        /// <param name="param">Invoked data, can be any type</param>
        public void SendMessage(EventType type, object param = null) {
            if (!_listeners.ContainsKey(type)) {
                UnityEngine.Debug.Log($"No registered listener for this event: {type.ToString()}");
                return;
            }

            var callbacks = _listeners[type];
            if (callbacks != null) callbacks(param);
            else _listeners.Remove(type);
        }
    }
    
    //Extension Class, add extension if necessary
    public static class EventDispatcherExtension {
        //Send w/ events
        public static void SendMessage(this MonoBehaviour listener, EventType type, object param) =>
            EventDispatcher.instance.SendMessage(type, param);
        
        //Send wo/ events
        public static void SendMessage(this MonoBehaviour listener, EventType type) =>
            EventDispatcher.instance.SendMessage(type);

        
        public static void SubscribeListener(this MonoBehaviour listener, EventType type, Action<object> callback) =>
            EventDispatcher.instance.SubscribeListener(type, callback);
    }
}
