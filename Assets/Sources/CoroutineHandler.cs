using System.Collections;
using System.Collections.Generic;

public class CoroutineHandler
{
    IList<IEnumerator> coroutines = new List<IEnumerator>();

    public void Add(IEnumerator coroutine)
    {
        coroutines.Add(coroutine);
    }

    public void Update()
    {
        for (int i = 0; i < coroutines.Count; i++)
        {
            var coroutine = coroutines[i];

            if (!coroutine.MoveNext())
            {
                coroutines.RemoveAt(i);
                i--;
                continue;
            }
        }
    }
}
