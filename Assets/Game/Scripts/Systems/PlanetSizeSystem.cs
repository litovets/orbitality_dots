using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class PlanetSizeSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity e, ref PlanetSize size) =>
        {
            EntityManager.AddComponentData(e, new Scale { Value = size.Value });
        });
    }
}
