using UnityEngine;

public class GameController : MonoBehaviour
{
    GameObject currentCollectable;

    public int collectedCount = 0;
    public int currentScore = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collectible")
        {
            currentCollectable = other.gameObject;
        }

        if (other.gameObject.tag == "GoalArea" && collectedCount >= 7)
        {
            print("Player entered goal area with " + collectedCount + " collectibles");
            print("Final score: " + currentScore);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentCollectable)
        {
            currentCollectable = null;
        }
    }

    void OnInteract()
    {
        if (currentCollectable != null)
        {
            Collectible collectibleScript = currentCollectable.GetComponent<Collectible>();

            collectedCount++;
            currentScore += collectibleScript.score;

            print("Player has collected " + collectedCount + " collectibles");
            print("Current score: " + currentScore);

            Destroy(currentCollectable);
            currentCollectable = null;
        }
    }
}