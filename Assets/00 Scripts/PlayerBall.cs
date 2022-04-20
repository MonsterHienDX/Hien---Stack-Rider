using System;
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
    private float scaleRateSpeed;
    private SphereCollider sphereCollider;
    public int coinInLevel;
    public bool isStop;
    public Stack<Ball> ballsCollected;
    public static PlayerBall instance;
    [SerializeField] private GameObject meshGO;

    public float ballRotateRateSpeed;

    [SerializeField] private GameObject smokeFXPrefab;
    [HideInInspector] public GameObject smokeFX;

    private Vector3 showTextPos;
    private void Awake()
    {
        instance = this;
        sphereCollider = GetComponent<SphereCollider>();
    }

    void Start()
    {
        smokeFX = Instantiate<GameObject>(smokeFXPrefab, this.transform);
        StopMove();
        ballsCollected = new Stack<Ball>();
    }

    public void StartPlay()
    {
        StartMove();
        PostEventUpdateBall(Constant.RUN_BACKWARD);
        EventDispatcher.Instance.PostEvent(EventID.StartPlay);
    }

    private void BallRotateAnim(bool isAhead)
    {
        if (isAhead)
        {
            meshGO.transform.Rotate(speed * ballRotateRateSpeed, 0, 0, Space.World);
        }
        else
        {
            meshGO.transform.Rotate(-speed * ballRotateRateSpeed, 0, 0, Space.World);
        }
    }


    void Update()
    {
        if (isStop) return;
        if (transform.position.z > endPoint.position.z)
        {
            transform.Translate(endPoint.position * Time.deltaTime * speed);
            BallRotateAnim(ballsCollected.Count % 2 == 1);
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

        this.meshGO.SetActive(true);
        sphereCollider.enabled = true;

        PostEventUpdateBall(Constant.IDLE);

        this.transform.position = levelInfo.startPoint.position;
        this.endPoint = levelInfo.endPoint;
        this.mapBallContainer = levelInfo.ballMapContainer;
        this.scaleRateSpeed = levelInfo.scaleRateSpeed;
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
            if (ballsCollected.Count > 0 && !wallCollision.isCollided && !ballsCollected.Peek().isLoseInMap)
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

    private void PostEventUpdateBall(int state)
    {
        EventDispatcher.Instance.PostEvent(EventID.ChangeCharacterState, state);
    }

    public void CollectBall(Ball newBall)
    {

        ReverseAllBallRollDir();

        ballsCollected.Push(newBall);

        newBall.rollAhead = true;

        showTextPos = new Vector3(-0.7f, 1f, this.transform.position.z);
        GameManager.instance.ShowFloatingText(($"+" + 1), 60, Color.yellow, showTextPos, Vector3.up * 60, 1f);

        int characterState = ballsCollected.Count % 2 == 0 ? Constant.RUN_BACKWARD : Constant.RUN_FAST;
        PostEventUpdateBall(characterState);

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

        Vibrator.Vibrate(Constant.STRONG_VIBRATE);
        SetSmokeFXPosition();

    }

    private void ReverseAllBallRollDir()
    {
        foreach (Ball b in ballsCollected)
        {
            b.ReverseRollDir(ballsCollected.Count);
        }
    }

    private Ball LoseBall()
    {
        Ball ballLose = ballsCollected.Pop();
        ballLose.transform.SetParent(mapBallContainer);
        ballLose.isLoseInMap = true;

        ReverseAllBallRollDir();

        int characterState = ballsCollected.Count % 2 == 0 ? Constant.RUN_BACKWARD : Constant.RUN_FAST;
        PostEventUpdateBall(characterState);
        Vibrator.Vibrate(Constant.STRONG_VIBRATE);
        SetSmokeFXPosition();
        return ballLose;
    }
    public void LoseBallByWall()
    {
        LoseBall();
    }

    public void LoseBallByLava()
    {
        Destroy(LoseBall().gameObject);

        // Play FX ball destroy by lava

    }

    public void CollectCoin(int amount)
    {
        coinInLevel += amount;
        EventDispatcher.Instance.PostEvent(EventID.UpdateCoin, coinInLevel);
        EventDispatcher.Instance.PostEvent(EventID.UpdateScore, amount);
        Vibrator.Vibrate(Constant.WEAK_VIBRATE);
    }

    public void StopMove()
    {
        this.speed = 0;
        smokeFX.GetComponentInChildren<ParticleSystem>().Pause();
        smokeFX.SetActive(false);
        // smokeFX.GetComponentInChildren<ParticleSystem>().Pause();
        isStop = true;

    }

    public void StartMove()
    {
        this.speed = canConfigSpeed * scaleRateSpeed;
        smokeFX.SetActive(true);
        smokeFX.GetComponentInChildren<ParticleSystem>().Play();
        isStop = false;
        SetSmokeFXPosition();
    }

    public void SetSmokeFXPosition()
    {
        if (ballsCollected.Count < 1)
        {
            smokeFX.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
        else
        {
            Transform ballTransform = ballsCollected.Peek().gameObject.transform;
            smokeFX.transform.position = new Vector3(ballTransform.position.x, ballTransform.position.y + 1, ballTransform.position.z);
        }
        // smokeFX.transform.position = new Vector3(transform.position.x, ballsCollected.Count + 1, transform.position.z);
    }

    public float GetSpeed()
    {
        return this.speed;
    }

    public void DestroyBallWhenWinAndShowPopup()
    {
        int explodeTime = ballsCollected.Count + 1;
        StartCoroutine(DestroyBallWhenWinCoroutine(Constant.DELAY_TO_DESTROY_BALL / explodeTime));
    }

    IEnumerator DestroyBallWhenWinCoroutine(float delay)
    {
        int coinCount = 0;
        while (ballsCollected.Count > 0)
        {
            yield return new WaitForSeconds(delay);
            Destroy(ballsCollected.Pop().gameObject);
            coinCount += 5;
            CollectCoin(coinCount);

            showTextPos = new Vector3(-0.85f, 1f, this.transform.position.z);
            GameManager.instance.ShowFloatingText(($"+" + coinCount), 60, Color.yellow, showTextPos, Vector3.up * 60, .5f);

            // ____Play FX ball explode____

        }
        yield return new WaitForSeconds(delay);

        coinCount += 5;
        CollectCoin(coinCount);

        showTextPos = new Vector3(-0.85f, 1f, this.transform.position.z);
        GameManager.instance.ShowFloatingText(($"+" + coinCount), 60, Color.yellow, showTextPos, Vector3.up * 60, .5f);

        this.meshGO.SetActive(false);
        sphereCollider.enabled = false;

        GameManager.instance.endGamePanelManager.ShowPopup(true);
        // EventDispatcher.Instance.PostEvent(EventID.BallsExpodeDone);
    }
}
