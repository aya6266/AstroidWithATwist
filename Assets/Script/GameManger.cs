using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : Singleton<GameManger>
{
    public GameManger instance;
    
    //constants variables
    public float speed = 1f;
    public float rotationSpeed = 1f;
    public float invisiblityTime = 2f;
    public int maxNumOfAsteroids = 5;
    public float speedOfAsteroids = 1f;
    private float smallAstroidScale = 1f;
    private float mediumAstroidScale = 2f;
    private float largeAstroidScale = 3f;
    
    
    //prefabs
    public GameObject redShipPrefab;
    public GameObject blueShipPrefab;
    public GameObject smallAstroid;
    public GameObject mediumAstroid;
    public GameObject largeAstroid;

    //Array of ships
    private GameObject[] ships = new GameObject[3];
    //private float[] shipScale = {smallAstroidScale, mediumAstroidScale, largeAstroidScale};
    
    //lives variables
    public static int redShipLive;
    private bool redShipInvincible = false;
    public static int blueShipLive;
    private bool blueShipInvincible = false;
    private bool oneLifeLeft = false;
    private bool oneLifeLeftPlayed = false;
    
    //ship references
    private GameObject redShip;
    private GameObject blueShip;
    
    //astroid total
    private int currentNumOfAsteroids;

    //Score vars
    public int currentScore;    
    public int highScore;

    //game state
    public static bool isGameOver = true;

    //scripts of ships
    private Ship redScript;
    private Ship blueScript;

    //Check if the game is muted
    public bool isMutedMusic = false;
    public bool isMutedSound = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //StartGame();
        ships[0] = smallAstroid;
        ships[1] = mediumAstroid;
        ships[2] = largeAstroid;
        
        redShipLive = 3;
        blueShipLive = 3;
        //get access to the public methods of the spaceships
        AudioManager.Instance.Play("GameLoopBeat");
        //playerPrefs
        isMutedSound = PlayerPrefs.GetInt("isMutedSound") == 1 ? true : false;
        isMutedMusic = PlayerPrefs.GetInt("isMutedMusic") == 1? true : false;
        highScore = PlayerPrefs.GetInt("highScore");
    }

    // Update is called once per frame
    void Update()
    {
        if(oneLifeLeft && !oneLifeLeftPlayed)
        {
            AudioManager.Instance.Play("ShipOneHealthLeft");
            oneLifeLeftPlayed = true;
        }
        if(!isGameOver && currentNumOfAsteroids < maxNumOfAsteroids)
        {
            
            SpawnAstroid();
        }
        if(isGameOver)
        {
            DestroyAllGameObjects();

            if(Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
            
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            isMutedSound = !isMutedSound;
            PlayerPrefs.SetInt("isMutedSound", isMutedSound ? 1 : 0);
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            isMutedMusic = !isMutedMusic;
            PlayerPrefs.SetInt("isMutedMusic", isMutedMusic ? 1 : 0);
        }
        
    }

    private void StartGame()
    {
        redShipLive = 3;
        blueShipLive = 3;
        currentNumOfAsteroids = 0;
        redShip = Instantiate(redShipPrefab, new Vector3(-2.5f, 0, 0), Quaternion.identity);
        blueShip = Instantiate(blueShipPrefab, new Vector3(2.5f, 0, 0), Quaternion.identity);
        currentScore = 0;
        isGameOver = false;
        oneLifeLeft = false;
        oneLifeLeftPlayed = false;

        //assign scripts var from instantiate of the ships
        redScript = redShip.GetComponent<Ship>();
        blueScript = blueShip.GetComponent<Ship>();
    }

    private void GameOver()
    {
        AudioManager.Instance.Stop("ShipOneHealthLeft");
        AudioManager.Instance.Play("ShipDeath");
        if(currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("highScore", highScore);
        }
        isGameOver = true;
        
    }

    private void DestroyAllGameObjects()
    {
        
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach(GameObject obj in allObjects)
        {
            
            if(obj.tag == "Astroid" || obj.tag == "Damage" || obj.tag == "Red" || obj.tag == "Blue")
            {
                Destroy(obj);
            }
            
        }
        
    }

    public void RemoveLive(string ship)
    {
        if(ship == "Red" && !redShipInvincible)
        {
            redShipLive--;
            redShipInvincible = true;
            //animation
            if(redScript) redScript.redInv();
            AudioManager.Instance.Play("ShipHit");
            Invoke("InvertRedShipInvincible", invisiblityTime);
           
        }
        else if(ship == "Blue" && !blueShipInvincible)
        {
            blueShipLive--;
            blueShipInvincible = true;
            //animation
            if(blueScript) blueScript.blueInv();
            AudioManager.Instance.Play("ShipHit");
            Invoke("InvertBlueShipInvincible", invisiblityTime);
            

        }
        
        if(redShipLive == 1 || blueShipLive == 1)
        {
            oneLifeLeft = true;
        }
        if(redShipLive == 0)
        {
            if(redScript) redScript.TriggerDeathAnimation();       
            GameOver();
        }
        if(blueShipLive == 0)
        {
            if(blueScript) blueScript.TriggerDeathAnimation();
            GameOver();
        }
        
    }

    
    
    public void AddScore(int score)
    {
        currentScore += score;
        currentNumOfAsteroids--;
    }

    private void InvertRedShipInvincible()
    {
        redShipInvincible = false;
        //animation
        if(redScript) redScript.redInvEnd();
    }
    private void InvertBlueShipInvincible()
    {
        blueShipInvincible = false;
        //animation
        if(blueScript) blueScript.blueInvEnd();
    }

    private void SpawnAstroid()
    {
        //first choose the 4 axis then choose the random pos they will spawn
        while(currentNumOfAsteroids < maxNumOfAsteroids)
        {
            int randomAxis = Random.Range(0, 4);
            float randomSize = WeightedRandomSize();
            GameObject astroid = ships[Random.Range(0, ships.Length)];
            //Axis relate 0 = North 1 = East 2 = South 3 = West
            switch(randomAxis)
            {
                case 0:
                    SpawnAstroid(astroid, new Vector3(Random.Range(-10, 10), 10, 0), randomSize);
                    break;
                case 1:
                    SpawnAstroid(astroid, new Vector3(10, Random.Range(-10, 10), 0), randomSize);
                    break;
                case 2:
                    SpawnAstroid(astroid, new Vector3(Random.Range(-10, 10), -10, 0), randomSize);
                    break;
                case 3:
                    SpawnAstroid(astroid, new Vector3(-10, Random.Range(-10, 10), 0), randomSize);
                    break;
            }
            currentNumOfAsteroids++;
            
        }
    }

    private void SpawnAstroid(GameObject astroid, Vector3 pos, float size)
    {
        var ast = Instantiate(astroid, pos, Quaternion.identity);
        ast.transform.localScale = new Vector3(size, size, 1);
                    
    }

    private float WeightedRandomSize()
    {
        float randomSize = Random.Range(0f, 1f);
        if(randomSize < 0.5f)
        {
            return smallAstroidScale;
        }
        else if(randomSize < 0.8f)
        {
            return mediumAstroidScale;
        }
        else
        {
            return largeAstroidScale;
        }
    }

    public bool GameState()
    {
        return isGameOver;
    }
}
