using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CoroutineActions
{
	[Serializable]
	public class Selector: Parallel
	{
		protected override bool Update()
		{
			if (Actions.Any(action => !action.IsExecuting))
			{
				foreach (var action in Actions)
				{
						action.Terminate();
				}
			}
			
			return base.Update();
		}
	}
}
