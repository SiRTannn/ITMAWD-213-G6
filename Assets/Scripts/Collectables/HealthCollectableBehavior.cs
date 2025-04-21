using UnityEngine;

public class HealthCollectableBehavior : MonoBehaviour, ICollectableBehavior
{
    [SerializeField]
    private float _healthAmount;


    public void OnCollected(GameObject player)
    {
        Debug.Log("Collected called!");
        player.GetComponent<PlayerHealth>().AddHealth(_healthAmount);
    }
}
