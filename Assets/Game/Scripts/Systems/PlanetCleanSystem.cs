using Unity.Entities;

[UpdateAfter(typeof(PlanetHUDSystem))]
public class PlanetCleanSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (GetSingleton<GamePause>().IsOn)
            return;

        Entities.ForEach((Entity e, ref Health health) =>
        {
            if (health.Value <= 0)
                PostUpdateCommands.DestroyEntity(e);
        });
    }
}