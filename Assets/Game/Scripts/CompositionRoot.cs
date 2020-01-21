using Unity.Entities;
using UnityEngine;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private PrefabsStorageGOScriptable _prefabsStorage;

    public static GameObject CanvasGO;
    public static PrefabsStorageGOScriptable PrefabsGO;

    private void Awake()
    {
        CanvasGO = _canvas;
        PrefabsGO = _prefabsStorage;

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        Entity entity = entityManager.CreateEntity(typeof(GamePause));
        entityManager.AddComponentData(entity, new GamePause { IsOn = false });
    }
}
