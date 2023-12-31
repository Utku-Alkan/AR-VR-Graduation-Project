using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class EnemySpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] Transform cameraPosition;
    [SerializeField] Text healthText;

    [SerializeField] List<GameObject> collectableAllies;
    [SerializeField] int collectableCount;
    private CollectableProperties CollectableProperties;

    private float timer = 0f;
    [SerializeField] float enemyBornInterval;
    [SerializeField] float moveSpeed;
    [SerializeField] int maxEnemyCount;

    private List<GameObject> enemyList = new List<GameObject>();

    private List<GameObject> collectableAllyList = new List<GameObject>();
    [SerializeField] float objectLifetime;

    // logics for UI
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Text youWinText;
    [SerializeField] Text HighscoreText;
    [SerializeField] Text ScoreText;

    [SerializeField] float locationThreshold = 0.0005f;
    [SerializeField] GameObject heartObject;

    // location based --> cafeteria
    [SerializeField] float cafeteriaLatitude = 40.8913894f;
    [SerializeField] float cafeteriaLongitude = 29.3798876f;
    [SerializeField] List<GameObject> cafeteriaCollectables;

    // location based --> IC
    [SerializeField] float ICLatitude = 40.9276085f; 
    [SerializeField] float ICLongitude = 29.3205172f;
    [SerializeField] List<GameObject> ICCollectables;

    // location based --> grass
    [SerializeField] float grassLatitude = 40.8913442f;
    [SerializeField] float grassLongitude = 29.3791577f;
    [SerializeField] List<GameObject> grassCollectables;

    // location based --> FENS
    [SerializeField] float FENSLatitude = 40.8906637f;
    [SerializeField] float FENSLongitude = 29.3797448f;
    [SerializeField] List<GameObject> FENSCollectables;

    // location based --> FMAN
    [SerializeField] float FMANLatitude = 40.8920220f;
    [SerializeField] float FMANLongitude = 29.3790876f;
    [SerializeField] List<GameObject> FMANCollectables;

    [SerializeField] Text LocationText;

    private float Distance;
    

    private bool isGameFinished = false;

    void Start()
    {
        #if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        #endif
        

        if (!Input.location.isEnabledByUser) return;

        Input.location.Start();

        // collectable born
        for(int i = 0; i < collectableCount; i++)
        {
            int allyRandom1 = Random.Range(-4, 4);
            int allyRandom2 = Random.Range(-4, 4);
            Vector3 instantiatePositionCollectable = new Vector3(transform.position.x + allyRandom1, transform.position.y, transform.position.z + allyRandom2);
            GameObject aliveCollectable = Instantiate(collectableAllies[Random.Range(0, collectableAllies.Count)], instantiatePositionCollectable, Quaternion.identity);
            aliveCollectable.AddComponent<SpawnTimestamp>();
            collectableAllyList.Add(aliveCollectable);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Increment the timer by the time passed since the last frame
        timer += Time.deltaTime;

        // Check if the timer has reached the interval
        if (timer >= enemyBornInterval)
        {
            timer = 0f;


            // collectable born
            if (Input.location.status == LocationServiceStatus.Running)
            {
                LocationInfo currentLocation = Input.location.lastData;
                // cafeteria
                if (IsCloseToLocation(currentLocation.latitude, currentLocation.longitude, cafeteriaLatitude, cafeteriaLongitude))
                {
                    LocationText.text = "Cafeteria: " + Distance.ToString();
                    SpawnCollectables(cafeteriaCollectables);
                }
                // FENS
                else if (IsCloseToLocation(currentLocation.latitude, currentLocation.longitude, FENSLatitude, FENSLongitude))
                {
                    LocationText.text = "FENS: " + Distance.ToString();
                    SpawnCollectables(FENSCollectables);
                }
                
                //grass
                else if (IsCloseToLocation(currentLocation.latitude, currentLocation.longitude, grassLatitude, grassLongitude))
                {
                    LocationText.text = "Grass: " + Distance.ToString();
                    SpawnCollectables(grassCollectables);
                }
                // FMAN
                else if (IsCloseToLocation(currentLocation.latitude, currentLocation.longitude, FMANLatitude, FMANLongitude))
                {
                    LocationText.text = "FMAN: " + Distance.ToString();
                    SpawnCollectables(FMANCollectables);
                }
                // IC
                else if (IsCloseToLocation(currentLocation.latitude, currentLocation.longitude, ICLatitude, ICLongitude))
                {
                    LocationText.text = "Library: " + Distance.ToString();
                    SpawnCollectables(ICCollectables);
                }
                else
                {
                    LocationText.text = "Unknown Location";
                    SpawnCollectables(collectableAllies);
                    //healthText.text = "90";
                }
            }
            else{
                LocationText.text = "Location not verified";
                SpawnCollectables(collectableAllies);
            }


            // enemy born
            if (enemyList.Count < maxEnemyCount)
            {
                // enemy born
                Debug.Log("Enemy Born");

                // randoming the instantiate
                int randomNumber = Random.Range(10, 21);
                int randomNumber2 = Random.Range(-20, -9);
                int selectedRange = Random.Range(0, 2);

                int specifiedRandomNumber = randomNumber2;
                if(selectedRange == 0)
                {
                    specifiedRandomNumber = randomNumber;
                }

                Vector3 instantiatePosition = new Vector3(transform.position.x + specifiedRandomNumber, transform.position.y + Random.Range(-1, 5), transform.position.z + specifiedRandomNumber);

                GameObject aliveEnemy = Instantiate(enemy, instantiatePosition, Quaternion.identity);
                aliveEnemy.AddComponent<SpawnTimestamp>();
                enemyList.Add(aliveEnemy);
            }

            // check if lifetime of objects finished
            CheckAndDestroyObjects(enemyList);
            CheckAndDestroyObjects(collectableAllyList);
        }

        foreach (GameObject enemyGameObject in enemyList)
        {
            float step = moveSpeed * Time.deltaTime;
            enemyGameObject.transform.position = Vector3.Lerp(enemyGameObject.transform.position, cameraPosition.position, step);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            if (!isGameFinished)
            {
                Debug.Log("Enemy hit player");
                healthText.text = (int.Parse(healthText.text) - 1).ToString();
                enemyList.Remove(other.gameObject);
                Destroy(other.gameObject);

                if (int.Parse(healthText.text) < 1) // you lost
                {
                    isGameFinished = true;
                    gameOverPanel.SetActive(true);
                    youWinText.text = "Your Score: " + ScoreText.text;

                    int highScore = PlayerPrefs.GetInt("highscore", 0);

                    if (highScore < int.Parse(ScoreText.text))
                    {
                        HighscoreText.text = "NEW HIGHSCORE!!!";
                        PlayerPrefs.SetInt("highscore", int.Parse(ScoreText.text));
                    }
                    else
                    {
                        HighscoreText.text = "Your Highscore: " + highScore.ToString();
                    }


                    StartCoroutine(ChangeSceneAfterDelay(10f));
                }
            }
        }
        else if (other.CompareTag("CollectableAlly") || other.CompareTag("Heart"))
        {
            if (!isGameFinished) { 
                Debug.Log("Player collected collectable");
                //CollectableProperties collectableProps = other.GetComponent<CollectableProperties>();
                
                if (other.CompareTag("Heart")) 
                {
                    healthText.text = (int.Parse(healthText.text) + 5).ToString();
                }
                else
                {
                    ScoreText.text = (int.Parse(ScoreText.text) + 1).ToString();
                }
                
                collectableAllyList.Remove(other.gameObject);
                Destroy(other.gameObject);

                if(collectableAllyList.Count < 1) // you win
                {
                    isGameFinished = true;
                    gameOverPanel.SetActive(true);
                    youWinText.text = "You Collected Everything?!";
                    int highScore = PlayerPrefs.GetInt("highscore", 0);
                    HighscoreText.text = "Your Highscore: " + highScore.ToString();
                    StartCoroutine(ChangeSceneAfterDelay(10f));
                }
            }
        }
    }

    void SpawnCollectables(List<GameObject> collectables)
    {
        for (int i = 0; i < collectableCount; i++)
        {
            int allyRandom1 = Random.Range(-4, 4);
            int allyRandom2 = Random.Range(-4, 4);
            Vector3 instantiatePosition = new Vector3(transform.position.x + allyRandom1, transform.position.y, transform.position.z + allyRandom2);

            GameObject aliveCollectable;

            // 10% chance to spawn the heart object
            if (Random.value < 0.1f)
            {
                aliveCollectable = Instantiate(heartObject, instantiatePosition, Quaternion.identity);
                aliveCollectable.AddComponent<SpawnTimestamp>();
                collectableAllyList.Add(aliveCollectable);

            }
            else
            {
                aliveCollectable = Instantiate(collectables[Random.Range(0, collectables.Count)], instantiatePosition, Quaternion.identity);
                aliveCollectable.AddComponent<SpawnTimestamp>();
                collectableAllyList.Add(aliveCollectable);
            }
        }
    }

    private IEnumerator ChangeSceneAfterDelay(float afterSeconds)
    {
        yield return new WaitForSeconds(afterSeconds); 
        SceneManager.LoadScene("MainScene");
    }

    bool IsCloseToLocation(float currentLatitude, float currentLongitude, float targetLatitude, float targetLongitude)
    {
        float distance = Mathf.Sqrt(
            Mathf.Pow(currentLatitude - targetLatitude, 2) +
            Mathf.Pow(currentLongitude - targetLongitude, 2));

        Distance = distance;

        return distance < locationThreshold;
    }


    void CheckAndDestroyObjects(List<GameObject> objectList)
    {
        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (GameObject obj in objectList)
        {
            // Check if the object's lifetime has exceeded
            if (obj.GetComponent<SpawnTimestamp>().lifeTime >= objectLifetime)
            {
                objectsToRemove.Add(obj);
            }
        }

        // Remove and destroy objects
        foreach (GameObject objToRemove in objectsToRemove)
        {
            objectList.Remove(objToRemove);
            Destroy(objToRemove);
        }
    }
}
