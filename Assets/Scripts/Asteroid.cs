using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * 20 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            StartCoroutine(hideAsteroid());
        }
    }

    private IEnumerator hideAsteroid()
    {
        yield return new WaitForSeconds(0.25f);

        _spawnManager.StartSpawning();

        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Destroy(this.gameObject);
    }
}
