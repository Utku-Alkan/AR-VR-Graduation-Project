using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] Transform cameraPosition;
    [SerializeField] Text healthText;

    private float timer = 0f;
    private float interval = 2f; // Each 2 seconds enemy born
    private float moveSpeed = 0.3f; // Speed at which the enemy moves
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

            Vector3 instantiatePosition = new Vector3(transform.position.x + specifiedRandomNumber, transform.position.y + Random.Range(-1, 5), transform.position.z + specifiedRandomNumber);

            GameObject aliveEnemy = Instantiate(enemy, instantiatePosition, Quaternion.identity);
            enemyList.Add(aliveEnemy);
        }

        foreach (GameObject enemyGameObject in enemyList)
        {
            float step = moveSpeed * Time.deltaTime;
            enemyGameObject.transform.position = Vector3.Lerp(enemyGameObject.transform.position, cameraPosition.position, step);
        }

        if(enemyList.Count > 50)
        {
            foreach (GameObject enemyGameObject in enemyList)
            {
                Destroy(enemyGameObject);
            }
            enemyList.Clear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enemy hit player");
        healthText.text = (int.Parse(healthText.text) - 1).ToString();
        enemyList.Remove(other.gameObject);
        Destroy(other.gameObject);
    }
}
