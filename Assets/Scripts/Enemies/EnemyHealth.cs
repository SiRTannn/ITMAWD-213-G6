using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 100;
    private int currentHealth;

    private Knockback knockback;
    private Flash flash;
    [SerializeField] private FloatingHealthBar healthBar;

    private EnemyCollectableDrop collectableDrop;

    private INGAMEAM audioManager;

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();

        currentHealth = startingHealth;
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();

        collectableDrop = GetComponent<EnemyCollectableDrop>(); 
    }

    private void Start()
    {
        healthBar.UpdateHealthBar(currentHealth, startingHealth);
        audioManager = FindObjectOfType<INGAMEAM>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, startingHealth);

        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.enemyDamage);
        }

        DetectDeath();

        knockback.GetKnockedBack(PlayerController.Instance.transform, 15f);
        StartCoroutine(flash.FlashRoutine());
    }

    private void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            collectableDrop?.RandomlyDropColectable();
            Destroy(gameObject);
        }
    }
}
