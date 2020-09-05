using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    private MoveBehaviour MoveBehaviorScript;
    [SerializeField] Text CoinScoreText;
    [SerializeField] Text ClearText;
    [SerializeField] Text ResetText;
    [SerializeField] int ResetNumberOfTimes;
    [SerializeField] int CoinScore;
    [SerializeField] int ObtainedStar;
    [SerializeField] int obtainedJumpItem;
    public int ObtainedJumpItem
    {
        get { return this.obtainedJumpItem; }  //取得用
        private set { this.obtainedJumpItem = value; } //値入力用
    }
    [SerializeField] int ObtainedHeart;
    [SerializeField] int NeedStar;
    [SerializeField] int RemainingHeart;
    [SerializeField] int RemainingJump;
    [SerializeField] Vector3 RespawnPoint;
    [SerializeField] GameObject PastCheckPoint;

    public AudioSource CheckPointSound;
    public AudioSource CopperCoinSound;
    public AudioSource SilverCoinSound;
    public AudioSource GoldCoinSound;
    public AudioSource DamageSound;
    public AudioSource HeartOnly1Sound;
    public AudioSource JumpItemSound;
    public AudioSource JumpSound;
    public AudioSource HeartItemSound;
    public AudioSource PlayerDeadSound;
    public AudioSource StageClearSound;
    public AudioSource StarItemSound;

    // Start is called before the first frame update
    void Start()
    {
        NeedStar = 4;
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
        CoinScoreText.text = "SCORE:" + CoinScore + "\nSTAR:" + ObtainedStar + "/" + NeedStar + "\nHEART:" + RemainingHeart + "/" + ObtainedHeart + "\nJUMP:" + MoveBehaviorScript.AirJumpRemaining + "/" + ObtainedJumpItem;
        if (Input.GetKeyDown(KeyCode.Return) || RemainingHeart <= 0)
        {
            PlayerDeadSound.PlayOneShot(PlayerDeadSound.clip);
            this.transform.position = RespawnPoint;
            RemainingHeart = ObtainedHeart;
            ResetNumberOfTimes++;
        }
        if (ObtainedStar >= NeedStar)
        {
            ClearText.text = "STAGE CLEAR";
        }
        ResetText.text = "RESET:" + ResetNumberOfTimes;
    }

    // 他のオブジェクトと衝突した時に呼び出される関数
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name.Contains("GoldCoin"))
        {
            GoldCoinSound.PlayOneShot(GoldCoinSound.clip);
            CoinScore += 100;
            Destroy(collision.gameObject);
        }
        else if (collision.name.Contains("SilverCoin"))
        {
            SilverCoinSound.PlayOneShot(SilverCoinSound.clip);
            CoinScore += 10;
            Destroy(collision.gameObject);
        }
        else if (collision.name.Contains("CopperCoin"))
        {
            CopperCoinSound.PlayOneShot(CopperCoinSound.clip);
            CoinScore++;
            Destroy(collision.gameObject);
        }
        else if (collision.name.Contains("Star"))
        {
            StageClearSound.PlayOneShot(StageClearSound.clip);
            ObtainedStar++;
            if (ObtainedStar == NeedStar)
            {
                StarItemSound.PlayOneShot(StarItemSound.clip);
            }
            Destroy(collision.gameObject);
        }
        else if (collision.name.Contains("JumpItem"))
        {
            JumpItemSound.PlayOneShot(JumpItemSound.clip);
            ObtainedJumpItem++;
            Destroy(collision.gameObject);
        }
        else if (collision.name.Contains("Heart"))
        {
            HeartItemSound.PlayOneShot(HeartItemSound.clip);
            ObtainedHeart++;
            RemainingHeart++;
            Destroy(collision.gameObject);
        }
        else if (collision.name.Contains("CheckPoint"))
        {
            RespawnPoint = collision.transform.position;
            if (PastCheckPoint != null)
            {
                MeshRenderer PastCheckPointMesh = PastCheckPoint.GetComponent<MeshRenderer>();
                PastCheckPointMesh.material.color = Color.gray;
            }
            if (PastCheckPoint != collision.gameObject)
            {
                CheckPointSound.PlayOneShot(CheckPointSound.clip);
                PastCheckPoint = collision.gameObject;
                RemainingHeart = ObtainedHeart;
            }
            MeshRenderer TmpMesh = collision.GetComponent<MeshRenderer>();
            TmpMesh.material.color = Color.green;
        }
        else if (collision.name.Contains("Magma"))
        {
            RemainingHeart = 0;
        }
        else if (collision.name.Contains("Damage"))
        {
            RemainingHeart--;
            DamageSound.PlayOneShot(DamageSound.clip);
            if (RemainingHeart == 1)
            {
                HeartOnly1Sound.PlayOneShot(HeartOnly1Sound.clip);
            }
            Destroy(collision.gameObject);
        }
    }
}