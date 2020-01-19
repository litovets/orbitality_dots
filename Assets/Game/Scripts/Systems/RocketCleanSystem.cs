using Unity.Entities;

public class RocketCleanSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity e, ref RocketBaseStats stats, ref CreationTime creation) =>
        {
            if (Time.ElapsedTime - creation.Value >= stats.Cooldown)
            {
                PostUpdateCommands.DestroyEntity(e);
            }
        });
    }
}
