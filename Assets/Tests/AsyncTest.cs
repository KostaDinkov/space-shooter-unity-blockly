using System.Collections;
using System.Threading.Tasks;


namespace Tests
{
    public class AsyncTest
    {
        public static IEnumerator Execute<T>(Task<T> task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            yield return task.Result;
        }
        public static IEnumerator Execute(Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsFaulted) throw task.Exception;


        }
    }
}
