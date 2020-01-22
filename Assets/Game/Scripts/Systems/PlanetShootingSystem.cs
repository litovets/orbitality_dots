using Game.Physics;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(PlayerInputSystem))]
public class PlanetShootingSystem : ComponentSystem
{
    private Random _random;

    protected override void OnCreate()
    {
        _random = new Random((uint)System.DateTime.Now.Second + 1);
    }

    protected override void OnUpdate()
    {
        if (GetSingleton<GamePause>().IsOn)
            return;

        Entities.ForEach((ref PrefabsStorageComponent prefabsStorage) =>
        {
            var storage = prefabsStorage;
            Entities.ForEach((Entity e, ref Translation translation, ref Scale scale, ref ShootDirectionInput fire, ref ShootCooldown cooldown, ref ShootTime shootTime, ref NextRocketType rocketType) =>
            {
                if (Time.ElapsedTime - shootTime.Value >= cooldown.Value)
                {
                    NativeArray<Entity> rocketPrefabs = new NativeArray<Entity>(3, Allocator.Temp);
                    rocketPrefabs[0] = storage.TomahawkRocketPrefab;
                    rocketPrefabs[1] = storage.RocketPrefab;
                    rocketPrefabs[2] = storage.HeavyRocketPrefab;

                    Entity rocketEntity = EntityManager.Instantiate(rocketPrefabs[rocketType.Value]);
                    EntityManager.AddComponentData(rocketEntity, new RocketTag());
                    EntityManager.AddComponentData(rocketEntity, new Velocity { Value = fire.Value * 5f });
                    EntityManager.SetComponentData(rocketEntity, new Translation { Value = new float3(translation.Value) + math.normalize(fire.Value) * scale.Value * 1.05f });
                    EntityManager.AddComponentData(rocketEntity, new CreationTime { Value = (float)Time.ElapsedTime });

                    PostUpdateCommands.SetComponent(e, new ShootTime { Value = (float)Time.ElapsedTime });
                    PostUpdateCommands.RemoveComponent(e, typeof(ShootDirectionInput));

                    rocketType.Value = _random.NextInt(0, rocketPrefabs.Length);

                    rocketPrefabs.Dispose();
                }
            });
        });        
    }
}
