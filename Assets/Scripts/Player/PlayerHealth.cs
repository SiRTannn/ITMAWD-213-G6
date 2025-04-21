using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthBar;

    private bool isDead;
    private Animator animator;

    public GameManagerScript gameManager;

    void Start()
    {
        maxHealth = health;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp01(health / maxHealth);

        if (health <= 0 && !isDead)
        {
            isDead = true;

            animator.SetBool("IsDead", true);

            // Stop player movement
            PlayerController.Instance.Die();

            // Stop all enemies from tracking player
            EnemyPathfinding[] enemies = FindObjectsOfType<EnemyPathfinding>();
            foreach (EnemyPathfinding enemy in enemies)
            {
                enemy.StopTrackingPlayer();
            }

            gameManager.gameOver();

            Debug.Log("Player is dead");

            // Wait for animation to finish before destroying
            StartCoroutine(HandleDeath());
        }
    }

    IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(1.5f); // match with PlayerDie animation length
        Destroy(gameObject); // or SetActive(false)
    }

    public void AddHealth(float healthAmount)
    {
        if (isDead) return;

        health += healthAmount;
        health = Mathf.Clamp(health, 0, maxHealth);

        healthBar.fillAmount = Mathf.Clamp01(health / maxHealth);
    }
}
