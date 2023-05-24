using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{

        public GameObject fish;

        private float lifetime;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
                lifetime += Time.deltaTime;
                if (lifetime > 2) {
                        lifetime -= 2;
                        SpawnEnemy();
                }
        }
        // Spawn a fish?
        void SpawnEnemy() {
                Instantiate(fish, new Vector2(-12, Random.Range(-3, 3)), Quaternion.identity)
                    .GetComponent<FishParentController>().SetDirection(1);
        }
}
