using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBackgroundScript : MonoBehaviour
{

        private SpriteRenderer sr;
        // Start is called before the first frame update
        void Start()
        {
                sr = GetComponent<SpriteRenderer>();
                sr.size = new Vector2(15, 2.5f);
        }

        // Update is called once per frame
        void Update()
        {
                transform.position += new Vector3(0.3f * Time.deltaTime, 0, 0);
                if (transform.position.x > 15) {
                        transform.position -= new Vector3(10.3f, 0, 0);
                }
        }
}
