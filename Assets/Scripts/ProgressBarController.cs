using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarController : MonoBehaviour
{

        public float totalLifetime = 100;
        private float lifetime;

        public GameObject innerBar;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
                // Incrementing lifetime
                lifetime += Time.deltaTime;

                // Updating bar
                innerBar.transform.localScale = new Vector2(lifetime/totalLifetime, 1);

                // Completion
                if (lifetime > totalLifetime) {
                        Destroy(gameObject);
                }
        }
}
