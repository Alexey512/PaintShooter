using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.States
{
	public class LoadGameState: BaseState
	{
		protected override void OnEnter()
		{
		}

		protected override IEnumerator OnExecute()
		{
			//TODO: load adressable assets
			
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
			while (!asyncLoad.isDone)
			{
				yield return null;
			}

			SignalBus.Fire<LoadCompleteEvent>();
		}
	}
}
