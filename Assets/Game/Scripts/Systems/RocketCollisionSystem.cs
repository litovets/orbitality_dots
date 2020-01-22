using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class RocketCollisionSystem : JobComponentSystem
{
    [BurstCompile]
    struct RocketTriggerJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<RocketBaseStats> rocketEntities;
        public EntityCommandBuffer.Concurrent CommandBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {
            bool isRocketA = rocketEntities.HasComponent(triggerEvent.Entities.EntityA);
            bool isRocketB = rocketEntities.HasComponent(triggerEvent.Entities.EntityB);

            if (!isRocketA && !isRocketB)
                return;

            Entity rocketEntity = isRocketA ? triggerEvent.Entities.EntityA : triggerEvent.Entities.EntityB;
            Entity bodyEntity = isRocketB ? triggerEvent.Entities.EntityA : triggerEvent.Entities.EntityB;

            int damage = rocketEntities[rocketEntity].Damage;
            CommandBuffer.AddComponent(0, bodyEntity, new TakeDamage { Value = damage });

            CommandBuffer.DestroyEntity(0, rocketEntity);
        }
    }

    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        if (GetSingleton<GamePause>().IsOn)
            return inputDependencies;

        var job = new RocketTriggerJob
        {
            rocketEntities = GetComponentDataFromEntity<RocketBaseStats>(),
            CommandBuffer = World.GetOrCreateSystem<EntityCommandBufferSystem>().CreateCommandBuffer().ToConcurrent()
        };

        var handle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDependencies);

        handle.Complete();

        return inputDependencies;
    }
}