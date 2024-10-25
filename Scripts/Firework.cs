using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component

    void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator component attached to the same GameObject
        if (animator == null)
        {
            Debug.LogError("Animator component not found!", this);
        }
    }

    public void PlayFireworks()
    {
        gameObject.SetActive(true); // Ensure the GameObject is active
        animator.Play("firework", -1, 0f); // Play the firework animation from the start
    }

    // Call this method at the end of your animation or when you want to stop the fireworks
    public void StopFireworks()
    {
        gameObject.SetActive(false); // Optionally deactivate the GameObject
    }
}

