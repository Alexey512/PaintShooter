using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace HSM
{
    public class WaitSignal: IDisposable
    {
        private readonly SignalBus _signalBus;
        
        private Type _signalType;
        
        private bool _isCall;
        
        private object _signalValue; 
        
        private WaitSignal(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Dispose()
        {
	        _signalBus?.TryUnsubscribe(_signalType, SignalHandler);
        }

        public static IEnumerator Running<T>(SignalBus signalBus, Action<T> callback)
        {
            yield return new WaitSignal(signalBus).WaitRunning<T>(s => callback?.Invoke(s));
        }

        private IEnumerator WaitRunning<T>(Action<T> callback)
        {
            _signalType = typeof(T);
            _signalBus.Subscribe<T>(SignalHandler);
            yield return new WaitWhile(() => !_isCall);
            callback?.Invoke((T)_signalValue);
        }

        private void SignalHandler<TSignal>(TSignal signal)
        {
            _signalBus.Unsubscribe<TSignal>(SignalHandler);
            _isCall = true;
            _signalValue = signal;
        }
    }
}