using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject[] targets;
    public Transform[] points;
    public float beat = (60/109)*2;
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
        //GameObject spawner = GameObject.Find("Spawner");
        // spawnTrans = spawner.GetComponent<Transform>();
       // Debug.Log(spawnTrans.position);
       // spawner.transform.localPosition = playerHead.GetComponent<Transform>().position;




        if (timer>beat)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 playerDirection = player.transform.forward;
            Quaternion playerRotation = player.transform.rotation;
            float spawnDistance = 1;

            Vector3 spawnOffset =/* Quaternion.Euler(Random.Range(-5, 5), Random.Range(-10, 10), Random.Range(-5, 5)) **/ playerDirection * spawnDistance; 
            Vector3 spawnPos = playerPos + spawnOffset;
            Debug.Log(spawnPos);
            GameObject target = Instantiate(targets[Random.Range(0, 4)], spawnPos, playerRotation);
            //GameObject target = Instantiate(targets[Random.Range(0, 4)], points[Random.Range(0, 4)]);
            timer -= beat;
        }
        timer += Time.deltaTime;
    }
}
