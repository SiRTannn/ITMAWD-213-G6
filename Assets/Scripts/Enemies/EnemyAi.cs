    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EnemyAI : MonoBehaviour
    {
        private enum State
        {
            Roaming,
            Chasing
        }

        private State state;
        private EnemyPathfinding enemyPathfinding;
        private Transform playerTransform;

        [SerializeField] private float chaseRange = 5f; // Distance to start chasing the player

    

    private void Awake()
        {
            enemyPathfinding = GetComponent<EnemyPathfinding>();
            state = State.Roaming;
            
    }

        private void Start()
        {

        

        // Find player by tag; ensure the player GameObject has the "Player" tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }

            StartCoroutine(BehaviorRoutine());
        }

        private IEnumerator BehaviorRoutine()
        {
            while (true)
            {
                switch (state)
                {
                    case State.Roaming:
                        yield return RoamingBehavior();
                        break;

                    case State.Chasing:
                        yield return ChasingBehavior();
                        break;
                }

                yield return null; // Wait for the next frame
            }
        }

        private IEnumerator RoamingBehavior()
        {
            Vector2 roamPosition = GetRoamingPosition();
            enemyPathfinding.MoveTo(roamPosition);

            float roamDuration = 2f; // Roam for 2 seconds
            float elapsed = 0f;

            while (elapsed < roamDuration)
            {
                elapsed += Time.deltaTime;

                // Check for player within chase range
                if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) < chaseRange)
                {
                    state = State.Chasing;
                    yield break;
                }

                yield return null;
            }
        }

        private IEnumerator ChasingBehavior()
        {
            while (state == State.Chasing)
            {
                if (playerTransform == null)
                {
                    state = State.Roaming;
                    yield break;
                }

                enemyPathfinding.MoveTo(playerTransform.position);

                // Exit chase if player is out of range
                if (Vector3.Distance(transform.position, playerTransform.position) > chaseRange)
                {
                    state = State.Roaming;
                    yield break;
                }

                yield return null; // Wait for the next frame
            }
        }

        private Vector2 GetRoamingPosition()
        {
            Vector2 roamOffset = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f)); // Adjust range as needed
            return (Vector2)transform.position + roamOffset;
        }
    }