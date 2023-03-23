using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

namespace Core.Utility
{
	public class IntervalCallback
	{
		public float Interval { get; set; }

		private Action _callback;

		private float _leftTime;

		public bool IsRunning { get; set; }

		public void SetCallback(Action callback)
		{
			_callback = callback;
		}

		public void Reset()
		{
			_leftTime = Interval;
		}

		public void Update()
		{
			if (!IsRunning)
				return;
			
			if (_leftTime < 0)
			{
				_callback?.Invoke();
				_leftTime = Interval;
			}
			else
			{
				_leftTime -= Time.deltaTime;
			}
		}
	}
}
