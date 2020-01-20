using Unity.Entities;

public class RocketCleanSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (GetSingleton<GamePause>().IsOn)
            return;

        Entities.ForEach((Entity e, ref RocketBaseStats stats, ref CreationTime creation) =>
        {
            if (Time.ElapsedTime - creation.Value >= stats.Lifetime)
            {
                PostUpdateCommands.DestroyEntity(e);
            }
        });
    }
}
