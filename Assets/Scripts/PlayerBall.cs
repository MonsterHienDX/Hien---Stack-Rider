using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    public Transform endPoint;
    [SerializeField] private Transform ballsContainer;
    [SerializeField] private Transform mapBallContainer;
    public float canConfigSpeed;
    private float speed;
    private SphereCollider sphereCollider;
    public int coinInLevel;
    public bool isStop;
    public Stack<Ball> ballsCollected;
    public static PlayerBall instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StopMove();
        ballsCollected = new Stack<Ball>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void StartPlay()
    {
        StartMove();
    }

    void Update()
    {
        if (isStop) return;
        if (transform.position.z > endPoint.position.z)
        {
            transform.Translate(endPoint.position * Time.deltaTime * speed);
        }
        else
        {
            GameManager.instance.EndLevel(true);
        }
    }

    public void Init(LevelInfo levelInfo)
    {
        StopMove();
        ballsCollected = new Stack<Ball>();
        // ballsCollected.Clear();
        if (ballsContainer.transform.childCount > 0)
        {
            for (int i = 0; i < ballsContainer.transform.childCount; i++)
            {
                Destroy(ballsContainer.transform.GetChild(i).gameObject);
            }
        }

        this.coinInLevel = 0;

        this.transform.position = levelInfo.startPoint.position;
        this.endPoint = levelInfo.endPoint;
        this.mapBallContainer = levelInfo.mapBallManager;
    }


    public void DragLeftRight(float xPos)
    {
        this.transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Ball")
        {
            Ball ballCollision = coll.gameObject.GetComponent<Ball>();
            if (!ballCollision.isCollected)
                CollectBall(ballCollision);
        }

        else if (coll.gameObject.tag == "Wall")
        {
            Wall wallCollision = coll.gameObject.GetComponent<Wall>();
            if (ballsCollected.Count > 0 && !wallCollision.isCollided)
            {
                LoseBall();
                wallCollision.isCollided = true;
            }
            else
            {
                GameManager.instance.EndLevel(false);
            }
        }

        else if (coll.gameObject.tag == "Coin")
        {
            Coin coin = coll.gameObject.GetComponent<Coin>();
            CollectCoin(coin.coinAmount);
            coll.gameObject.SetActive(false);
        }

        else if (coll.gameObject.tag == "Lava")
        {
            if (ballsCollected.Count > 0)
            {
                LoseBallByLava();
            }
            else
            {
                GameManager.instance.EndLevel(false);
            }
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Ball")
        {
            Ball ballCollision = coll.gameObject.GetComponent<Ball>();
            if (!ballCollision.isCollected)
                CollectBall(ballCollision);
        }

        else if (coll.gameObject.tag == "Wall")
        {
            Wall wallCollision = coll.gameObject.GetComponent<Wall>();
            if (ballsCollected.Count > 0 && !wallCollision.isCollided)
            {
                LoseBall();
                wallCollision.isCollided = true;
            }
            else
            {
                GameManager.instance.EndLevel(false);
            }
        }

        else if (coll.gameObject.tag == "Coin")
        {
            Coin coin = coll.gameObject.GetComponent<Coin>();
            CollectCoin(coin.coinAmount);
            coll.gameObject.SetActive(false);
        }

        else if (coll.gameObject.tag == "Lava")
        {
            if (ballsCollected.Count > 0)
            {
                LoseBallByLava();
            }
            else
            {
                GameManager.instance.EndLevel(false);
            }
        }
    }

    public void CollectBall(Ball newBall)
    {
        ballsCollected.Push(newBall);
        // Debug.LogWarning("ballsCollected.Count: " + ballsCollected.Count);

        transform.position = new Vector3(transform.position.x, ballsCollected.Count - 1, transform.position.z);
        // sphereCollider.center = new Vector3(sphereCollider.center.x, sphereCollider.center.y - 1, sphereCollider.center.z);

        newBall.isCollected = true;

        if (ballsCollected.Count < 1)
        {
            newBall.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        }
        else
        {
            newBall.transform.position = new Vector3(transform.position.x, ballsCollected.Peek().gameObject.transform.position.y - 1, transform.position.z);
        }
        newBall.transform.SetParent(ballsContainer);
    }

    private Ball LoseBall()
    {
        Ball ballLose = ballsCollected.Pop();
        ballLose.transform.SetParent(mapBallContainer);
        return ballLose;

    }
    public void LoseBallByWall()
    {
        LoseBall();
    }

    public void LoseBallByLava()
    {
        Debug.LogWarning("LoseBallByLava");
        Destroy(LoseBall().gameObject);

        // Play FX ball destroy by lava

    }

    public void CollectCoin(int amount)
    {
        coinInLevel += amount;
        EventDispatcher.Instance.PostEvent(EventID.UpdateCoin, coinInLevel);
    }

    public void StopMove()
    {
        this.speed = 0;
        isStop = true;
    }

    public void StartMove()
    {
        this.speed = canConfigSpeed;
        isStop = false;
    }
}
