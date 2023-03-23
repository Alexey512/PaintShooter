using Core.CoroutineActions;

namespace Core.Utility.Extensions
{
	public static class CoroutineActionExtensions
	{
		public static T FindAction<T>(this CoroutineAction owner) where T : CoroutineAction
		{
			if (owner is T)
			{
				return owner as T;
			}

			if (owner is CompositeAction composite)
			{
				foreach (var action in composite.Actions)
				{
					var curr = action.FindAction<T>();
					if (curr != null)
						return curr;
				}
			}

			return null;
		}
	}
}
