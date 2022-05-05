using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void PointerClick(object sender, PointerEventArgs e)
    // {
    //     if (e.target.name == "Button")
    //     {
    //         Debug.Log("Button was clicked");
    //         MoveToScene(1);
    //     }
    // }

    public void MoveToScene(int sceneID){
        SceneManager.LoadScene(sceneID);
    }
}
