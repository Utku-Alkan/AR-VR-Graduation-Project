using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject enemy;
    [SerializeField] Transform cameraPosition;

    private float timer = 0f;
    private float interval = 2f; // Each 5 seconds enemy born
    private float moveSpeed = 0.5f; // Speed at which the enemy moves
    private List<GameObject> enemyList = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        // Increment the timer by the time passed since the last frame
        timer += Time.deltaTime;

        // Check if the timer has reached the interval (5 seconds)
        if (timer >= interval)
        {
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

            Vector3 instantiatePosition = new Vector3(specifiedRandomNumber, Random.Range(-1, 5), specifiedRandomNumber);

            GameObject aliveEnemy = Instantiate(enemy, instantiatePosition, Quaternion.identity);
            enemyList.Add(aliveEnemy);
        }

        foreach (GameObject enemyGameObject in enemyList)
        {
            float step = moveSpeed * Time.deltaTime;
            enemyGameObject.transform.position = Vector3.Lerp(enemyGameObject.transform.position, cameraPosition.position, step);
        }

        if(enemyList.Count > 10)
        {
            foreach (GameObject enemyGameObject in enemyList)
            {
                Destroy(enemyGameObject);
            }
            enemyList.Clear();
        }
    }
}
