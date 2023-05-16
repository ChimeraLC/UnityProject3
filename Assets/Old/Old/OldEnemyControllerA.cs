using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OldEnemyControllerA : MonoBehaviour
{
        private Rigidbody2D bodyRb;
        public GameObject shatter;
        private int hp;
        // Start is called before the first frame update
        void Start()
        {
                bodyRb = GetComponent<Rigidbody2D>();
                hp = 2;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void PlayerHit(float hitAngle)
        {
                if (hp-- <= 0)
                {
                        Instantiate(shatter, transform.position, shatter.transform.rotation);
                        Destroy(gameObject);
                }
                else
                {
                        bodyRb.AddForce(new Vector2(-Mathf.Cos(hitAngle * Mathf.Deg2Rad),
                            -Mathf.Sin(hitAngle * Mathf.Deg2Rad)) * -500f);
                        Debug.Log("I've been hit");
                }
        }

}
