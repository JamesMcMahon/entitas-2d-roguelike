using Entitas;
using Entitas.CodeGeneration.Attributes;

[Pool]
[Unique]
public class FoodBagComponent : IComponent
{
    public int points;
}
