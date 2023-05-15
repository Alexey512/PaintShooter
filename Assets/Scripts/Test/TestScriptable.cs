using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Test
{
	[CreateAssetMenu(menuName = "Test/" + nameof(TestScriptable))]	
	public class TestScriptable: ScriptableObject
	{
		[SerializeField]
		private List<TestScriptable> _childs;
	}
}
