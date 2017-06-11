using Entitas;
using System.Collections.Generic;

namespace ICollectionOfEntityExtensions
{
    public static class ICollectionOfEntityExtensions
    {
        public static bool ContainsComponent(this ICollection<PoolEntity> entities,
                                             int componentId, 
                                             out PoolEntity entity)
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

        public static bool ContainsComponent(this ICollection<PoolEntity> entities,
                                             int componentId)
        {
            PoolEntity unused = null;
            return entities.ContainsComponent(componentId, out unused);
        }
    }
}
