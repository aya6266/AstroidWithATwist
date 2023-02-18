using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float bulletSpeed = 10f;
    public float bulletrate = 1.0f;
    public bool canFire = true;
    public GameObject bulletPrefab;
    private Rigidbody2D rb;
    private Transform tr;
    private Renderer[] renderers;
    //pretty sure theres a better way to do this
    public Animator animator;

    private ParticleSystem part;
   

    
    // Start is called before the first frame update
    
    private bool forward_state = false;
    private bool leftRot_state = false;
    private bool rightRot_state = false;
    private bool shoot_state = false;
    private bool is_Wrapping_state_X = false;
    private bool is_Wrapping_state_Y = false;

    //state of the sound
    private bool flying_sound_state = false;
   

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        renderers = GetComponentsInChildren<Renderer>();
        
        speed = GameManger.Instance.speed;
        rotationSpeed = GameManger.Instance.rotationSpeed;
        
        //animation code
        animator = GetComponent<Animator>();
        //part = GetComponent<ParticleSystem>();
        

    }

    // Update is called once per frame
    private void Update()
    {
        //part.Stop();
        CurrentMove();
        ScreenWrap();
    }

    private void FixedUpdate() {
        Movement();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if(other.gameObject.tag == "Damage")
        {
            GameManger.Instance.RemoveLive(gameObject.tag);
        }    
    }

    private void OnTriggerEnter(Collider other) {
     
        if(other.gameObject.tag == "Damage" || other.gameObject.tag == "Asteroid")
        {
            GameManger.Instance.RemoveLive(gameObject.tag);
        }
        
    }

    private void DestroySelf()
    {
        Debug.Log("DS was triggered");
        Destroy(gameObject);
    }
    public void TriggerDeathAnimation()
    {

        if(animator) animator.SetBool("isDead", true);
    }

    private void Movement()
    {

        if(forward_state)
        {
            rb.velocity = transform.up * speed;
        }
        if(leftRot_state)
        {
//            rb.rotation = rb.rotation + rotationSpeed;
            Vector3 rot = new Vector3(tr.rotation[0],tr.rotation[1],tr.rotation[2]+rotationSpeed);
             tr.Rotate(rot);

        }
        else if(rightRot_state)
        {
//            rb.rotation = rb.rotation - rotationSpeed;
            Vector3 rot = new Vector3(tr.rotation[0],tr.rotation[1],tr.rotation[2]-
            
            rotationSpeed);
             tr.Rotate(rot);


        }
        if(shoot_state && canFire && GameManger.Instance.GameState() == false)
        {
            Shoot();
            canFire = false;
            shoot_state = false;
            Invoke("InvertCanFire", bulletrate);
        }
    }

    private void CurrentMove()
    {
        if(Input.GetKey(KeyCode.W))
        {
            forward_state = true;
            animator.SetBool("isFlying", true);
            if(!flying_sound_state)
            {
                AudioManager.Instance.Play("Flying");
                flying_sound_state = true;
            }
        }
        else{
            forward_state = false;
            animator.SetBool("isFlying", false);
            flying_sound_state = false;
        }

        if(Input.GetKey(KeyCode.A))
        {
            leftRot_state = true;
        }
        
        else if(Input.GetKey(KeyCode.D))
        {
            rightRot_state = true;
        }
        else
        {
            leftRot_state = false;
            rightRot_state = false;
        }
        //need to get KeyCode
        if(canFire)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                shoot_state = true;
            }
        }
        

    }
    private void Shoot()
    {
        var bulletPos = tr.position + tr.up * 0.75f;
        AudioManager.Instance.Play("GunShot");
        GameObject bull = Instantiate(bulletPrefab, bulletPos, tr.rotation);
        bull.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
        
    }

    private void InvertCanFire()
    {
        canFire = true;
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
           // part.Play();
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
            //part.Stop();
            newPosition.x = -newPosition.x;
            is_Wrapping_state_X = true;
        }
        if(!is_Wrapping_state_Y && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            //part.Stop();
            newPosition.y = -newPosition.y;
            is_Wrapping_state_Y = true;
        }
        tr.position = newPosition;
    }

    public void redInv()
    {
        animator.SetBool("isInv", true);
    }
    public void redInvEnd()
    {
        animator.SetBool("isInv", false);
    }
    public void blueInv()
    {
        animator.SetBool("isInv", true);
    }
    public void blueInvEnd()
    {
        animator.SetBool("isInv", false);
    }
}
