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
        Entities.ForEach((Entity e, ref TakeDamage damage, ref Health health) =>
        {
            health.Value -= damage.Value;

            if (health.Value <= 0)
                PostUpdateCommands.DestroyEntity(e);

            PostUpdateCommands.RemoveComponent(e, typeof(TakeDamage));
        });
    }
}