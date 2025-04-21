    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Sword : MonoBehaviour
    {
        [SerializeField] private GameObject slashAnimPrefab;
        [SerializeField] private Transform slashAnimSpawnPoint;
        [SerializeField] private Transform weaponCollider;

        private PlayerControls playerControls;
        private Animator myAnimator;
        private PlayerController playerController;
        private ActiveWeapon activeWeapon;

        private GameObject slashAnim;

        private void Awake()
        {
            playerController = GetComponentInParent<PlayerController>();
            activeWeapon = GetComponentInParent<ActiveWeapon>();
            myAnimator = GetComponent<Animator>();
            playerControls = new PlayerControls();
        }

        private void OnEnable()
        {
            playerControls.Enable();
        }

        void Start()
        {
            playerControls.Combat.Attack.started += _ => Attack();
        }

    private void Update()
    {
        if (PauseMenu.GameIsPaused) return;
        MouseFollowWithOffset();
    }

    private void Attack()
    {
        if (PauseMenu.GameIsPaused) return;

        if (myAnimator == null)
        {
            Debug.LogError("Animator is missing or destroyed.");
            return;
        }

        myAnimator.SetTrigger("Attack");

        if (weaponCollider != null)
        {
            weaponCollider.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Weapon collider is not set.");
        }

        if (slashAnimPrefab != null && slashAnimSpawnPoint != null)
        {
            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            slashAnim.transform.parent = this.transform.parent;
        }
        else
        {
            Debug.LogWarning("Slash animation prefab or spawn point is not set.");
        }
    }



    public void DoneAttackingAnimEvent()
        {
            weaponCollider.gameObject.SetActive(false);
        }


        public void SwingUpFlipAnimEvent()
        {
            slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

            if (playerController.FacingLeft)
            {
                slashAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        public void SwingDownFlipAnimEvent()
        {
            slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (playerController.FacingLeft)
            {
                slashAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        private void MouseFollowWithOffset()
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

            if (mousePos.x < playerScreenPoint.x)
            {
                activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
                weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            else
            {
                activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
                weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }