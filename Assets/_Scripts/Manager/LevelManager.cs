using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    static public LevelManager instance;

    [Header("Coin")]
    [SerializeField] private Transform[] coinPositions;
    [SerializeField] private EventHandlerScriptableObject _eventHandler;
    private int _coinsCollected;

    [Header("Reference")]
    [SerializeField] private Transform _player;
    private float _killPlayerHeight = -106f;

    AudioManager _audioManager;
    InputsManager _inputs;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _eventHandler.MaxCoinCount = coinPositions.Length;
        _audioManager = AudioManager.instance;
        _inputs = InputsManager.Instance;
        SetupCoins();
    }
    private void OnEnable()
    {
        _eventHandler.OnCollectingCoin.AddListener(AddCoinCount);
    }

    private void OnDisable()
    {
        _eventHandler.OnCollectingCoin.RemoveListener(AddCoinCount);
    }
    private void SetupCoins()
    {
        foreach(var pos in coinPositions)
        {
            Instantiate(_eventHandler.CoinPrefab, pos.position, Quaternion.identity);
        }
    }

    public void AddCoinCount()
    {
        _audioManager.PlaySound(Helper.COIN_SOUND);
        _coinsCollected++;

    }

    public int GetCoinCount()
    {
        return _coinsCollected;
    }
   
    void Update()
    {
        if(_inputs.Restart)
        {
            SceneManager.LoadScene(0);
        }

        if(_inputs.Quit)
        {
            Application.Quit();
        }

        if(_player.transform.position.y < _killPlayerHeight)
        {
            SceneManager.LoadScene(0);
        }
    }
}
