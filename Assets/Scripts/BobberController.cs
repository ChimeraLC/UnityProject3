using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobberController : MonoBehaviour
{
        private Vector2 destination;
        private Vector2 pathPosition;
        private Vector2 direction;
        private Vector2 velocity;

        private int state;

        private float totalDistance;
        // Start is called before the first frame update
        void Start()
        {
                Debug.Log("Bobber destination: " + destination);
                pathPosition = transform.position;
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
                            new Vector2(0, totalDistance / 4 * Mathf.Sin(Mathf.PI * velocity.magnitude / totalDistance));

                        // Landing in water.
                        // Check if velocity reaches 0
                        if (Vector2.Dot(velocity, direction) < 0) {
                                state = 1;
                                pathPosition = transform.position;
                        }

                }


        }

        public void SetDestination(Vector2 destination) {
                this.destination = destination;
                velocity = (destination - (Vector2)transform.position) * 1.5f;
                totalDistance = velocity.magnitude;
                direction = velocity.normalized;
        }
}
