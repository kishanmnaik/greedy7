using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinToss : MonoBehaviour
{
    // Start is called before the first frame update
    
        public GameObject coin;
        public Sprite headSprite;
        public Sprite tailSprite;
        public Sprite[] images;
        void Update()
        {
        images = new Sprite[4];
        images[0] = headSprite;
        images[1] = tailSprite;
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Clicked");
            System.Random rnd = new System.Random();
            int num = rnd.Next(0, 2);
 
            coin.GetComponent<SpriteRenderer>().sprite = images[num];
            
        }
    }
        
    
}