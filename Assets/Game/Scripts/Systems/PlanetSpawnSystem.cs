using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Physics;
using System.Collections.Generic;

public class PlanetSpawnSystem : ComponentSystem
{
    private const int SpawnCount = 4;
    private readonly float[] SpawnRadiuses = { 7f, 9f, 11f, 13f, 15f, 17f, 19f };

    private Random _random;
    private int _count = 0;

    protected override void OnCreate()
    {        
        _random = new Random((uint)System.DateTime.Now.Second + 1);
    }

    protected override void OnUpdate()
    {
        Entities.ForEach((Entity e, ref CreatePlayers createPlayers) =>
        {
            PrefabsStorageComponent prefabsStorage = GetSingleton<PrefabsStorageComponent>();
            List<int> usedRadiusIndexes = new List<int>();

            while (_count < createPlayers.Value)
            {
                if (_count == 0)
                {
                    Entity playerEntity = CreatePlanetEntity(prefabsStorage.PlanetPrefab, usedRadiusIndexes);
                    EntityManager.AddComponentData(playerEntity, new PlayerTag());
                    ++_count;
                    continue;
                }

                CreatePlanetEntity(prefabsStorage.PlanetPrefab, usedRadiusIndexes);
                ++_count;
            }

            EntityManager.DestroyEntity(e);
        });        
    }

    private Entity CreatePlanetEntity(Entity prefabEntity, List<int> usedIndexes)
    {
        Entity spawnedEntity = EntityManager.Instantiate(prefabEntity);

        RenderMesh renderMesh = EntityManager.GetSharedComponentData<RenderMesh>(spawnedEntity);
        UnityEngine.Material mat = new UnityEngine.Material(renderMesh.material);
        mat.SetVector("_RedMinMax", new UnityEngine.Vector4(_random.NextFloat(0f, 1f), _random.NextFloat(0f, 1f)));
        mat.SetVector("_GreenMinMax", new UnityEngine.Vector4(_random.NextFloat(0f, 1f), _random.NextFloat(0f, 1f)));
        mat.SetVector("_BlueMinMax", new UnityEngine.Vector4(_random.NextFloat(0f, 1f), _random.NextFloat(0f, 1f)));
        mat.SetFloat("_NoiseScale", _random.NextFloat(10f, 80f));
        renderMesh.material = mat;
        EntityManager.SetSharedComponentData(spawnedEntity, renderMesh);

        float rndSize = _random.NextFloat(1f, 2.5f);
        int rndOrbitRadiusIdx = _random.NextInt(0, SpawnRadiuses.Length);
        while (usedIndexes.Contains(rndOrbitRadiusIdx)) 
            rndOrbitRadiusIdx = _random.NextInt(0, SpawnRadiuses.Length);

        float rndOrbitRadius = SpawnRadiuses[rndOrbitRadiusIdx];
        int rndDir = _random.NextInt(-5, 6);
        rndDir = rndDir * 2 + 1;
        float movementSpeed = math.sign(rndDir) * 1f * rndSize / rndOrbitRadius;
        float rndPosRadians = _random.NextFloat(0f, math.PI);

        EntityManager.AddComponentData(spawnedEntity, new OrbitRadius { Value = rndOrbitRadius });
        EntityManager.AddComponentData(spawnedEntity, new MovementSpeed { Value = movementSpeed });
        EntityManager.AddComponentData(spawnedEntity, new OrbitAngle { Value = rndPosRadians });
        EntityManager.AddComponentData(spawnedEntity, new Scale { Value = rndSize });
        EntityManager.AddComponentData(spawnedEntity, new ShootCooldown { Value = 0.5f });
        EntityManager.AddComponentData(spawnedEntity, new ShootTime { Value = 0f });
        EntityManager.AddComponentData(spawnedEntity, new AddPlanetHUD());
        EntityManager.AddComponentData(spawnedEntity, new ChangeSphereColliderRadius { Value = rndSize*0.5f });
        EntityManager.AddComponentData(spawnedEntity, new NextRocketType { Value = 0 });
        PhysicsMass planetMass = EntityManager.GetComponentData<PhysicsMass>(spawnedEntity);
        planetMass.InverseMass /= rndSize;
        EntityManager.SetComponentData(spawnedEntity, planetMass);
        EntityManager.SetComponentData(spawnedEntity,
            new Translation { Value = new float3(math.cos(_random.NextFloat(0f, 1f) * math.PI), 0f, math.sin(_random.NextFloat(0f, 1f) * math.PI)) * rndOrbitRadius });

        usedIndexes.Add(rndOrbitRadiusIdx);

        return spawnedEntity;
    }
}
