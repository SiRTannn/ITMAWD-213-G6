using UnityEngine;

public class PortalConroller : MonoBehaviour
{
    public Transform desination;
    GameObject player;
    Animation anim;
    Rigidbody2D playerRb;

    private INGAMEAM ingameAudioManager;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ingameAudioManager = FindObjectOfType<INGAMEAM>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Vector2.Distance(player.transform.position, transform.position) > 0.3f)
            {
                player.transform.position = desination.transform.position;

                if (ingameAudioManager != null && ingameAudioManager.portal != null)
                {
                    ingameAudioManager.PlaySFX(ingameAudioManager.portal);
                }
            }
        }
    }
}
