using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    public Text CoinScoreText;
    public Text ClearText;
    public Text ResetText;
    public long ResetNumberOfTimes;
    public long CoinScore;
    public long ObtainedStar;
    public long ObtainedJumpItem;
    public long ObtainedHeart;
    public long NeedStar;
    public long RemainingHeart;
    public long RemainingJump;
    public Vector3 RespawnPoint;
    public GameObject PastCheckPoint;
    private MoveBehaviour MoveBehaviorScript;

    // Start is called before the first frame update
    void Start()
    {
        ResetNumberOfTimes = 0;
        CoinScore = 0;
        ObtainedStar = 0;
        ObtainedJumpItem = 0;
        ObtainedHeart = 1;
        RemainingHeart = 1;
        RespawnPoint = this.transform.position;
        MoveBehaviorScript = this.GetComponent<MoveBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        CoinScoreText.text = "SCORE" + CoinScore + "\n☆" + ObtainedStar + "/" + NeedStar + "\n♡" + RemainingHeart + "/" + ObtainedHeart + "\n◯" + MoveBehaviorScript.AirJumpRemaining + "/" + ObtainedJumpItem;
        if (Input.GetKeyDown(KeyCode.Return) || RemainingHeart <= 0)
        {
            this.transform.position = RespawnPoint;
            RemainingHeart = ObtainedHeart;
            ResetNumberOfTimes++;
        }
        if (ObtainedStar >= NeedStar)
        {
            ClearText.text = "STAGE CLEAR";
        }
        ResetText.text = "RESET" + ResetNumberOfTimes;
    }

    // 他のオブジェクトと衝突した時に呼び出される関数
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name.Contains("GoldCoin"))
        {
            CoinScore += 100;
            Destroy(collision.gameObject);
        }
        else if (collision.name.Contains("SilverCoin"))
        {
            CoinScore += 10;
            Destroy(collision.gameObject);
        }
        else if (collision.name.Contains("CopperCoin"))
        {
            CoinScore++;
            Destroy(collision.gameObject);
        }
        else if (collision.name.Contains("Star"))
        {
            ObtainedStar++;
            Destroy(collision.gameObject);
        }
        else if (collision.name.Contains("JumpItem"))
        {
            ObtainedJumpItem++;
            Destroy(collision.gameObject);
        }
        else if (collision.name.Contains("Heart"))
        {
            ObtainedHeart++;
            RemainingHeart++;
            Destroy(collision.gameObject);
        }
        else if (collision.name.Contains("CheckPoint"))
        {
            RemainingHeart = ObtainedHeart;
            RespawnPoint = collision.transform.position;
            if (PastCheckPoint != null)
            {
                MeshRenderer PastCheckPointMesh = PastCheckPoint.GetComponent<MeshRenderer>();
                PastCheckPointMesh.material.color = Color.gray;
            }
            PastCheckPoint = collision.gameObject;
            MeshRenderer TmpMesh = collision.GetComponent<MeshRenderer>();
            TmpMesh.material.color = Color.green;
        }
        else if (collision.name.Contains("Magma"))
        {
            RemainingHeart = 0;
        }
    }
}
