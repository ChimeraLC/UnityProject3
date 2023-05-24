using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish1Controller : FishParentController
{
        private int direction = -1;
        private int state = 0;

        private Animator fishAnim;
        // Start is called before the first frame update
        void Start()
        {
                fishAnim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
                // Traveling through water
                if (state == 0) {
                        transform.position += Time.deltaTime *
                            new Vector3((direction % 2) * (2 - direction), ((direction - 1) % 2) * (direction - 3), 0);
                }
        }

        public override void Hook() { 
        
        }

        public override void SetDirection(int dir) {
                direction = dir;
                GetComponent<Animator>().SetInteger("direction", dir);
        }
}
