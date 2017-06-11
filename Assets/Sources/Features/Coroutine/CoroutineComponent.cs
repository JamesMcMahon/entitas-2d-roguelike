using Entitas;
using System.Collections;

[Pool]
public class CoroutineComponent : IComponent
{
    public IEnumerator value;
}
