using UnityEngine;

public class DoorController : MonoBehaviour
{
    Animator myAnimator;

    bool isOpen = false;
//for doors to open and close
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void Interact()
    {
        if(myAnimator != null)
        {
            if(isOpen)
                myAnimator.SetTrigger("CloseDoor");
            else
                myAnimator.SetTrigger("OpenDoor");

            isOpen = !isOpen;
        }
    }
}