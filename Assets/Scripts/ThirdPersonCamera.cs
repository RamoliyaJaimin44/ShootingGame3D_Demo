using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform _target;
    public Vector3 _offset = new Vector3(0, 5, -7);
    public float _followSpeed = 10f;
    public float _rotationSpeed = 5f;

    void LateUpdate()
    {
        if (_target == null) return;

        Vector3 desiredPosition = _target.position + _target.TransformDirection(_offset);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, _followSpeed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(_target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
}

