using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class PlanetDamageSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (GetSingleton<GamePause>().IsOn)
            return;

        Entities.ForEach((Entity e, ref TakeDamage damage, ref Health health) =>
        {
            health.Value -= damage.Value;

            if (health.Value <= 0)
            {
                PostUpdateCommands.AddComponent(e, new RemovePlanetHUD());
            }

            PostUpdateCommands.RemoveComponent(e, typeof(TakeDamage));
        });
    }
}