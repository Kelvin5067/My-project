using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int score = 1;

    AudioSource collectibleAudio;

    void Start()
    {
        collectibleAudio = GetComponent<AudioSource>();
    }

    public void Collect()
    {
        if (collectibleAudio != null)
        {
            AudioSource.PlayClipAtPoint(collectibleAudio.clip, transform.position);
        }

        Destroy(gameObject);
    }
}