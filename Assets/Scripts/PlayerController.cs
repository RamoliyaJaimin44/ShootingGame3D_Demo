using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float _moveSpeed = 5f;
    public float _rotationSpeed = 100f;

    private Rigidbody _rb;

    public float _dashDistance = 5f;
    public float _dashDuration = 0.3f;
    public float _dashCooldown = 3f;

    private bool _isDashing = false;
    private bool _canDash = true;

    public GameObject _bulletPrefab;
    public Transform _firePoint;
    public int _poolSize = 20;
    public float _fireRate = 0.2f;
    public Transform _bulletParent;

    public int _health = 100;
    public int _damege = 33;

    public TextMeshProUGUI _healthText;
    public GameObject _gameOverPanel;

    private GameObject[] _bullets;
    private int _currentIndex = 0;
    private float _nextFireTime = 0f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _bullets = new GameObject[_poolSize];
        for (int i = 0; i < _poolSize; i++)
        {
            _bullets[i] = Instantiate(_bulletPrefab, _bulletParent);
            _bullets[i].SetActive(false);
        }
        _healthText.text = "Health : " + _health;
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float rotateInput = Input.GetAxis("Horizontal");

        Vector3 moveDirection = transform.forward * moveInput * _moveSpeed;
        _rb.MovePosition(_rb.position + moveDirection * Time.fixedDeltaTime);

        Quaternion turnRotation = Quaternion.Euler(0f, rotateInput * _rotationSpeed * Time.fixedDeltaTime, 0f);
        _rb.MoveRotation(_rb.rotation * turnRotation);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
        {
            StartCoroutine(Dash());
        }
        if (Input.GetMouseButton(0) && Time.time >= _nextFireTime)
        {
            Fire();
            _nextFireTime = Time.time + _fireRate;
        }
    }

    IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;

        float dashSpeed = _dashDistance / _dashDuration;

        float startTime = Time.time;
        while (Time.time < startTime + _dashDuration)
        {
            _rb.MovePosition(_rb.position + transform.forward * dashSpeed * Time.deltaTime);
            yield return null;
        }

        _isDashing = false;

        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }

    void Fire()
    {

        GameObject bullet = _bullets[_currentIndex];
        bullet.transform.position = _firePoint.position;
        bullet.transform.rotation = _firePoint.rotation;
        bullet.SetActive(true);

        _currentIndex = (_currentIndex + 1) % _poolSize;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(_damege);
        }
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;
        Debug.Log("Player Damage ... Health :" + _health);
        _healthText.text = "Health : " + _health.ToString();
        if (_health <= 0)
        {
            Debug.Log("Game over ...");
            gameObject.SetActive(false);
            _gameOverPanel.SetActive(true);
            _health = 0;
            _healthText.text = "Health : " + _health.ToString();
        }
    }
}
