using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateBefore(typeof(BuildPhysicsWorld))]
public class ChangeSphereColliderRadiusSystem : JobComponentSystem
{
    [BurstCompile]
    struct ChangeSphereColliderRadiusJob : IJobForEach<PhysicsCollider, ChangeSphereColliderRadius>
    {
        public unsafe void Execute(ref PhysicsCollider collider, [ReadOnly] ref ChangeSphereColliderRadius radius)
        {
            if (collider.ColliderPtr->Type != ColliderType.Sphere)
            {
                /*BlobAssetReference<Collider> colliderBlob = SphereCollider.Create(
                    new SphereGeometry { Center = new Unity.Mathematics.float3(), Radius = radius.Value }, 
                    new CollisionFilter { CollidesWith = ~0u }, new Material { Flags = Material.MaterialFlags.IsTrigger }
                    );
                collider.Value = colliderBlob;*/
                return;
            }

            SphereCollider* colPtr = (SphereCollider*)collider.ColliderPtr;
            var sphereGeometry = colPtr->Geometry;
            sphereGeometry.Radius = radius.Value;
            colPtr->Geometry = sphereGeometry;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new ChangeSphereColliderRadiusJob();
        return job.Schedule(this, inputDependencies);
    }
}