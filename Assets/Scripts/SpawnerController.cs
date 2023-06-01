using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{

        public GameObject fish;
        public GameController gameController;
        private BobberController bob;

        private float lifetime;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
                lifetime += Time.deltaTime;
                if (lifetime > 3) {
                        lifetime -= 3;
                        SpawnEnemy();
                }
        }
        // Spawn a fish?
        void SpawnEnemy() {
                // Creating fish
                fish = Instantiate(fish, new Vector2(-12, Random.Range(-3, 3)), Quaternion.identity);
                fish.transform.SetParent(transform);
                // Setting inital stuff
                fish.GetComponent<FishParentController>().SetDirection(1);
                fish.GetComponent<FishParentController>().gameController = gameController;
        }
}
