using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
        public Image boatHealthBar;
        public float boatHealth = 100f;

        // Start is called before the first frame update
        void Start()
        {
                
        }

        // Update is called once per frame
        void Update()
        {
                boatHealthBar.fillAmount = boatHealth / 100;
        }
}
