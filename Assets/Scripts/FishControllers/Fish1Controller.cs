using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Simple fish class, requires clicking to reel in
public class Fish1Controller : FishParentController
{
        private int direction = -1;
        private int state = 0; //0, swimming, 1, clamped

        private float degrees = 0;
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

                        // TODO: Destroying out of bounds
                        if (Mathf.Abs(transform.position.x) > 30 || Mathf.Abs(transform.position.y) > 30) {
                                Destroy(gameObject);
                        } 
                }
                if (state == 1) { 
                        transform.rotation = Quaternion.Euler(Vector3.forward * degrees);
                        transform.position = bob.transform.position + new Vector3(-Mathf.Cos(Mathf.Deg2Rad * degrees) * 1,
                            -Mathf.Sin(Mathf.Deg2Rad * degrees) * 1, 0);
                        degrees += Time.deltaTime * 10;
                }

                // rough boundary checker
                if (Mathf.Abs(transform.position.x) > 20 || Mathf.Abs(transform.position.y) > 20) {
                        Destroy(gameObject);
                }
        }

        public override void Hook(GameObject bobber)
        {
                bob = bobber.GetComponent<BobberController>();
                if (bob.GetState() == 1)
                {
                        bob.SetState(2);
                        bob.SetFish(this);
                        //item = bob.parentRod;
                        state = 1;

                        // Clamp bobber to fish?
                        bob.pathPosition = transform.position +
                                new Vector3((direction % 2) * (2 - direction), ((direction - 1) % 2) * (direction - 3), 0);

                        // Changing animation
                        GetComponent<Animator>().SetInteger("clamped", 1);
                        // changing pivot
                        //GetComponent<RectTransform>().pivot = 
                        //    new Vector2((direction % 2) * (2 - direction)/2 + 0.5f, ((direction - 1) % 2) * (direction - 3)/2 + 0.5f);
                }
        }

        public override void SetDirection(int dir) {
                direction = dir;
                GetComponent<Animator>().SetInteger("direction", dir);
        }
        /*
        // TODO: make this a continuous check
        public void OnTriggerEnter2D(Collider2D collision)
        {
                if (collision.CompareTag("Bobber")) {
                        bob = collision.GetComponent<BobberController>();
                        if (bob.GetState() == 1) {
                                bob.SetState(2);
                                bob.SetFish(this);
                                item = bob.parentRod;
                                state = 1;

                                // Clamp bobber to fish?
                                bob.pathPosition = transform.position + 
                                    new Vector3((direction % 2) * (2 - direction), ((direction - 1) % 2) * (direction - 3), 0);

                                // Changing animation
                                GetComponent<Animator>().SetInteger("clamped", 1);
                                // changing pivot
                                //GetComponent<RectTransform>().pivot = 
                                //    new Vector2((direction % 2) * (2 - direction)/2 + 0.5f, ((direction - 1) % 2) * (direction - 3)/2 + 0.5f);
                        }
                }
        }
        */

        public override int Reel(ItemController item)
        {
                item.Reset();
                gameController.Caught(1 );
                Destroy(gameObject);
                return 1;
        }


        public override int Release(ItemController item)
        {
                item.Reset();
                transform.rotation = Quaternion.identity;
                state = 1;
                return 1;
        }
}
