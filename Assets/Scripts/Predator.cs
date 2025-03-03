using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Predator : MonoBehaviour
{
    public float DetectionRadius = 2f;
    public float PredatorSpeed = 1;
    public float PredatorSpeedMultiplier = 1.02f;
    public LayerMask DefenseLayer;
    private Collider2D lastSlimeCollider;
    Vector2 currentTargetDirection;

    float lastPathfindTime;
    float PathCooldown = 2;
    float PathCooldownVariety;

    float LastScared;
    float ScareCooldown = 0.5f;
    Vector2 DebugWanderTarget;

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        StartCoroutine(SpeedUpCoroutine());
    }

    private IEnumerator SpeedUpCoroutine()
    {
        while (true)
        {
            PredatorSpeed *= PredatorSpeedMultiplier;
            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        PickMovement();
    }

    void RandomizeCooldown()
    {
        PathCooldownVariety = Random.Range(-PathCooldown / 2, PathCooldown / 2);
    }

    void PickMovement()
    {
        Transform slimePosition = null;
        int layerMask = 1 << LayerMask.NameToLayer("Slime");
        var colliders = Physics2D.OverlapCircleAll(transform.position, DetectionRadius, layerMask);

        float smallestMagnitude = Mathf.Infinity;
        foreach (var collider in colliders)
        {
            if ((collider.transform.position - transform.position).sqrMagnitude < smallestMagnitude)
            {
                slimePosition = collider.transform;
                smallestMagnitude = (collider.transform.position - transform.position).sqrMagnitude;
            }
        }


        Transform defensePosition = null;
        defensePosition = Physics2D.OverlapCircle(transform.position, DetectionRadius, DefenseLayer)?.transform;
        //Debug.Log(slimePosition);

        if (defensePosition != null)
        {
            currentTargetDirection = defensePosition.position - transform.position;
            Debug.DrawLine(transform.position, defensePosition.position, Color.red);
            HeadTowards(-currentTargetDirection);
            LastScared = Time.time;
        }
        else
        {
            if(Time.time > LastScared + ScareCooldown)
            {
                if (slimePosition != null)
                {
                    currentTargetDirection = slimePosition.position - transform.position;
                    Debug.DrawLine(transform.position, slimePosition.position, Color.red);
                    HeadTowards(currentTargetDirection);
                }
                else
                {
                    Wander();
                }
            } else
            {
                HeadTowards(-currentTargetDirection);
            }
        }


    }

    void Wander()
    {
        
        if (Time.time > lastPathfindTime + PathCooldown + PathCooldownVariety)
        {
            Vector2 WanderTarget = Random.insideUnitCircle * DetectionRadius;
            currentTargetDirection = new Vector2(transform.position.x, transform.position.y) + WanderTarget;
            DebugWanderTarget = WanderTarget;
            
            lastPathfindTime = Time.time;
            RandomizeCooldown();
        }

        Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + DebugWanderTarget, Color.green);
        HeadTowards(DebugWanderTarget - new Vector2(transform.position.x, transform.position.y));
    }

    void HeadTowards(Vector2 target)
    {
        if (target.magnitude > 0.1 )
        {
            _rigidbody2D.linearVelocity = target.normalized * PredatorSpeed * Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
    }
}
