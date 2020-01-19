using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(PlayerInputSystem))]
public class PlanetShootingSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        PrefabsStorageComponent prefabsStorage = GetSingleton<PrefabsStorageComponent>();
        Entities.ForEach((Entity e, ref Translation translation, ref CompositeScale scale, ref ShootDirectionInput fire) =>
        {
            Entity rocketEntity = EntityManager.Instantiate(prefabsStorage.RocketPrefab);
            EntityManager.AddComponentData(rocketEntity, new RocketTag());
            EntityManager.AddComponentData(rocketEntity, new Velocity { Value = fire.Value*5f });
            EntityManager.SetComponentData(rocketEntity, new Translation { Value = new float3(translation.Value) + math.normalize(fire.Value)*scale.Value.c0.x*1.05f });
            EntityManager.AddComponentData(rocketEntity, new CreationTime { Value = (float) Time.ElapsedTime });

            PostUpdateCommands.RemoveComponent(e, typeof(ShootDirectionInput));
        });
    }
}
