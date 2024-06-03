using System;
using UnityEngine;

public class DiskController : MonoBehaviour
{
    private float _throwPower;
    private float _borderSize;
    private float _stopThreshHold;
    private Vector3 _startingDiskPos;
    private bool _isHolding;
    private Camera _camera;
    private Rigidbody _rigidbody;
    private bool _isReleased;
    private float _previousFrameVelocity;
    private Animator _animator;

    public void Init(float throwPower, float borderSize, float stopThreshHold)
    {
        _throwPower = throwPower;
        _borderSize = borderSize;
        _stopThreshHold = stopThreshHold;
    }

    private void Start()
    {
        _isHolding = true;
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
        _startingDiskPos = transform.position;
        _animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (_isReleased)
        {
            if (_rigidbody.velocity.magnitude < _stopThreshHold &&
                _previousFrameVelocity > _rigidbody.velocity.magnitude)
            {
                _animator.SetBool("Slowing", false);
            }

            _previousFrameVelocity = _rigidbody.velocity.magnitude;

            return;
        }

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonUp(0))
        {
            _isHolding = false;
            OnRelease(ray);
            _animator.SetBool("Pull", false);
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (_isHolding)
            {
                Physics.Raycast(ray, out var hit);
                Move(hit);
            }
        }
    }

    private void Move(RaycastHit hit)
    {
        var delta = hit.point - _startingDiskPos;
        var rayPosition = Mathf.Min(delta.magnitude, _borderSize) * delta.normalized + _startingDiskPos;
        rayPosition.y = _startingDiskPos.y;
        transform.position = Vector3.Lerp(transform.position, rayPosition, Time.deltaTime * 10);
    }

    private void OnRelease(Ray ray)
    {
        _isReleased = true;
        if (Physics.Raycast(ray, out var hit, 50, LayerMask.GetMask("RedDiskLayer", "BlueDiskLayer")))
        {
            var deltaPos = _startingDiskPos - hit.point;
            var pushForceVector = Mathf.Min(deltaPos.magnitude, _borderSize) * deltaPos.normalized;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(
                (pushForceVector / _borderSize).sqrMagnitude * _throwPower * pushForceVector.normalized,
                ForceMode.Impulse
            );
        }
    }

    private void DiskDestroy()
    {
        Destroy(gameObject);
    }
}