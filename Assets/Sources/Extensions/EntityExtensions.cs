using Entitas;

public static class EntityExtensions
{
    public static void AddAudioAttackSource(this PoolEntity entity,
                                              params Audio[] clips)
    {
        entity.AddAudioAttackSource(clips, clips.Length > 1);
    }

    public static void AddAudioDeathSource(this PoolEntity entity,
                                              params Audio[] clips)
    {
        entity.AddAudioDeathSource(clips, clips.Length > 1);
    }

    public static void AddAudioPickupSource(this PoolEntity entity,
                                              params Audio[] clips)
    {
        entity.AddAudioPickupSource(clips, clips.Length > 1);
    }

    public static void AddAudioWalkSource(this PoolEntity entity,
                                            params Audio[] clips)
    {
        entity.AddAudioWalkSource(clips, clips.Length > 1);
    }

    public static void AddDestructible(this PoolEntity entity, int hp)
    {
        entity.AddDestructible(hp, hp);
    }

    public static void DamageDestructible(this PoolEntity entity)
    {
        var current = entity.destructible;
        entity.ReplaceDestructible(current.hp - 1, current.startingHP);
    }
}
