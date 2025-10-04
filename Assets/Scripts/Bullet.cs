using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _speed = 20f;
    public float _lifeTime = 5f;
    public int _damage = 10;

    private Rigidbody _rb;
    private float _spawnTime;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        _spawnTime = Time.time;
        _rb.linearVelocity = transform.forward * _speed; 
    }

    void Update()
    {
        if (Time.time - _spawnTime >= _lifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false);
    }
}

