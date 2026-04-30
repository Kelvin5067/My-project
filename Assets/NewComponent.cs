using UnityEngine;

public class NewComponent : MonoBehaviour
{
    // Movement speed
    public float moveSpeed = 2f;

    // Rotation speed
    public float rotateSpeed = 50f;

    // Limit before reversing direction
    public float limit = 5f;

    // Direction control
    private int moveDirection = 1;
    private int rotateDirection = 1;

    void Start()
    {
        print("Starting Position: " + transform.position);
    }

    void Update()
    {
        
        transform.position += new Vector3(moveSpeed * moveDirection * Time.deltaTime, 0f, 0f);
        if (transform.position.x >= limit)
        {
            moveDirection = -1;
        }
        else if (transform.position.x <= -limit)
        {
            moveDirection = 1;
        }

        transform.Rotate(0f, rotateSpeed * rotateDirection * Time.deltaTime, 0f);

        float currentY = transform.eulerAngles.y;

        if (currentY >= 180f)
        {
            rotateDirection = -1;
        }
        else if (currentY <= 10f)
        {
            rotateDirection = 1;
        }
    }
}