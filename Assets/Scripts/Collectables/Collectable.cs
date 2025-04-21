using UnityEngine;

public class Collectable : MonoBehaviour
{
    private ICollectableBehavior _collectableBehavior;

    private void Awake()
    {
        _collectableBehavior = GetComponent<HealthCollectableBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            _collectableBehavior.OnCollected(player.gameObject);
            Destroy(gameObject);
        }
    }
}
