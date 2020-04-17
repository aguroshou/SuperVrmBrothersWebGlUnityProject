using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowCollisionSetting : MonoBehaviour
{
    public Text CoinScoreText;
    public long CoinScore;
    public long ObtainedStar;
    public long ObtainedJumpItem;
    public long ObtainedHeart;
    public Transform RespawnPoint;
    public GameObject PastCheckPoint;

    // Start is called before the first frame update
    void Start()
    {
        CoinScore = 0;
        ObtainedStar = 0;
        ObtainedJumpItem = 0;
        ObtainedHeart = 0;
        RespawnPoint = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        CoinScoreText.text = "SCORE" + CoinScore;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            this.transform.position = RespawnPoint.position;
        }
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
            Destroy(collision.gameObject);
        }
        else if (collision.name.Contains("CheckPoint"))
        {
            RespawnPoint = collision.transform;
            if (PastCheckPoint != null)
            {
                MeshRenderer PastCheckPointMesh = PastCheckPoint.GetComponent<MeshRenderer>();
                PastCheckPointMesh.material.color = Color.gray;
            }
            PastCheckPoint = collision.gameObject;
            MeshRenderer TmpMesh = collision.GetComponent<MeshRenderer>();
            TmpMesh.material.color = Color.green;
        }
    }
}
