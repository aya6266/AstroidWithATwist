using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLiveUI : MonoBehaviour
{
    public GameObject redShipLive1;
    public GameObject redShipLive2;
    public GameObject redShipLive3;
    // Start is called before the first frame update
    void Start()
    {
        redShipLive1 = gameObject.transform.GetChild(1).gameObject;
        redShipLive2 = gameObject.transform.GetChild(2).gameObject;
        redShipLive3 = gameObject.transform.GetChild(3).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
      //set to deactive
        if (GameManger.redShipLive == 2)
        {
            redShipLive3.SetActive(false);
        }
        else if (GameManger.redShipLive == 1)
        {
            redShipLive2.SetActive(false);
        }
        else if (GameManger.redShipLive == 0)
        {
            redShipLive1.SetActive(false);
        }
        else
        {
            redShipLive1.SetActive(true);
            redShipLive2.SetActive(true);
            redShipLive3.SetActive(true);
        }

    }
}
