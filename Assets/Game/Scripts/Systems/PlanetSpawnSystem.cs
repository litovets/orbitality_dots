using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;

public class PlanetSpawnSystem : ComponentSystem
{
    private const int SpawnCount = 4;
    private int _count = 0;
    private Random _random;

    protected override void OnCreate()
    {        
        _random = new Random((uint)System.DateTime.Now.Second);
    }

    protected override void OnUpdate()
    {
        PrefabsStorageComponent prefabsStorage = GetSingleton<PrefabsStorageComponent>();
        float prevOrbitRadius = 5f;
        while (_count < SpawnCount)
        {
            Entity spawnedEntity = EntityManager.Instantiate(prefabsStorage.PlanetPrefab);

            RenderMesh renderMesh = EntityManager.GetSharedComponentData<RenderMesh>(spawnedEntity);
            UnityEngine.Material mat = new UnityEngine.Material(renderMesh.material);
            mat.SetVector("_RedMinMax", new UnityEngine.Vector4(_random.NextFloat(0f, 1f), _random.NextFloat(0f, 1f)));
            mat.SetVector("_GreenMinMax", new UnityEngine.Vector4(_random.NextFloat(0f, 1f), _random.NextFloat(0f, 1f)));
            mat.SetVector("_BlueMinMax", new UnityEngine.Vector4(_random.NextFloat(0f, 1f), _random.NextFloat(0f, 1f)));
            mat.SetFloat("_NoiseScale", _random.NextFloat(10f, 80f));
            renderMesh.material = mat;
            EntityManager.SetSharedComponentData(spawnedEntity, renderMesh);

            float rndSize = _random.NextFloat(1f, 2.5f);
            float rndOrbitRadius = prevOrbitRadius + rndSize *_random.NextFloat(1.5f, 2f);
            int rndDir = _random.NextInt(-5, 5);
            rndDir = rndDir * 2 + 1;
            float movementSpeed = math.sign(rndDir)*5f*rndSize / rndOrbitRadius;
            float rndPosRadians = _random.NextFloat(0f, math.PI);

            EntityManager.AddComponentData(spawnedEntity, new OrbitRadius { Value = rndOrbitRadius });
            EntityManager.AddComponentData(spawnedEntity, new MovementSpeed { Value = movementSpeed });
            EntityManager.AddComponentData(spawnedEntity, new OrbitAngle { Value = rndPosRadians });
            EntityManager.AddComponentData(spawnedEntity, new CompositeScale
            {
                Value = new float4x4(
                new float4(rndSize, 0f, 0f, 0f),
                new float4(0f, rndSize, 0f, 0f),
                new float4(0f, 0f, rndSize, 0f),
                new float4(0f, 0f, 0f, 1f)
                )
            });
            EntityManager.SetComponentData(spawnedEntity, 
                new Translation { Value = new float3(math.cos(_random.NextFloat(0f, 1f)*math.PI), 0f, math.sin(_random.NextFloat(0f, 1f) * math.PI)) * rndOrbitRadius });

            prevOrbitRadius = rndOrbitRadius;
            ++_count;
        }
    }
}
