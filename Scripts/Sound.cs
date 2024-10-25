using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Sound class manages toggling the sound state within the game. It responds to mouse clicks to switch between playing and muting game sounds.
public class Sound : MonoBehaviour
{
    // Reference to the GameObject that represents the sound play state in the UI.
    public GameObject play;
    // Reference to the GameObject that represents the sound mute state in the UI.
    public GameObject mute;

    // This method is called when the GameObject this script is attached to is clicked.
    public void OnMouseDown()
    {
        // Check the current sound state to toggle between play and mute.
        if (gameManager.gm.sound)
        {
            // If sound is currently enabled, disable it, show the mute icon, and hide the play icon.
            gameManager.gm.sound = false; // Update the gameManager's sound state to reflect this change.
            play.SetActive(false); // Hide the play icon since we're muting the sound.
            mute.SetActive(true); // Show the mute icon to indicate the sound is off.
        }
        else
        {
            // If sound is currently disabled, enable it, show the play icon, and hide the mute icon.
            gameManager.gm.sound = true; // Update the gameManager's sound state to enabled.
            play.SetActive(true); // Show the play icon to indicate sound is on.
            mute.SetActive(false); // Hide the mute icon since sound is no longer muted.
        }
    }
}
