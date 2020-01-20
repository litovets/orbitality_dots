using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerInputSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var pause = GetSingleton<GamePause>();

        if (!pause.IsOn && Input.GetMouseButtonDown(0))
        {            
            Entities.ForEach((Entity e, ref PlayerTag player, ref Translation translation) =>
            {
                var playerPos = Camera.main.WorldToScreenPoint(new Vector3(translation.Value.x, translation.Value.y, translation.Value.z));
                var mouseClickPos = Input.mousePosition;
                Vector3 dir = mouseClickPos - playerPos;
                EntityManager.AddComponentData(e, new ShootDirectionInput { Value = math.normalize(new float3(dir.x, 0f, dir.y)) });
            });
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause.IsOn = !pause.IsOn;
            SetSingleton(pause);
        }
    }
}
