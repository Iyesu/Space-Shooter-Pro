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

    // Start is called before the first frame update
    void Start()
    {
        transform.position = setPosition();

        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -2.7f)
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

            Destroy(this.gameObject);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            //Update score

            if (_player != null)
            {
                _player.OnEnemyKill(Random.Range(8, 13));
            }

            Destroy(this.gameObject);
        }
    }

    private Vector3 setPosition()
    {
        return new Vector3(Random.Range(_xEnemySpawnMin, _xEnemySpawnMax), _yEnemySpawn, 0);
    }
}
