using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is designed to manage the sound effects associated with dice actions within the game,
// extending the basic functionalities provided by MonoBehaviour.
public class Dice : MonoBehaviour
{
    // Private reference to the AudioSource component attached to the same GameObject.
    // This component is used to play sound clips, including the dice roll sound effect.
    AudioSource ads;

    // The Start method is a MonoBehaviour callback that Unity calls before the first frame update.
    // It's used here to initialize component references.
    void Start()
    {
        // Retrieves the AudioSource component attached to this GameObject,
        // ensuring that the dice roll sound effect can be played.
        ads = GetComponent<AudioSource>();
    }

    // This public method is designed to be called by other scripts or GameObjects in the game,
    // specifically when a dice roll occurs and the corresponding sound effect needs to be played.
    public void playSound()
    {
        // Triggers the playback of the sound clip currently assigned to the AudioSource component.
        // This is expected to be the dice roll sound, providing immediate auditory feedback to the player.
        ads.Play();
    }
}
