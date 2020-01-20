using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private GameObject _selectPlayerCountPanel;
    [SerializeField] private Slider _playersCountSlider;
    [SerializeField] private Text _playersCountLabel;

    private void Start()
    {
        _startButton.onClick.AddListener(() =>
        {
            _startButton.gameObject.SetActive(false);
            _selectPlayerCountPanel.SetActive(true);
        });

        _startGameButton.onClick.AddListener(OnStartGameClick);

        _playersCountSlider.onValueChanged.AddListener(val => _playersCountLabel.text = val.ToString("#"));
    }

    private void OnStartGameClick()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        Entity entity = entityManager.CreateEntity(typeof(GamePause));
        entityManager.AddComponentData(entity, new GamePause { IsOn = false });

        int playersCount = (int)_playersCountSlider.value;        
        entity = entityManager.CreateEntity(typeof(CreatePlayers));
        entityManager.SetComponentData(entity, new CreatePlayers { Value = playersCount });

        entity = entityManager.CreateEntity(typeof(AddGameHUD));
        entityManager.AddComponentData(entity, new AddGameHUD());

        gameObject.SetActive(false);
    }
}
