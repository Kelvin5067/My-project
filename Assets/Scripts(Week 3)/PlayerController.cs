using UnityEngine;

public class GameController : MonoBehaviour
{
    GameObject currentCollectable;
    DoorController currentDoor;

    public int collectedCount = 0;
    public int currentScore = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collectible")
        {
            currentCollectable = other.gameObject;
        }

        if (other.gameObject.tag == "Door")
        {
            currentDoor = other.GetComponent<DoorController>();
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

        if (other.GetComponent<DoorController>() == currentDoor)
        {
            currentDoor = null;
        }
    }

    void OnInteract()
    {
        if (currentCollectable != null)
        {
            Collectible collectibleScript = currentCollectable.GetComponent<Collectible>();

            collectedCount++;
            currentScore += collectibleScript.score;
            collectibleScript.Collect();

            print("Player has collected " + collectedCount + " collectibles");
            print("Current score: " + currentScore);

            currentCollectable = null;
        }

        if (currentDoor != null)
        {
            currentDoor.Interact();
        }
    }
}