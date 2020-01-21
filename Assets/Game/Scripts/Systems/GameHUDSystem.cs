using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GameHUDSystem : ComponentSystem
{
    private GameHUD _gameHud;

    protected override void OnUpdate()
    {
        if (GetSingleton<GamePause>().IsOn)
            return;

        Entities.ForEach((Entity e, ref AddGameHUD addHud) =>
        {
            PrefabsStorageComponent prefabsStorage = GetSingleton<PrefabsStorageComponent>();
            GameObject hudObj = GameObject.Instantiate(CompositionRoot.PrefabsGO.GameHUD, CompositionRoot.CanvasGO.transform);
            _gameHud = hudObj.GetComponent<GameHUD>();

            PostUpdateCommands.DestroyEntity(e);
        });

        Entities.ForEach((ref PlayerTag pTag, ref NextRocketType type) =>
        {
            _gameHud?.UpdateHUD(type.Value);
        });        
    }
}
