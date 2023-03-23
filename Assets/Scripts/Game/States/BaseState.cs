using System;
using System.Collections;
using HSM;
using Zenject;

namespace Game.States
{
	public class BaseState: State
	{
		[Inject] 
		protected SignalBus SignalBus;

		protected IEnumerator SignalWaiting<TSignal>(Action<TSignal> callback = null)
		{
			yield return WaitSignal.Running<TSignal>(SignalBus, s => callback?.Invoke(s));
		}
	}
}
