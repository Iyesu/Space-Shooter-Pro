using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _yEnemySpawn = 8.3f;
    private float _xEnemySpawnMin = -7.0f;
    private float _xEnemySpawnMax = 7.0f;

    private Player _player;
    private Animator _animator;

    private bool _isEnemyDead = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = setPosition();

        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

        if (_animator == null)
        {
            Debug.LogError("Animator is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -2.7f && !_isEnemyDead)
        {
            transform.position = setPosition();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //debut log
        //Debug.Log("Hit: " + other.transform.name);

        if (other.tag == "Player")
        {
            //Damage the player code
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            DestroyEnemy();
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            //Update score

            if (_player != null)
            {
                _player.OnEnemyKill(Random.Range(8, 13));
            }

            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        //Play explosion animation
        _animator.SetTrigger("OnEnemyDeath");
        //Disable the collider so the animation can play without interruption
        GetComponent<Collider2D>().enabled = false;
        _isEnemyDead = true;

        Destroy(this.gameObject, 2.8f);
    }

    private Vector3 setPosition()
    {
        return new Vector3(Random.Range(_xEnemySpawnMin, _xEnemySpawnMax), _yEnemySpawn, 0);
    }
}
