using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Fish2Controller : FishParentController
{
        private int direction = -1;
        private int state = 0; //0, swimming, 1, clamped, 2, reeling;

        private float degrees = 0;
        private float degreesTarget = 15;
        private float degreesOriginal;
        private Animator fishAnim;
        // Start is called before the first frame update

        // Fishing progress bar
        public GameObject progressBarPrefab;
        private ItemController itemController;
        private ProgressBarControlledController progressBar;
        private float pullTime = 0;
        private float maxPullTime = 2;
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
                if (state >= 1) { 
                        transform.rotation = Quaternion.Euler(Vector3.forward * degrees);
                        transform.position = bob.transform.position + new Vector3(-Mathf.Cos(Mathf.Deg2Rad * degrees) * 1,
                            -Mathf.Sin(Mathf.Deg2Rad * degrees) * 1 - 0.2f, 0);

                        if (degreesTarget > degrees)
                        {
                                degrees += Time.deltaTime * 80;
                                if (degrees >= degreesTarget) degreesTarget *= -1;
                        }
                        else {
                                degrees -= Time.deltaTime * 80;
                                if (degrees < degreesTarget) {
                                        degreesTarget *= -1;
                                }
                        }
                        

                        if (state == 2) {
                                if (Input.GetMouseButton(0))
                                {
                                        pullTime += Time.deltaTime;
                                        if (pullTime > maxPullTime)
                                        {
                                                itemController.Reset();
                                                gameController.Caught(1);
                                                Destroy(progressBar.gameObject);
                                                Destroy(gameObject);
                                        }
                                }
                                else {
                                        pullTime = Mathf.Max(pullTime - Time.deltaTime / 2, 0);
                                }
                                // Set progress bar
                                progressBar.SetRatio(pullTime / maxPullTime);
                        }
                }

                // rough boundary checker
                if (Mathf.Abs(transform.position.x) > 20 || Mathf.Abs(transform.position.y) > 20)
                {
                        Destroy(gameObject);
                }
        }

        public override void Hook(GameObject bobber) {
                bob = bobber.GetComponent<BobberController>();
                if (bob.GetState() == 1)
                {
                        bob.SetState(2);
                        bob.SetFish(this);
                        itemController = bob.parentRod;
                        state = 1;

                        // Saving animation state
                        degreesOriginal = gameObject.transform.eulerAngles.x;

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

                // Setting collision size
                GetComponent<BoxCollider2D>().size = new Vector2(1.5f/(1 + (dir + 1) % 2), 1.5f/(1 + dir % 2));
        }

        // Old code
        /*
        public void OnTriggerEnter2D(Collider2D collision)
        {
                Debug.Log("hit");
                if (collision.CompareTag("Bobber"))
                {
                        bob = collision.GetComponent<BobberController>();
                        if (bob.GetState() == 1)
                        {
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
                state = 2;

                // Create progress bar
                progressBar = Instantiate(progressBarPrefab, bob.pathPosition + new Vector2(1, 0), Quaternion.identity)
                    .GetComponent<ProgressBarControlledController>();
                return 1;
        }

        public override int Release(ItemController item)
        {
                // Reset rod
                item.Reset();

                // Return to swimming state
                transform.rotation = Quaternion.identity;
                state = 0;
                if (progressBar != null) {
                        Destroy(progressBar.gameObject);
                }
                //TODO: reset animation
                return 1;
        }
}
