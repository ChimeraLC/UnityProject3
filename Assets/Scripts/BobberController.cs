using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BobberController : MonoBehaviour
{
        private Vector2 destination;
        public Vector2 pathPosition;
        private Vector2 direction;
        private Vector2 velocity;

        private int state = 0; //0 - thrown, 1 - sitting, 2 - caught
        private float lifetime;
        //boat bounds
        public float[] bounds;

        public GameController gameController;
        public ItemController parentRod;

        private FishParentController clampedFish;

        private BoxCollider2D col;

        private float totalDistance;
        // Start is called before the first frame update
        void Start()
        {
                //Debug.Log("Bobber destination: " + destination);
                pathPosition = transform.position;

                col = GetComponent<BoxCollider2D>();
                col.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
                
                // Once destination set
                if (destination != null && state == 0) {
                        // Simulated slowdown.
                        pathPosition += velocity * Time.deltaTime;
                        velocity -= 6 * direction * Time.deltaTime;

                        // Additional upward stuff.
                        transform.position = pathPosition +
                            new Vector2(0, totalDistance / 2 * Mathf.Sin(Mathf.PI * velocity.magnitude / totalDistance));

                        // Landing in water.
                        // Check if velocity reaches 0
                        if (Vector2.Dot(velocity, direction) < 0) {
                                state = 1;
                                pathPosition = transform.position;
                                // Enable collider
                                col.enabled = true;

                                // checking bounds
                                if (transform.position.x < bounds[0] && transform.position.x > bounds[1] &&
                                    transform.position.y < bounds[2] && transform.position.y > bounds[3]) {
                                        Destroy(gameObject);
                                        parentRod.Reset();
                                }
                        }

                }

                // In the water.
                else {
                        lifetime += Time.deltaTime;

                        if (lifetime > 2) {
                                lifetime -= 2;
                        }

                        // Bobbing animation
                        if (state == 1) {
                                transform.position = pathPosition - new Vector2(0, 0.2f * Mathf.Sin(lifetime * Mathf.PI));
                        }
                        if (state == 2) {
                                transform.position = pathPosition - new Vector2(0.1f * Mathf.Sin(lifetime * 3 * Mathf.PI)
                                    , 0.2f * Mathf.Sin(lifetime * Mathf.PI));
                        }
                }


        }

        public void SetDestination(Vector2 destination) {
                this.destination = destination;
                velocity = (destination - (Vector2)transform.position) * 1.5f;
                totalDistance = velocity.magnitude;
                direction = velocity.normalized;
        }

        public int Pull() {
                if (state == 2) {
                        /* Pulling fish
                        if (clampedFish != null) {
                                clampedFish.Reel();
                        }
                        */
                        return 1;
                }
                return 0;
        }

        // Probability of catching fish
        void FishCheck()
        {
                if (state == 1 && UnityEngine.Random.Range(0, 5) > 1)
                {
                        state = 2;
                }
                else {
                        state = 1;
                }
        }
        // setters and getters
        public int GetState() {
                return state;        
        }

        public void SetState(int st) {
                state = st;
        }

        // Set caught fish
        public void SetFish(FishParentController fish) {
                clampedFish = fish;
                parentRod.clampedFish = fish;
        }
}
