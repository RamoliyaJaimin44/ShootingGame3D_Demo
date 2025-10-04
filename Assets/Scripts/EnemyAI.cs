using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection Settings")]
    public float _attackRange = 10f;
    public float _fieldOfView = 60f;      

    [Header("Firing Settings")]
    public float _fireRate = 1.2f;
    public Transform _firePoint;
    public GameObject _bulletPrefab;      
    public int _poolSize = 10;     
    public Transform _bulletParent;

    [Header("Rotation Settings")]
    public float _rotationSpeed = 5f;

    private Transform _player;
    private GameObject[] _bulletPool;
    private int _currentIndex = 0;
    private float _nextFireTime = 0f;

    public int _health = 50;
    public int _damege = 33;
    public event Action onDeath;

    void Start()
    {
        _player = WaveSpawner.Instance._player.gameObject.transform;

        _bulletPool = new GameObject[_poolSize];
        for (int i = 0; i < _poolSize; i++)
        {
            _bulletPool[i] = Instantiate(_bulletPrefab, _bulletParent);
            _bulletPool[i].SetActive(false);
        }
    }

    void Update()
    {
        if (_player == null) return;

        Vector3 directionToPlayer = (_player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(_player.position, transform.position);

        float dot = Vector3.Dot(transform.forward, directionToPlayer);

        float cosAngle = Mathf.Cos(_fieldOfView * 0.5f * Mathf.Deg2Rad);

        if (distanceToPlayer <= _attackRange && dot >= cosAngle)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            if (Time.time >= _nextFireTime)
            {
                Fire();
                _nextFireTime = Time.time + _fireRate;
            }
        }
    }

    void Fire()
    {
        GameObject bullet = _bulletPool[_currentIndex];
        bullet.transform.position = _firePoint.position;
        bullet.transform.rotation = _firePoint.rotation;
        bullet.SetActive(true);

        _currentIndex = (_currentIndex + 1) % _poolSize;
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;
        Debug.Log("Enemy Damege ... Health : " + _health);
        if (_health <= 0)
        {
            onDeath?.Invoke();
            Debug.Log("Enemy Destroy ...");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(_damege);
        }
    }
}
