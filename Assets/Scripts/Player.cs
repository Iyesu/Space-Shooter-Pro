using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public variables are visible in the Unity Editor
    //public variables are best used for values that you want to change in the Unity Editor
    //to change the visibility of a private variable, use SerializeField to make it visible in the Unity Editor
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShopPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private float _speed = 5.0f;
    private int _speedMultiplier = 2;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;

    private float xRightLimit = 9f;
    private float xLeftLimit = -9f;
    private float yTopLimit = 6.5f;
    private float yBottomLimit = -1.0f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    private bool _tripleShotActive = false;
    private bool _speedPowerUpActive = false;
    private bool _shieldPowerUpActive = false;

    [SerializeField]
    private int _score = 0;

    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        // Assign start position = new position(0, 0, 0)
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        //Debug code
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if ( _uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

    }

    /**
     * This method controls the player movement.
     */
    private void CalculateMovement()
    {
        // 1 unit = 1 meter, 1 meter per frame, 60 frames per second
        //transform.Translate(Vector3.right * speed * Time.deltaTime);
        //get user input from horizontal axis (A/D keys or Left/Right arrow keys)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        float multiplier = _speedPowerUpActive ? _speedMultiplier : 1;

        //works separately, but we can combine them into one line of code
        transform.Translate(direction * (_speed * multiplier) * Time.deltaTime);
        //cleaner code that does the same thing as the code below, but we can use Mathf.Clamp to limit the player's movement within the bounds of the screen
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, yBottomLimit, yTopLimit), 0);

        //can use switch statement but cannot use in current version of C# (7.3) because it does not support pattern matching
        //old code to understand the concept of player bounds
        //if (transform.position.y >= yTopLimit)
        //{
        //    transform.position = new Vector3(transform.position.x, yTopLimit, 0);
        //}
        //else if (transform.position.y <= yBottomLimit)
        //{
        //    transform.position = new Vector3(transform.position.x, yBottomLimit, 0);
        //}

        if (transform.position.x >= xRightLimit)
        {
            transform.position = new Vector3(xLeftLimit, transform.position.y, 0);
        }
        else if (transform.position.x <= xLeftLimit)
        {
            transform.position = new Vector3(xRightLimit, transform.position.y, 0);
        }
    }

    private void FireLaser()
    {
        //Time.time is the time since the game started, so we can use it to control the fire rate of the laser
        //_canFire starts at -1f so the player can immediately fire when the game starts. We change the value of 
        //_canFire to the current time that the player shot the current fire plus the fire rate, so by the time
        //the check happens again, Time.time MUST be greater than the value set by _canFire, effectively creating
        //a cooldown for the laser fire rate
        _canFire = Time.time + _fireRate;

        GameObject laserType = _laserPrefab;
        Vector3 shotPosition = transform.position + new Vector3(0, 0.9f, 0);

        if (_tripleShotActive)
        {
            laserType = _tripleShopPrefab;
            shotPosition = transform.position;
        }

        //Quarternion.identity means no rotation, the laser will be instantiated with the same rotation as the player
        Instantiate(laserType, shotPosition, Quaternion.identity);
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotActive = false;
    }

    private IEnumerator SpeedPowerUpPickup()
    {
        yield return new WaitForSeconds(7.0f);
        _speedPowerUpActive = false;
    }

    public void Damage()
    {
        if (_shieldPowerUpActive)
        {
            _shieldPowerUpActive = false;
            _shieldVisualizer.SetActive(false);

            return;
        }

        _lives--;

        _uiManager.UpdateLives(_lives);

        if (_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
    public void OnTripleShotPickup()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void OnSpeedPickup()
    {
        _speedPowerUpActive = true;
        StartCoroutine(SpeedPowerUpPickup());
    }

    public void OnShieldPickup()
    {
        _shieldPowerUpActive = true;

        _shieldVisualizer.SetActive(true);
    }

    public void OnEnemyKill(int points)
    {
        _score += points;

        _uiManager.UpdateScore(_score);
    }
}
