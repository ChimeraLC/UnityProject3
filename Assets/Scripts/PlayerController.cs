using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
        public GameObject fishingRod;
        private RodController rodController;
        // Start is called before the first frame update
        void Start()
        {
                GameObject rod = Instantiate(fishingRod, transform.position, Quaternion.identity);
                rod.transform.parent = transform;
                rodController = rod.GetComponent<RodController>();
        }

        // Update is called once per frame
        void Update()
        {
                if (Input.GetMouseButtonDown(0)) {
                        rodController.Cast();
                }
        }
}
