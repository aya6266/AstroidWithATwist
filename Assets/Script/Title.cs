using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject titleObjectSwitch;
    void Start()
    {
        titleObjectSwitch = gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(GameManger.isGameOver) 
        {
            titleObjectSwitch.SetActive(true);
        }
        else
        {
            titleObjectSwitch.SetActive(false);
        }
        
    }
}
