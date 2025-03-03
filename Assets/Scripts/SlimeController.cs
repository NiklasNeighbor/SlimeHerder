using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class SlimeController : MonoBehaviour
{
    [SerializeField] private float _maxMovementDistance;
    [SerializeField] private float _minMovementDistance;
    [SerializeField] private float _destinationChangeTime;

    [SerializeField] private float _movementForce;
    [SerializeField] private float _movementForceMultiplier = 1.01f;

    private Rigidbody2D _rigidbody;
    private Vector3 _destination;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        StartCoroutine(SetDestination());
        StartCoroutine(SpeedUpCoroutine());
    }

    private IEnumerator SetDestination()
    {
        while (gameObject.activeSelf)
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            Vector2 position = new(transform.position.x, transform.position.y);
            Debug.Log(direction);
            _destination = direction * Random.Range(_minMovementDistance, _maxMovementDistance) + position;
            yield return new WaitForSeconds(_destinationChangeTime);
        }
        
    }
    
    private IEnumerator SpeedUpCoroutine()
    {
        while (true)
        {
            _movementForce *= _movementForceMultiplier;
            yield return new WaitForSeconds(1f);
        }
    }


    void FixedUpdate()
    {
        MoveToDestination();
    }

    private void MoveToDestination()
    {
        Vector2 direction = _destination - transform.position;
        if (direction.sqrMagnitude <= 0.5f)
            return;
        
        _rigidbody.AddForce(direction.normalized * (_movementForce * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            SlimeSpawner.Instance.RemoveSlime(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_destination, 0.2f);
    }
}
