using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControllerUI : MonoBehaviour
{
    public GameObject sound;
    public GameObject muteSound;
    public GameObject music;
    public GameObject muteMusic;


    // Start is called before the first frame update
    void Start()
    {
        sound = gameObject.transform.GetChild(0).gameObject;
        muteSound = gameObject.transform.GetChild(1).gameObject;
        music = gameObject.transform.GetChild(2).gameObject;
        muteMusic = gameObject.transform.GetChild(3).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManger.Instance.isMutedMusic)
        {
            music.SetActive(false);
            muteMusic.SetActive(true);
        }
        else
        {
            music.SetActive(true);
            muteMusic.SetActive(false);
        }
        if(GameManger.Instance.isMutedSound)
        {
            sound.SetActive(false);
            muteSound.SetActive(true);
        }
        else
        {
            sound.SetActive(true);
            muteSound.SetActive(false);
        }
    }
}
