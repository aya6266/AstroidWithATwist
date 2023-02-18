using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   private Transform tr;
   private Renderer[] renderers;
   
   private bool is_Wrapping_state_X = false;
   private bool is_Wrapping_state_Y = false;
   
   private void Awake() {
         tr = GetComponent<Transform>();
         renderers = GetComponentsInChildren<Renderer>();
   }
   private void Update() {
         ScreenWrap();
   }
   private void OnTriggerEnter2D(Collider2D other) 
   {
        //adds damaged to collided object
        
        GameManger.Instance.RemoveLive(other.gameObject.tag);
        
        Destroy(gameObject);
   }
   private bool CheckRenderers()
    {
        foreach(Renderer r in renderers)
        {
            if(r.isVisible)
            {
                return true;
            }
        }
        return false;
    }

    private void ScreenWrap()
    {
        bool isVisible = CheckRenderers();
        if(isVisible)
        {
            is_Wrapping_state_X = false;
            is_Wrapping_state_Y = false;
            return;
        }
        if(is_Wrapping_state_X && is_Wrapping_state_Y)
        {
            return;
        }
        var cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(tr.position);
        var newPosition = tr.position;
        if(!is_Wrapping_state_X && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;
            is_Wrapping_state_X = true;
        }
        if(!is_Wrapping_state_Y && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;
            is_Wrapping_state_Y = true;
        }
        tr.position = newPosition;
    }
}
