using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemySpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] Transform cameraPosition;
    [SerializeField] Text healthText;

    [SerializeField] List<GameObject> collectableAllies;
    [SerializeField] int collectableCount;

    private float timer = 0f;
    [SerializeField] float enemyBornInterval;
    [SerializeField] float moveSpeed;
    [SerializeField] int maxEnemyCount;

    private List<GameObject> enemyList = new List<GameObject>();

    private List<GameObject> collectableAllyList = new List<GameObject>();

    // logics
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Text youWinText;


    void Start()
    {
        // collectable born
        for(int i = 0; i < collectableCount; i++)
        {
            int allyRandom1 = Random.Range(-4, 4);
            int allyRandom2 = Random.Range(-4, 4);
            Vector3 instantiatePositionCollectable = new Vector3(transform.position.x + allyRandom1, transform.position.y, transform.position.z + allyRandom2);
            GameObject aliveCollectable = Instantiate(collectableAllies[Random.Range(0, collectableAllies.Count)], instantiatePositionCollectable, Quaternion.identity);
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

            if (enemyList.Count < maxEnemyCount)
            {
                // enemy born
                Debug.Log("Enemy Born");
                timer = 0f;

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
                enemyList.Add(aliveEnemy);
            }
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
            Debug.Log("Enemy hit player");
            healthText.text = (int.Parse(healthText.text) - 1).ToString();
            enemyList.Remove(other.gameObject);
            Destroy(other.gameObject);

            if(int.Parse(healthText.text) < 1) // you lost
            {
                gameOverPanel.SetActive(true);
                youWinText.text = "You Lost :(";
                StartCoroutine(ChangeSceneAfterDelay(5f));
            }
        }else if (other.CompareTag("CollectableAlly"))
        {
            Debug.Log("Player collected collectable");
            healthText.text = (int.Parse(healthText.text) + 1).ToString();
            collectableAllyList.Remove(other.gameObject);
            Destroy(other.gameObject);

            if(collectableAllyList.Count < 1) // you win
            {
                gameOverPanel.SetActive(true);
                youWinText.text = "You Win!";
                StartCoroutine(ChangeSceneAfterDelay(5f));
            }
        }
    }

    private IEnumerator ChangeSceneAfterDelay(float afterSeconds)
    {
        yield return new WaitForSeconds(afterSeconds); 
        SceneManager.LoadScene("MainScene");
    }
}
