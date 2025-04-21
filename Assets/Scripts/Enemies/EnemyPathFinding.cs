using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Knockback knockback;

    private bool targetDead = false;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (knockback.gettingKnockedBack || targetDead) return;

        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
    }

    public void MoveTo(Vector2 targetPosition)
    {
        if (targetDead) return;

        moveDir = (targetPosition - rb.position).normalized;
    }

    public void StopTrackingPlayer()
    {
        targetDead = true;
        moveDir = Vector2.zero;
    }
}
