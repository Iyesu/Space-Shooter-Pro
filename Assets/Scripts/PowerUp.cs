using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powerUpId;

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
        }
    }
}
