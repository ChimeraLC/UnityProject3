using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarControlledController : MonoBehaviour
{
        public GameObject innerBar;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetRatio(float ratio) {
                innerBar.transform.localScale = new Vector2(ratio, 1);
        }
}
