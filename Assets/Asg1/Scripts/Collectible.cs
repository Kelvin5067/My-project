/*
 * Author: Kelvin Teo
 * Date: 14 June 2026
 * Description: Handles collectible items such as coins and crystals.
 */

using UnityEngine;

public class Collectible : MonoBehaviour
{
    /// <summary>
    /// Score value given when this collectible is collected.
    /// Coin = 10, Crystal = 30.
    /// </summary>
    public int score = 10;

    /// <summary>
    /// Audio source used to play collection sound.
    /// </summary>
    AudioSource collectibleAudio;

    void Start()
    {
        collectibleAudio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Collects the item, plays sound, and hides the item.
    /// </summary>
    public void Collect()
    {
        if (collectibleAudio != null && collectibleAudio.clip != null)
        {
            AudioSource.PlayClipAtPoint(collectibleAudio.clip, transform.position);
        }

        gameObject.SetActive(false);
    }
}