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

    private AudioSource CheckPointSound;
    private AudioSource CoinCopperSound;
    private AudioSource CoinSilverSound;
    private AudioSource CoinGoldSound;
    private AudioSource DamageSound;
    private AudioSource HeartOnly1Sound;
    private AudioSource JumpItemSound;
    private AudioSource JumpSound;
    private AudioSource HeartItemSound;
    private AudioSource PlayerDeadSound;
    private AudioSource StageClearSound;
    private AudioSource StarItemSound;

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
        AudioSource[] audioSources = GetComponents<AudioSource>();
        CheckPointSound = audioSources[0];
        CoinCopperSound = audioSources[1];
        CoinSilverSound = audioSources[2];
        CoinGoldSound = audioSources[3];
        DamageSound = audioSources[4];
        HeartOnly1Sound = audioSources[5];
        JumpItemSound = audioSources[6];
        JumpSound = audioSources[7];
        HeartItemSound = audioSources[8];
        PlayerDeadSound = audioSources[9];
        StageClearSound = audioSources[10];
        StarItemSound = audioSources[11];
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
            CoinGoldSound.PlayOneShot(CoinGoldSound.clip);
            CoinScore += 100;
            Destroy(collision.gameObject);
        }
        else if (collision.name.Contains("SilverCoin"))
        {
            CoinSilverSound.PlayOneShot(CoinSilverSound.clip);
            CoinScore += 10;
            Destroy(collision.gameObject);
        }
        else if (collision.name.Contains("CopperCoin"))
        {
            CoinCopperSound.PlayOneShot(CoinCopperSound.clip);
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
            RemainingHeart = ObtainedHeart;
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