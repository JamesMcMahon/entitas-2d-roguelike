using Entitas;

public static class EntityExtensions
{
    public static Entity AddAudioAttackSource(this Entity entity,
                                              params Audio[] clips)
    {
        return entity.AddAudioAttackSource(clips, clips.Length > 1);
    }

    public static Entity AddAudioDeathSource(this Entity entity,
                                              params Audio[] clips)
    {
        return entity.AddAudioDeathSource(clips, clips.Length > 1);
    }

    public static Entity AddAudioPickupSource(this Entity entity,
                                              params Audio[] clips)
    {
        return entity.AddAudioPickupSource(clips, clips.Length > 1);
    }

    public static Entity AddAudioWalkSource(this Entity entity,
                                            params Audio[] clips)
    {
        return entity.AddAudioWalkSource(clips, clips.Length > 1);
    }

    public static Entity AddDestructible(this Entity entity, int hp)
    {
        return entity.AddDestructible(hp, hp);
    }

    public static Entity DamageDestructible(this Entity entity)
    {
        var current = entity.destructible;
        return entity.ReplaceDestructible(current.hp - 1, current.startingHP);
    }
}
