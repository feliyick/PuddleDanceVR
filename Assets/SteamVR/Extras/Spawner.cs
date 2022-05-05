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
    public float beat= (60/109)*2;
    private float timer;
    public GameObject player;
    private float timerProgress;
    private Slider ProgressBar;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;
    Dictionary<string, int> songIdxDict = new Dictionary<string, int>();
    public Dictionary<string, Dictionary<int, float>> songBPMDict = new Dictionary<string, Dictionary<int, float>>();
    Dictionary<int, float> eyeOfTheTiger = new Dictionary<int, float>();
    Dictionary<int, float> iWillSurvive = new Dictionary<int, float>();
    Dictionary<int, float> stronger = new Dictionary<int, float>();
    // Start is called before the first frame update
    
    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        initSongIdxDict();
        // initSongBPMDict();
        // player = GameObject.Find("XRRig/CameraOffset/MainCamera");
    }
    
    void Start()
    {
        Debug.Log("SLIDER START VALUE: " + ProgressBar.value);
        // player = GameObject.Find("XRRig/CameraOffset/MainCamera");
    }

    void initSongIdxDict() {
        songIdxDict.Add("EyeOfTheTiger", 0);
        songIdxDict.Add("IWillSurvive", 1);
        songIdxDict.Add("WhatDoesntKillYou", 2);
    }

    void initSongBPMDict() {
        
        eyeOfTheTiger.Add(0, (60/109)*4);
        eyeOfTheTiger.Add(1, (60/109)*2);
        songBPMDict.Add("EyeOfTheTiger", eyeOfTheTiger);

        
        iWillSurvive.Add(0, (60/116)*4);
        iWillSurvive.Add(1, (60/116)*2);
        songBPMDict.Add("IWillSurvive", iWillSurvive);


        
        stronger.Add(0, (60/116)*4);
        stronger.Add(1, (60/116)*2);
        songBPMDict.Add("WhatDoesntKillYou", stronger);

    }

    public void SetSong(string songChoice, int diffLevel) {
        Debug.Log("SONG CHOICE: " + songChoice);
        Debug.Log("SONG CHOICE DIFFICULTY: " + diffLevel);
        int idx = songIdxDict[songChoice];
        Debug.Log("SONG CHOICE IDX: " + idx);
        musicSource.clip = audioClips[idx];
        Debug.Log("SONG CHOICE CLIP NAME: " + audioClips[idx]);
        // Debug.Log("SONG CHOICE BPM: " + songBPMDict[songChoice]);
        // Debug.Log("SONG CHOICE BPM LEVEL: " + songBPMDict[songChoice][diffLevel]);

        if (songChoice == "EyeOfTheTiger") {
            if (diffLevel == 0) {
                beat = (60f/109f)*4f;
            } else {
                beat = (60f/109f)*2f;
            }
        } else {
            Debug.Log("SETTING BEAT");
            if (diffLevel == 0) {
                Debug.Log("SETTING BEAT TO I WILL SURVIVE");
                beat = (60f/116f)*4f;
            } else {
                beat = (60f/116f)*2f;
            }
        }
        // beat = (songBPMDict[songChoice])[diffLevel];
        Debug.Log("BEAT: " + beat);
        musicSource.Play();
    }   

    public void StopSong() {
        musicSource.Stop();
    }

    public void ResetSpawner() {
        timer = 0;
        timerProgress = 0;
        ProgressBar.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ProgressBar = GameObject.Find("ProgressBar").GetComponent<Slider>();
        player = GameObject.Find("MainCamera");
        if (player != null) {
            Debug.Log("SPAWNING");
            ProgressBar.value = timerProgress / musicSource.clip.length;

            if (timer>beat+0.1)
            {
                Vector3 playerPos = player.transform.position;
                Vector3 playerDirection = player.transform.forward;
                Quaternion playerRotation = player.transform.rotation;
                float spawnDistance = 0.7f;

                Vector3 spawnOffset =Quaternion.Euler(0, Random.Range(-30, 30), 0) * playerDirection * spawnDistance; 
                Vector3 spawnPos = playerPos + spawnOffset;
                Debug.Log(spawnPos);
                

                int targetIdx = Random.Range(0, 3);
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
