using Entitas;
using UnityEngine;
using UnityEngine.UI;

public class FoodTextController : MonoBehaviour
{
    Text label;
    int? food;

    void Start()
    {
        label = GetComponent<Text>();

        var pool = Pools.pool;
        pool.GetGroup(Matcher.FoodBag).OnEntityAdded += 
            (group, entity, index, component) => UpdateFood(entity.foodBag.points);
        if (pool.hasFoodBag)
        {
            food = pool.foodBag.points;
            UpdateFood(food.Value);
        }
    }

    void UpdateFood(int newFood)
    {
        if (!food.HasValue)
        {
            food = newFood;
        }

        var diff = newFood - food.Value;
        var symbol = diff > 0 ? "+" : ""; 
        var prefix = Mathf.Abs(diff) > 1 ? symbol + diff + " " : "";
        label.text = prefix + "Food: " + newFood;
        food = newFood;
    }
}
