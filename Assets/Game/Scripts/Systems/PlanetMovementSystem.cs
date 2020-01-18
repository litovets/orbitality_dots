using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class PlanetMovementSystem : JobComponentSystem
{
    [BurstCompile]
    private struct MoveJob : IJobForEach<Translation, OrbitRadius, OrbitAngle, MovementSpeed>
    {
        public float DeltaTime;
        public void Execute(ref Translation translation, ref OrbitRadius orbitRadius, ref OrbitAngle orbitAngle, ref MovementSpeed speed)
        {
            orbitAngle.Value += DeltaTime * speed.Value;
            translation.Value = new float3(math.cos(orbitAngle.Value), 0f, math.sin(orbitAngle.Value)) * orbitRadius.Value;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        MoveJob job = new MoveJob { DeltaTime = Time.DeltaTime };
        return job.Schedule(this, inputDeps);

    }
}
