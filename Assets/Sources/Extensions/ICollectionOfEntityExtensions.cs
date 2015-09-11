using Entitas;
using System.Collections.Generic;

namespace ICollectionOfEntityExtensions
{
    public static class ICollectionOfEntityExtensions
    {
        public static bool ContainsComponent(this ICollection<Entity> entities,
                                             int componentId, 
                                             out Entity entity)
        {
            foreach (var e in entities)
            {
                if (e.HasComponent(componentId))
                {
                    entity = e;
                    return true;
                }
            }
            entity = null;
            return false;
        }

        public static bool ContainsComponent(this ICollection<Entity> entities,
                                             int componentId)
        {
            Entity unused = null;
            return entities.ContainsComponent(componentId, out unused);
        }
    }
}
