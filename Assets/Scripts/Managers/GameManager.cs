using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("spawnComonents")]
    GameObject player;
    public static GameManager gameManagerSingleton;
    [SerializeField] Transform playerSpawnPoint;
    [SerializeField] GameObject defaultPlayer;
    [SerializeField] Transform[] enemySpawns;
    [SerializeField] GameObject[] enemyTypes;
    [SerializeField] List<Transform> chestSpawns;
    [SerializeField] int minChests,maxChests;
    [SerializeField] GameObject chest;
    [SerializeField] GameObject flag;
    [SerializeField] Transform[] flagLocs;

    [Header("Stats")]
    public float spawnDelay;
    [SerializeField] float minSpawnDistance,maxSpawnDistance;

    void Awake()
    {

        if(CharacterSelector.selectedPlayer!=null)
        {
            player = Instantiate(CharacterSelector.selectedPlayer, playerSpawnPoint.transform.position,Quaternion.identity);
        }
        else
        {
            player = Instantiate(defaultPlayer, playerSpawnPoint.transform.position,Quaternion.identity);
        }
        FindObjectOfType<FollowPlayer>().StartFollowPlayer(player.transform);


        StartCoroutine(spawnLoop());
        SpawnChests();
        SpawnFlag();

        
    }

    void SpawnEnemy()
    {




        int chance = Random.Range(0,101);

        List<Transform> inRangeSpawns = getAvailableSpawnPoints();
        if(inRangeSpawns == null)
        {
            return;
        }

        foreach(GameObject enemy in enemyTypes)
        {
            if(chance <= enemy.GetComponent<Enemy>().spawnChance)
            {
                

                int spawnIndex = Random.Range(0,inRangeSpawns.Count);
                GameObject en = Instantiate(enemy,inRangeSpawns[spawnIndex].position,Quaternion.identity);
                en.GetComponent<Enemy>().OnSpawn();
                break;
            }
            else
            {
                
            }
        }
    }

    List<Transform> getAvailableSpawnPoints()
    {
        List<Transform> inRangeSpawns = new List<Transform>();
        foreach(Transform spawnPoint in enemySpawns)
        {
            float distance = Mathf.Abs(spawnPoint.position.x - player.transform.position.x); 
            if(distance > minSpawnDistance && distance < maxSpawnDistance)
            {
                
                inRangeSpawns.Add(spawnPoint);
            }
        }
        if(inRangeSpawns.Count == 0)
        {
           
            return null;
        }
        return inRangeSpawns;

    }

    IEnumerator spawnLoop()
    {
        float time = Random.Range(spawnDelay,spawnDelay * 2);
        yield return new WaitForSeconds(time);
        SpawnEnemy();
        StartCoroutine(spawnLoop());

    }

    void SpawnChests()
    {
        int chestsToSpawn = Random.Range(minChests,maxChests+1);

        for(int i=0; i<chestsToSpawn; i++)
        {   
            int chestLocation = Random.Range(0,chestSpawns.Count);
            Instantiate(chest,chestSpawns[chestLocation].position,Quaternion.identity);
            chestSpawns.RemoveAt(chestLocation);
        }

    }

    void SpawnFlag()
    {
        int flagLocation = Random.Range(0,flagLocs.Length);
        Instantiate(flag,flagLocs[flagLocation].position,flagLocs[flagLocation].rotation);
    }


    public void loadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
