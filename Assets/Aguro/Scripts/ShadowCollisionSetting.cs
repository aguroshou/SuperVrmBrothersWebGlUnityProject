using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowCollisionSetting : MonoBehaviour
{
    public Text CoinScoreText;
    public long CoinScore;

    // Start is called before the first frame update
    void Start()
    {
        CoinScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CoinScoreText.text = "SCORE" + CoinScore;
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
            CoinScore += 1;
            Destroy(collision.gameObject);
        }
    }
}
