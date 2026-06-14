using UnityEngine;

public class NewComponent : MonoBehaviour
//for collectibles to rotate in place
{
    public float rotateSpeed = 50f;

    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }
}