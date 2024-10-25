using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The redHome class is a specialized extension of the ludoHome class, focusing on the unique aspects of the red player's home base in a game resembling Ludo.
// It serves to implement red-specific logic, which may include handling the spawning of player pieces, managing pieces returning to home after being captured,
// and any visual or interactive elements unique to the red player's home. The class utilizes Unity's lifecycle methods, Start and Update, as placeholders
// for initializing and continuously updating the state of the red home throughout the game's duration.
public class redHome : ludoHome
{
    // Start is called before the first frame update
    void Start()
    {
        // Here, initialization specific to the red home could be implemented, setting up the home base at the beginning of the game.
    }

    // Update is called once per frame
    void Update()
    {
        // This method provides an opportunity to add any updates that need to happen every frame, potentially reacting to game state changes or player actions.
    }
}
