using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class PlanetSizeSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity e, ref PlanetSize size) =>
        {
            /*EntityManager.SetComponentData(e, new CompositeScale
            {
                Value = new float4x4(
                new float4(size.Value, 0f, 0f, 0f),
                new float4(0f, size.Value, 0f, 0f),
                new float4(0f, 0f, size.Value, 0f),
                new float4(0f, 0f, 0f, 1f)
                )
            });*/
            EntityManager.AddComponentData(e, new Scale { Value = size.Value });
        });
    }
}
