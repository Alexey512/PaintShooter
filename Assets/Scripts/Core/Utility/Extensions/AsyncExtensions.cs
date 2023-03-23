using System.Collections;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class AsyncExtensions
    {
        public static IEnumerator Await(this Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }
        }
    }
}
