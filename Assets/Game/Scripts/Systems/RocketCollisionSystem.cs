using Game.Physics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class RocketCollisionSystem : JobComponentSystem
{
    [BurstCompile]
    struct RocketCollisionJob : IJobParallelFor
    {
        [ReadOnly] public PhysicsLayer CollisionMask;
        [ReadOnly] public float3 RocketPos;
        [ReadOnly] public float RocketRadius;
        [ReadOnly] public PhysicsLayer RocketLayer;
        [ReadOnly] public NativeArray<float3> OtherPositions;
        [ReadOnly] public NativeArray<float> OtherRadiuses;
        [ReadOnly] public NativeArray<PhysicsLayer> OtherLayers;
        public NativeArray<bool> CollisionResult;

        public void Execute(int index)
        {
            bool hasRoketLayer = (CollisionMask & RocketLayer) == RocketLayer;
            bool hasOtherLayer = (CollisionMask & OtherLayers[index]) == OtherLayers[index];
            float distanceSqr = math.distancesq(RocketPos, OtherPositions[index]);
            float collisionDistanceSqr = math.pow(RocketRadius + OtherRadiuses[index], 2f);
            CollisionResult[index] = hasRoketLayer && hasOtherLayer && distanceSqr < collisionDistanceSqr; 
        }
    }

    [BurstCompile]
    struct RocketCollisionJob_V2 : IJobParallelFor
    {
        [ReadOnly] public PhysicsLayer CollisionMask;
        [ReadOnly] public Translation RocketPos;
        [ReadOnly] public SimpleSphereCollider RocketCollider;
        [ReadOnly] public BodyPhysicsLayer RocketLayer;
        [ReadOnly] public NativeArray<Translation> OtherPositions;
        [ReadOnly] public NativeArray<SimpleSphereCollider> OtherColliders;
        [ReadOnly] public NativeArray<BodyPhysicsLayer> OtherLayers;
        public NativeArray<bool> CollisionResult;

        public void Execute(int index)
        {
            bool hasRoketLayer = (CollisionMask & RocketLayer.Value) == RocketLayer.Value;
            bool hasOtherLayer = (CollisionMask & OtherLayers[index].Value) == OtherLayers[index].Value;
            float distanceSqr = math.distancesq(RocketPos.Value, OtherPositions[index].Value);
            float collisionDistanceSqr = math.pow(RocketCollider.Radius + OtherColliders[index].Radius, 2f);
            CollisionResult[index] = hasRoketLayer && hasOtherLayer && distanceSqr < collisionDistanceSqr;
        }
    }

    private EntityQuery _rocketsQuery;
    private EntityQuery _othersQuery;

    protected override void OnCreate()
    {
        EntityQueryDesc rocketQueryDesc = new EntityQueryDesc
        {
            All = new ComponentType[]
            {
                ComponentType.ReadOnly<RocketTag>(),
                ComponentType.ReadOnly<Translation>(),
                ComponentType.ReadOnly<BodyPhysicsLayer>(),
                ComponentType.ReadOnly<SimpleSphereCollider>(),
                ComponentType.ReadOnly<RocketBaseStats>()
            }
        };
        _rocketsQuery = GetEntityQuery(rocketQueryDesc);

        EntityQueryDesc othersQueryDesc = new EntityQueryDesc
        {
            None = new ComponentType[] { typeof(RocketTag) },
            All = new ComponentType[]
            {
                ComponentType.ReadOnly<Translation>(),
                ComponentType.ReadOnly<BodyPhysicsLayer>(),
                ComponentType.ReadOnly<SimpleSphereCollider>()
            }
        };
        _othersQuery = GetEntityQuery(othersQueryDesc);
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        if (GetSingleton<GamePause>().IsOn)
            return inputDependencies;

        
        var rocketEntities = _rocketsQuery.ToEntityArray(Allocator.TempJob);
        var rocketPositions = _rocketsQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
        var rocketColliders = _rocketsQuery.ToComponentDataArray<SimpleSphereCollider>(Allocator.TempJob);
        var rocketLayers = _rocketsQuery.ToComponentDataArray<BodyPhysicsLayer>(Allocator.TempJob);
        var rocketBaseStats = _rocketsQuery.ToComponentDataArray<RocketBaseStats>(Allocator.TempJob);

        
        var otherEntities = _othersQuery.ToEntityArray(Allocator.TempJob);
        var otherPositions = _othersQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
        var otherLayers = _othersQuery.ToComponentDataArray<BodyPhysicsLayer>(Allocator.TempJob);
        var othersColliders = _othersQuery.ToComponentDataArray<SimpleSphereCollider>(Allocator.TempJob);

        NativeArray<bool> collisionResult = new NativeArray<bool>(otherEntities.Length, Allocator.TempJob, NativeArrayOptions.ClearMemory);

        for (int i = 0; i < rocketEntities.Length; ++i)
        {
            var job = new RocketCollisionJob_V2
            {
                CollisionMask = PhysicsLayer.Planet | PhysicsLayer.Rocket,
                RocketPos = rocketPositions[i],
                RocketCollider = rocketColliders[i],
                RocketLayer = rocketLayers[i],
                OtherLayers = otherLayers,
                OtherColliders = othersColliders,
                OtherPositions = otherPositions,
                CollisionResult = collisionResult
            };

            var handle = job.Schedule(otherEntities.Length, 16);
            handle.Complete();

            bool collide = false;
            for (int j = 0; j < collisionResult.Length; ++j)
            {
                collide = collide || collisionResult[j];

                if (collisionResult[j])
                    EntityManager.AddComponentData(otherEntities[j], new TakeDamage { Value = rocketBaseStats[i].Damage });
                
                collisionResult[j] = false;
            }

            if (collide)
                EntityManager.DestroyEntity(rocketEntities[i]);
        }        

        rocketEntities.Dispose();
        rocketPositions.Dispose();
        rocketColliders.Dispose();
        rocketLayers.Dispose();
        rocketBaseStats.Dispose();

        otherEntities.Dispose();
        otherPositions.Dispose();
        othersColliders.Dispose();
        otherLayers.Dispose();        

        collisionResult.Dispose();        

        return inputDependencies;
    }
}