using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEyeBasicController : EnemyParentController
{
        private int state;

        // Sprite parts
        private GameObject sclera;
        private GameObject pupil;

        // Eye animation
        private float animationTime;
        private float angleCurrent;
        private float angleTarget;
        // Start is called before the first frame update
        void Start()
        {
                // Getting sprite parts
                sclera = transform.GetChild(0).gameObject;
                pupil = transform.GetChild(1).gameObject;
        }

        // Update is called once per frame
        void Update()
        {
                /*
                animationTime += Time.deltaTime;
                if (animationTime > 1) {
                        angleTarget = Random.Range(0, 360);
                }
                */

                pupil.transform.localPosition = new Vector2(Mathf.Cos(angleCurrent * Mathf.Deg2Rad), Mathf.Sin(angleCurrent * Mathf.Deg2Rad));

                if (state == 1) {
                        angleCurrent += Time.deltaTime * 90;
                }

        }

        public override void SetDirection(int dir) { 
        
        }

        public override void Hook(GameObject bobber) {
                bob = bobber.GetComponent<BobberController>();
                if (bob.GetState() == 1)
                {
                        bob.SetState(2);
                        bob.SetFish(this);
                        state = 1;

                }
        }

        public override int Reel(ItemController item) {
                Destroy(gameObject);
                item.Reset();
                return 0;
        
        }
        public override int Release(ItemController item) {
                return 0;
        }
}
