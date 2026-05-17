using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powerUpId;

    //private AudioSource _audioSource;
    //private GameObject _audioManager;

    [SerializeField]
    private AudioClip _powerUpAudioClip;

    void Start()
    {
        //Refer to Enemy.cs for this type of audio management for the PowerUp audio source.
        //_audioManager = GameObject.Find("Audio Manager");

        //if (_audioManager == null)
        //{
        //    Debug.LogError("The Audio Manager is NULL.");
        //}
        //else
        //{
        //    Transform explosionSource = _audioManager.transform.Find("PowerUp");

        //    if (explosionSource == null)
        //    {
        //        Debug.LogError("PowerUp audio source is NULL.");
        //    }

        //    _audioSource = explosionSource.GetComponent<AudioSource>();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -2.7)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            //Another way to play an audio clip so that it will play even if the PowerUp is destroyed immediately after pickup.
            //This is a static method that creates a temporary audio source to play the clip at the position of the PowerUp.
            AudioSource.PlayClipAtPoint(_powerUpAudioClip, transform.position);

            switch (_powerUpId) {
                case 0:
                    player.OnTripleShotPickup();
                    break;
                case 1:
                    player.OnSpeedPickup();
                    break;
                default:
                    player.OnShieldPickup();
                    break;
            }

            Destroy(this.gameObject);
            //_audioSource.Play();
        }
    }
}
