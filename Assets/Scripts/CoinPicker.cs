using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
public class CoinPicker : MonoBehaviour
{
    private float coins = 0;
    public TMP_Text coinsText;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Coin")
        {
            coins++;
            coinsText.text=coins.ToString();
            Destroy(collision.gameObject);
        }
    }
}
