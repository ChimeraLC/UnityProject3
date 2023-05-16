using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobberController : MonoBehaviour
{
        private Vector2 destination;
        private Vector2 pathPosition;

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
                if (destination != null) {
                        pathPosition = Vector2.Lerp(pathPosition, destination, 0.005f);
                }

                transform.position = pathPosition + 
                        new Vector2 (0, 3 * Mathf.Sin(Mathf.PI * (destination - pathPosition).magnitude / totalDistance));
        }

        public void SetDestination(Vector2 destination) {
                this.destination = destination;
                totalDistance = (destination - (Vector2) transform.position).magnitude;
        }
}
