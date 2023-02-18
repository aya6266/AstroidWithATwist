using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueLiveUI : MonoBehaviour
{
    public GameObject blueShipLive1;
    public GameObject blueShipLive2;
    public GameObject blueShipLive3;
    // Start is called before the first frame update
    void Start()
    {
        blueShipLive1 = gameObject.transform.GetChild(1).gameObject;
        blueShipLive2 = gameObject.transform.GetChild(2).gameObject;
        blueShipLive3 = gameObject.transform.GetChild(3).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
      //set to deactive
        if (GameManger.blueShipLive == 2)
        {
            blueShipLive3.SetActive(false);
        }
        else if (GameManger.blueShipLive == 1)
        {
            blueShipLive2.SetActive(false);
        }
        else if (GameManger.blueShipLive == 0)
        {
            blueShipLive1.SetActive(false);
        }
        else
        {
            blueShipLive1.SetActive(true);
            blueShipLive2.SetActive(true);
            blueShipLive3.SetActive(true);
        }

    }
}
