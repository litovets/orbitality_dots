using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class RocketGravitySystem : ComponentSystem
{
    private const double G = 6.67e-11f;

    protected override void OnUpdate()
    {
        Entities.ForEach((Entity e, ref Translation rocketTranslation, ref Velocity vel, ref RocketBaseStats stats) =>
        {
            float3 rocketPos = rocketTranslation.Value;
            float3 rocketVel = vel.Value;
            float rocketMass = stats.Weight;

            Entities.WithNone<RocketTag>().ForEach((ref Translation bodyTranslation, ref PhysicsMass bodyMass) =>
            {
                float distance = math.distance(rocketPos, bodyTranslation.Value);
                float gravityForce = (float)(G * rocketMass / (bodyMass.InverseMass * distance * distance));
                float3 gravityDir = math.normalize(bodyTranslation.Value - rocketPos);
                rocketVel += gravityDir * gravityForce * Time.DeltaTime;
            });

            EntityManager.SetComponentData(e, new Velocity { Value = rocketVel });
        });
    }
}