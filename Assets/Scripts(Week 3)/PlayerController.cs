using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[] collectibles;

    public int collectedCount = 0;

    void OnCollisionEnter(Collision collision)
    {
        print("Collision detected with " + collision.gameObject.name);

        for(int i = 0; i < collectibles.Length; i++)
        {
            if(collision.gameObject == collectibles[i])
            {
                Destroy(collision.gameObject);

                collectedCount++;

                print("Collected: " + collectedCount);

                if(collectedCount == collectibles.Length)
                {
                    print("All collectibles collected!");
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        print("Trigger entered by " + other.gameObject.name);

        if(other.gameObject.name == "TriggerZone")
        {
            if(collectedCount == collectibles.Length)
            {
                print("Player entered after collecting ALL collectibles!");
            }
            else
            {
                print("Please collect all collectibles first!");
            }
        }
    }
}