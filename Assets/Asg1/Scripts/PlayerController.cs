/*
 * Author: Kelvin Teo
 * Date: 14 June 2026
 * Description: Controls collectibles, score, health, lava damage, goal area, UI, respawn, and BGM.
 */

using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// Stores the collectible currently within interaction range.
    /// </summary>
    GameObject currentCollectable;

    /// <summary>
    /// Stores the door currently within interaction range.
    /// </summary>
    DoorController currentDoor;

    /// <summary>
    /// Tracks the player's current score.
    /// </summary>
    public int currentScore = 0;

    /// <summary>
    /// Tracks the number of collectibles collected by the player.
    /// </summary>
    public int collectedCount = 0;

    /// <summary>
    /// Minimum score required to clear the level.
    /// </summary>
    public int targetScore = 170;

    /// <summary>
    /// Maximum possible score obtainable in the level.
    /// </summary>
    public int maxScore = 260;

    /// <summary>
    /// Maximum health of the player.
    /// </summary>
    public int maxHealth = 100;

    /// <summary>
    /// Current health of the player.
    /// </summary>
    public float currentHealth = 100f;

    /// <summary>
    /// Amount of damage dealt by lava every second.
    /// </summary>
    public float lavaDamagePerSecond = 25f;

    /// <summary>
    /// Tracks whether the player is dead.
    /// </summary>
    bool isDead = false;

    /// <summary>
    /// Tracks whether the level has been cleared.
    /// </summary>
    bool levelCleared = false;

    /// <summary>
    /// Stores the player's starting position for respawning.
    /// </summary>
    Vector3 startPosition;

    /// <summary>
    /// Stores all collectibles in the scene.
    /// </summary>
    Collectible[] allCollectibles;

    /// <summary>
    /// Background music audio source.
    /// </summary>
    public AudioSource bgmSource;

    /// <summary>
    /// Displays the player's current score.
    /// </summary>
    public TextMeshProUGUI scoreText;

    /// <summary>
    /// Displays the remaining points needed to reach the target score.
    /// </summary>
    public TextMeshProUGUI pointsLeftText;

    /// <summary>
    /// Displays the number of collectibles left in the level.
    /// </summary>
    public TextMeshProUGUI collectiblesLeftText;

    /// <summary>
    /// Displays the player's current health.
    /// </summary>
    public TextMeshProUGUI healthText;

    /// <summary>
    /// Displays gameplay instructions.
    /// </summary>
    public TextMeshProUGUI instructionText;

    /// <summary>
    /// Displays movement controls.
    /// </summary>
    public TextMeshProUGUI controlsText;

    /// <summary>
    /// Displays game messages such as level clear and death notifications.
    /// </summary>
    public TextMeshProUGUI messageText;

    /// <summary>
    /// Initializes variables, UI text, collectibles, and background music.
    /// </summary>
    void Start()
    {
        startPosition = transform.position;
        currentHealth = maxHealth;

        allCollectibles = FindObjectsByType<Collectible>(FindObjectsSortMode.None);

        if (bgmSource != null)
        {
            bgmSource.Play();
        }

        if (instructionText != null)
        {
            instructionText.text = "Collect crystals and coins to gain points!";
        }

        if (controlsText != null)
        {
            controlsText.text = "Use WASD or Arrow Keys to move";
        }

        if (messageText != null)
        {
            messageText.text = "";
        }

        UpdateUI();
    }

    /// <summary>
    /// Detects when the player enters trigger areas such as collectibles, doors, and goal area.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (isDead || levelCleared)
        {
            return;
        }

        if (other.gameObject.tag == "Collectible")
        {
            currentCollectable = other.gameObject;
        }

        if (other.gameObject.tag == "Door")
        {
            currentDoor = other.GetComponent<DoorController>();
        }

        if (other.gameObject.tag == "GoalArea")
        {
            CheckGoalArea();
        }
    }

    /// <summary>
    /// Continuously checks for lava damage while the player remains inside the lava trigger.
    /// </summary>
    void OnTriggerStay(Collider other)
    {
        if (isDead || levelCleared)
        {
            return;
        }

        if (other.gameObject.tag == "Lava")
        {
            TakeDamage(lavaDamagePerSecond * Time.deltaTime);
        }
    }

    /// <summary>
    /// Removes references when the player exits collectible or door trigger areas.
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentCollectable)
        {
            currentCollectable = null;
        }

        if (other.GetComponent<DoorController>() == currentDoor)
        {
            currentDoor = null;
        }
    }

    /// <summary>
    /// Handles interaction with collectibles and doors.
    /// </summary>
    void OnInteract()
    {
        if (isDead || levelCleared)
        {
            return;
        }

        if (currentCollectable != null)
        {
            Collectible collectibleScript = currentCollectable.GetComponent<Collectible>();

            collectedCount++;
            currentScore += collectibleScript.score;

            collectibleScript.Collect();

            currentCollectable = null;

            UpdateUI();
        }

        if (currentDoor != null)
        {
            currentDoor.Interact();
        }
    }

    /// <summary>
    /// Reduces player health when damage is taken and checks for death.
    /// </summary>
    void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateUI();

        if (currentHealth <= 0 && !isDead)
        {
            PlayerDied();
        }
    }

    /// <summary>
    /// Checks whether the player has enough points to clear the level.
    /// </summary>
    void CheckGoalArea()
    {
        CancelInvoke("ClearMessage");

        if (currentScore >= targetScore)
        {
            levelCleared = true;

            if (bgmSource != null)
            {
                bgmSource.Stop();
            }

            if (messageText != null)
            {
                messageText.text = "LEVEL CLEARED!\nFinal Score: " + currentScore;
            }

            Invoke("ResetGame", 6f);
        }
        else
        {
            if (messageText != null)
            {
                messageText.text = "Not enough points!\nCollect more coins and crystals.";
            }

            Invoke("ClearMessage", 6f);
        }
    }

    /// <summary>
    /// Handles player death and initiates level reset.
    /// </summary>
    void PlayerDied()
    {
        isDead = true;
        CancelInvoke("ClearMessage");

        if (bgmSource != null)
        {
            bgmSource.Stop();
        }

        if (messageText != null)
        {
            messageText.text = "You died from lava damage!\nRestarting level...";
        }

        Invoke("ResetGame", 6f);
    }

    /// <summary>
    /// Resets the player, collectibles, score, health, and UI to their starting state.
    /// </summary>
    void ResetGame()
    {
        CharacterController controller = GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;
            transform.position = startPosition;
            controller.enabled = true;
        }
        else
        {
            transform.position = startPosition;
        }

        currentScore = 0;
        collectedCount = 0;
        currentHealth = maxHealth;

        currentCollectable = null;
        isDead = false;
        levelCleared = false;

        foreach (Collectible item in allCollectibles)
        {
            item.gameObject.SetActive(true);
        }

        if (bgmSource != null)
        {
            bgmSource.Play();
        }

        ClearMessage();
        UpdateUI();
    }

    /// <summary>
    /// Clears any message currently displayed on screen.
    /// </summary>
    void ClearMessage()
    {
        if (messageText != null)
        {
            messageText.text = "";
        }
    }

    /// <summary>
    /// Updates all gameplay UI elements including score, health, collectibles, and points remaining.
    /// </summary>
    void UpdateUI()
    {
        int pointsLeft = targetScore - currentScore;

        if (pointsLeft < 0)
        {
            pointsLeft = 0;
        }

        int collectiblesLeft = allCollectibles.Length - collectedCount;

        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore + " / " + targetScore;
        }

        if (pointsLeftText != null)
        {
            pointsLeftText.text = "Points Left: " + pointsLeft;
        }

        if (collectiblesLeftText != null)
        {
            collectiblesLeftText.text = "Collectibles Left: " + collectiblesLeft;
        }

        if (healthText != null)
        {
            healthText.text = "Health: " + Mathf.CeilToInt(currentHealth) + " / " + maxHealth;
        }
    }
}
