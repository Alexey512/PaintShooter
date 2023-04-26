using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ZigZag.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using Zenject;

namespace Game.ZigZag.Input
{
	public class ScreenTapInput: MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		[Inject]
		private SignalBus _signalBus;
		
		public void OnPointerDown(PointerEventData eventData)
		{
			_signalBus?.Fire<TapScreenEvent>();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
		}
	}
}
