using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //The audio for the explosion of both the Asteroid and the Player is the same. The audio source is played immediately when
        //the explosion GameObject is instantiated as long as we tick the Play On Awake option in the Audio Source component of the explosion prefab.
        Destroy(this.gameObject, 3.0f);
    }
}
