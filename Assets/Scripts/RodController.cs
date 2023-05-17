using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RodController : MonoBehaviour
{
        public GameObject bobber;
        private GameObject curBobber;
        private LineRenderer fishingLine;
        private int castState = 0;
        private float castAngle;
        // Start is called before the first frame update
        void Start()
        {
                castAngle = 0;
                fishingLine = GetComponent<LineRenderer>();
                fishingLine.startWidth = 0.02f;
                fishingLine.endWidth = 0.02f;
        }

        // Update is called once per frame
        void Update()
        {
                // Charging up cast.
                if (castState == 1) {
                        castAngle += 45 * Time.deltaTime;

                        // Releasing cast.
                        if (Input.GetMouseButtonUp(0)) {
                                // Minimum chargeup.
                                if (castAngle > 30)
                                {
                                        castState = 2;
                                        // Mouse position.
                                        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                                        // Create bobber
                                        curBobber = Instantiate(bobber, transform.parent.position, Quaternion.identity);
                                        curBobber.GetComponent<BobberController>().SetDestination(
                                            (mousePosition - (Vector2)transform.parent.position).normalized * castAngle / 15
                                            + (Vector2)transform.parent.position);
                                }
                                else {
                                        castState = 0;
                                }
                        }
                }

                // Release
                if (castState == 2) {
                        fishingLine.enabled = true;
                        castAngle = Mathf.Lerp(castAngle, 0, 0.1f);
                        fishingLine.SetPosition(0, transform.position + new Vector3(0.75f, 0.75f));
                        fishingLine.SetPosition(1, curBobber.transform.position);
                }

                // Reeling in
                if (castState == -1) {
                        castState = 0;
                        fishingLine.enabled = false;
                        Destroy(curBobber);
                }

                transform.rotation = Quaternion.Euler(Vector3.forward * castAngle);
        }
        // Cast fishing rod.
        public void Cast() {
                // Starting up cast.
                if (castState == 0) {
                        // TODO: make this more efficient.
                        castAngle = Mathf.Lerp(castAngle, 0, 0.1f);
                        castState = 1;
                }

                // Reeling back in
                if (castState == 2) {
                        castState = -1;
                }

        }
}
