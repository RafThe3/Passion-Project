using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private bool isInvincible = false;
    [Min(0), SerializeField] private int startingHealth = 1;
    [Min(0), SerializeField] private int maxHealth = 100;
    [Min(0), SerializeField] private int startingHealthPacks = 1;
    [Min(0), SerializeField] private int maxHealthPacks = 1;
    [Min(0), SerializeField] private float healInterval = 1;

    [Header("UI")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI healthPacksText;

    //Internal Variables
    private int currentHealth = 0;
    private int healthPacks = 0;
    private float healTimer = 0;
    //private Animator animator;
    private CharacterController character;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
        character = GetComponent<CharacterController>();
    }

    private void Start()
    {
        if (startingHealth > maxHealth)
        {
            startingHealth = maxHealth;
        }
        currentHealth = startingHealth;
        healthPacks = startingHealthPacks;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }

    private void Update()
    {
        healTimer += Time.deltaTime;
        UpdateUI();

        bool isMoving = Mathf.Abs(character.velocity.z) > Mathf.Epsilon;
        //animator.SetBool("isMoving", isMoving);

        //test
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Heal(10);
        }
    }

    private void UpdateUI()
    {
        healthBar.value = currentHealth;
        healthText.text = $"Health: {currentHealth} / {maxHealth}";

        /*
        Ray cameraDirection = new(Camera.main.transform.position, Camera.main.transform.forward);
        crosshair.color = Physics.Raycast(cameraDirection, out RaycastHit hit) && hit.collider.CompareTag("Enemy") ? Color.red
                        : hit.collider.CompareTag("Player") ? Color.blue
                        : Color.white;
        */
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            return;
        }

        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int health)
    {
        if (currentHealth < maxHealth && healTimer >= healInterval && healthPacks > 0)
        {
            currentHealth += health;
            healthPacks--;
            healTimer = 0;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //GetComponent<SaveSystem>().Load();
    }

    public void AddHealthPack()
    {
        if (healthPacks < maxHealthPacks)
        {
            healthPacks++;
        }

        if (healthPacks > maxHealthPacks)
        {
            healthPacks = maxHealthPacks;
        }
    }

    public void SetMaxHealth(int health)
    {
        maxHealth = health;
    }

    public int GetMaxHealth => maxHealth;

    public int GetCurrentHealth => currentHealth;
}
