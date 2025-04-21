using UnityEngine;

public class EnemyCollectableDrop : MonoBehaviour
{
    [SerializeField]
    private float _chanceOfCollectableDrop;

    private CollectableSpawner _collectableSpawner;

    private void Awake()
    {
        _collectableSpawner = FindFirstObjectByType<CollectableSpawner>();  
    }

    public void RandomlyDropColectable()
    {
        float random = Random.Range(0f, 1f);

        if (_chanceOfCollectableDrop >= random)
        {
            _collectableSpawner?.SpawnCollectable(transform.position);
        }
    }
}
