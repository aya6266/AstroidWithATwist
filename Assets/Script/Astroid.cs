using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    public string type;
    private bool is_Wrapping_state_X = false;
    private bool is_Wrapping_state_Y = false;
    private Transform tr;
    private Renderer[] renderers;
    private int life;
    public Animator animator;
    

  private void Awake() 
  {
    var rb = GetComponent<Rigidbody2D>();
    tr = GetComponent<Transform>();
    renderers = GetComponentsInChildren<Renderer>();
    rb.velocity = Random.insideUnitCircle * 3;  
    life = Random.Range(1, 3);
    animator = GetComponent<Animator>();
    
  }

  private void Update()
   {
    ScreenWrap();
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

    private void OnCollisionEnter2D(Collision2D other) {
        
        if(other.gameObject.tag == "Damage")
        {
            life--;
            
            if(life <= 0)
            {
                GameManger.Instance.AddScore(10);
                
            }
        }
        if(other.gameObject.tag == "Blue")
        {
            GameManger.Instance.RemoveLive("Blue");
        }
        if(other.gameObject.tag == "Red")
        {
            GameManger.Instance.RemoveLive("Red");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
        
    {
        
        if(other.gameObject.tag == "Damage")
        {
            life--;
            
            if(life <= 0)
            {
                GameManger.Instance.AddScore(10);
                AudioManager.Instance.Play("AstroidExplosion");
                if(animator)
                {
                    animator.SetBool("isDead", true);
                    
                } 
            }
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    
   

}
