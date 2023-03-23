using System.Collections;
using UnityEngine;

namespace Core.CoroutineActions
{
    public class Coroutiner : Singleton<Coroutiner>
    {
        public static bool ApplicationIsQuitting { get; private set; }

        public Coroutine StartLocalCoroutine(IEnumerator enumerator)
        {
	        return StartCoroutine(enumerator);
        }

        public void StopLocalCoroutine(Coroutine coroutine)
        {
	        StopCoroutine(coroutine);
        }

        public void StopLocalCoroutine(IEnumerator enumerator)
        {
	        StopCoroutine(enumerator);
        }

        public void StopAllLocalCoroutines()
        {
	        StopAllCoroutines();
        }
        
        public static Coroutine Start(IEnumerator enumerator)
        {
            return Instance != null && enumerator != null ? Instance.StartLocalCoroutine(enumerator) : null;
        }

        public static void Stop(IEnumerator enumerator)
        {
            if (Instance == null || enumerator == null) 
	            return;
            Instance.StopLocalCoroutine(enumerator);
        }

        public static void Stop(Coroutine coroutine)
        {
            if (Instance == null || coroutine == null) 
	            return;
            Instance.StopLocalCoroutine(coroutine);
        }

        public static void StopAll()
        {
            if (Instance == null) 
	            return;
            Instance.StopAllLocalCoroutines();
        }

        private void OnApplicationQuit()
        {
	        ApplicationIsQuitting = true;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RunOnStart()
        {
	        ApplicationIsQuitting = false;
        }
    }
}