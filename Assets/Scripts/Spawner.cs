using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject[] targets;
    public Transform[] points;
    public float beat; // = (60/109)*2;
    private float timer;
    public GameObject player;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        if (timer>beat)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 playerDirection = player.transform.forward;
            Quaternion playerRotation = player.transform.rotation;
            float spawnDistance = 0.8f;

            Vector3 spawnOffset =Quaternion.Euler(0, Random.Range(-30, 30), 0) * playerDirection * spawnDistance; 
            Vector3 spawnPos = playerPos + spawnOffset;
            Debug.Log(spawnPos);
            

            int targetIdx = Random.Range(0, 4);
            if (targetIdx == 2 || targetIdx == 3) { // Foot Target is Spawned
                float yRange = Random.Range(-0.8f, -0.55f);
                spawnPos.y = playerPos.y + yRange;
            } else { // Hand Target is spawned
                float yRange = Random.Range(-0.4f, 0.1f);
                spawnPos.y = playerPos.y + yRange;
            }
            GameObject target = Instantiate(targets[targetIdx], spawnPos, playerRotation);
            timer -= beat;
        }
        timer += Time.deltaTime;
    }
}
