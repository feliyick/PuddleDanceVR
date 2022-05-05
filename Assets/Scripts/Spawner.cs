using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{

    public GameObject[] targets;
    public Transform[] points;
    public AudioClip[] audioClips;
    public float beat; // = (60/109)*2;
    private float timer;
    public GameObject player;
    private float timerProgress;
    private Slider ProgressBar;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;
    Dictionary<string, int> songIdxDict = new Dictionary<string, int>();
    public Dictionary<string, Dictionary<int, float>> songBPMDict = new Dictionary<string, Dictionary<int, float>>();
    // Start is called before the first frame update
    void Start()
    {
        ProgressBar = GameObject.Find("ProgressBar").GetComponent<Slider>();
        Debug.Log("SLIDER START VALUE: " + ProgressBar.value);
        initSongIdxDict();
        initSongBPMDict();
    }

    void initSongIdxDict() {
        songIdxDict.Add("EyeOfTheTiger", 0);
        songIdxDict.Add("IWillSurvive", 1);
        songIdxDict.Add("WhatDoesntKillYou", 2);
    }

    void initSongBPMDict() {
        Dictionary<int, float> eyeOfTheTiger = new Dictionary<int, float>();
        eyeOfTheTiger.Add(0, (60/109)*4);
        eyeOfTheTiger.Add(1, (60/109)*2);
        songBPMDict.Add("EyeOfTheTiger", eyeOfTheTiger);

        Dictionary<int, float> iWillSurvive = new Dictionary<int, float>();
        iWillSurvive.Add(0, (60/116)*4);
        iWillSurvive.Add(1, (60/116)*2);
        songBPMDict.Add("IWillSurvive", iWillSurvive);


        Dictionary<int, float> stronger = new Dictionary<int, float>();
        stronger.Add(0, (60/116)*4);
        stronger.Add(1, (60/116)*2);
        songBPMDict.Add("WhatDoesntKillYou", stronger);

    }

    void SetSong(string songChoice, int diffLevel) {
        int idx = songIdxDict[songChoice];
        musicSource.clip = audioClips[idx];
        beat = songBPMDict["songChoice"][diffLevel];
    }   

    void StopSong() {
        musicSource.Stop();
    }

    void ResetSpawner() {
        timer = 0;
        timerProgress = 0;
        ProgressBar.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ProgressBar.value = timerProgress / musicSource.clip.length;

        if (timer>beat+0.1)
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
        timerProgress += Time.deltaTime;

    }
}


/***
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Spawner : MonoBehaviour
{

    public GameObject[] targets;
    public Transform[] points;
    public float beat; // = (60/109)*2;
    private float timer;
    public GameObject player;
    private float timerProgress;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;
    public Slider ProgressBar;
    // Start is called before the first frame update
    void Start()
    {
       ProgressBar = GameObject.Find("ProgressBar").GetComponent<Slider>();
       Debug.Log("Progress Bar slider value: " + ProgressBar.value);
       Debug.Log("SONG LENGTH: " + musicSource.clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        ProgressBar.value = timerProgress / musicSource.clip.length;
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
        timerProgress += Time.deltaTime;
    }
}
***/
