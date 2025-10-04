using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;

    [Header("Wave Settings")]
    public int _gameWaveCount = 10;
    public float _spawnRadius = 100f;
    public GameObject _enemyPrefab;
    public Transform _enemyParent;

    [Header("References")]
    public Transform _player;
    public TextMeshProUGUI _waveText;
    public GameObject _gameWinPanel;


    private int _currentWave = 0;
    private List<GameObject> _aliveEnemies = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartNextWave();
    }

    void Update()
    {
        if (_aliveEnemies.Count == 0 && _currentWave < _gameWaveCount)
        {
            StartNextWave();
        }
        else if (_aliveEnemies.Count == 0 && _currentWave >= _gameWaveCount)
        {
            Victory();
        }
    }

    void StartNextWave()
    {
        _currentWave++;
        int enemyCount = _currentWave * 5;

        if (_waveText != null)
            _waveText.text = "Wave: " + _currentWave + "/" + _gameWaveCount;

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPos = _player.position + Random.insideUnitSphere * _spawnRadius;
            spawnPos.y = _player.position.y; 
            Quaternion randomYRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            GameObject enemy = Instantiate(_enemyPrefab, spawnPos, randomYRotation, _enemyParent);
            _aliveEnemies.Add(enemy);

            EnemyAI enemyHealth = enemy.GetComponent<EnemyAI>();
            if (enemyHealth != null)
            {
                enemyHealth.onDeath += () => { _aliveEnemies.Remove(enemy); };
            }
        }
    }

    void Victory()
    {
        if (_waveText != null)
            _waveText.text = " All Waves Completed ";
        Debug.Log("Player Wins!");
        _gameWinPanel.SetActive(true);
    }
}
