using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitboxController : MonoBehaviour
{
        private float lifetime;
        // Start is called before the first frame update
        void Start()
        {
                lifetime = 0;
        }

        // Update is called once per frame
        void Update()
        {
                // Destroy after 1/4 second
                lifetime += Time.deltaTime;
                if (lifetime > 0.25) Destroy(gameObject);
        }

        // Interaction with enemies
        void OnTriggerEnter2D(Collider2D collision)
        {
                //Debug.Log("Found");
                if (collision.tag == "Enemy") {
                        //collision.GetComponent<Rigidbody2D>().velocity = new Vector2(5, 0);
                        //Debug.Log("Hit");
                        collision.GetComponent<EnemyControllerB>().Hit();
                }
        }
}
