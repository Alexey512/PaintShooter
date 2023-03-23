using System.Collections;
using Game.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game.States
{
	public class LoadGameState: BaseState
	{
		[Inject]
		private ILoadScreen _loadScreen;
		
		protected override void OnEnter()
		{
			_loadScreen.Show();
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

			_loadScreen.Hide();
		}
	}
}
