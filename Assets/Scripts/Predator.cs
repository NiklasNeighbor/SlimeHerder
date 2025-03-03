using UnityEngine;

public class Predator : MonoBehaviour
{
    public float DetectionRadius = 2f;
    public float PredatorSpeed = 1;
    Vector2 currentTargetDirection;

    float lastPathfindTime;
    float PathCooldown = 2;
    float PathCooldownVariety;
    Vector2 DebugWanderTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        slimePosition = Physics2D.OverlapCircle(transform.position, DetectionRadius, layerMask)?.transform;
        //Debug.Log(slimePosition);

        if (slimePosition != null)
        {
            currentTargetDirection = slimePosition.position - transform.position;
            Debug.DrawLine(transform.position, slimePosition.position, Color.red);
            HeadTowards(currentTargetDirection);
        } else
        {
            Wander();
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
        if (target.magnitude > 0.1)
        {
            transform.Translate(target.normalized * PredatorSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
    }
}
