using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(PlanetShootingSystem))]
public class RocketMovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (GetSingleton<GamePause>().IsOn)
            return;

        Entities.ForEach((Entity e, ref RocketTag rocketTag, ref RocketBaseStats baseStats, ref Velocity velocity, ref Translation translation) =>
        {
            EntityManager.SetComponentData(e, new Translation { Value = translation.Value + velocity.Value * Time.DeltaTime });

            Velocity nextVelocity = new Velocity { Value = velocity.Value + math.normalize(velocity.Value)*baseStats.Acceleration / baseStats.Weight };
            EntityManager.SetComponentData(e, nextVelocity);            
        });
    }
}
