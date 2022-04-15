using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform ballsContainer;
    [SerializeField] private Transform mapBallContainer;
    public float canConfigSpeed;
    private float speed;
    public bool isLose;
    private SphereCollider sphereCollider;

    private int coinInLevel;

    public Stack<Ball> ballsCollected;
    void Start()
    {
        isLose = false;
        speed = 0;
        ballsCollected = new Stack<Ball>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void StartPlay()
    {
        speed = canConfigSpeed;
    }

    void Update()
    {
        if (transform.position.z > endPoint.position.z && !isLose)
        {
            transform.Translate(endPoint.position * Time.deltaTime * speed);
        }
        else
        {
            WinLevel();
            speed = 0;
        }
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
                LoseLevel();
                Debug.LogWarning("isLose: " + isLose);
            }
        }

        else if (coll.gameObject.tag == "Coin")
        {
            Coin coin = coll.gameObject.GetComponent<Coin>();
            CollectCoin(coin.coinAmount);
            coll.gameObject.SetActive(false);
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

    public void LoseBall()
    {
        Ball ballLose = ballsCollected.Pop();
        ballLose.transform.SetParent(mapBallContainer);

        // transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        // sphereCollider.center = new Vector3(sphereCollider.center.x, sphereCollider.center.y + 1, sphereCollider.center.z);
    }

    public void CollectCoin(int amount)
    {
        coinInLevel += amount;
        Debug.Log("coinInLevel: " + coinInLevel);
    }

    public void WinLevel()
    {
        if (PlayerPrefs.HasKey(StringConstant.KEY_SAVE_COIN))
            PlayerPrefs.SetInt(StringConstant.KEY_SAVE_COIN, PlayerPrefs.GetInt(StringConstant.KEY_SAVE_COIN) + coinInLevel);
        else
        {
            PlayerPrefs.SetInt(StringConstant.KEY_SAVE_COIN, coinInLevel);
        }

    }

    public void LoseLevel()
    {
        isLose = true;
    }

}
