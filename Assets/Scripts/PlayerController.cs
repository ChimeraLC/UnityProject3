using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

        // Body control
        public GameObject bodyObject;
        private int bodyCount = 1;
        private List<BodyController> bodyRbs = new List<BodyController>();
        private int bodyCurrent = 0;
        private float speed = 6f;
        public Vector2 currentPos = Vector2.zero;

        // Weapon control
        public GameObject weaponObject;
        public GameObject weaponHitboxObject;
        const float weaponBaseAngle = 70;
        private float weaponAngle = weaponBaseAngle;
        private float weaponCoef = 1;
    
        private float mouseAngle;

        // Start is called before the first frame update
        void Start()
        {
                // Create initial bodies.

                // Manually instantiate the color of the first boject.
                GameObject newBody = Instantiate(bodyObject, Vector2.zero, Quaternion.identity);
                bodyRbs.Add(newBody.GetComponent<BodyController>());
                newBody.GetComponent<SpriteRenderer>().color = Color.blue;
                // Create the rest
                for (int i = 1; i < bodyCount; i++) {
                        newBody = Instantiate(bodyObject, new Vector2(0, 0), Quaternion.identity);
                        bodyRbs.Add(newBody.GetComponent<BodyController>());
                }

                // Creating weapon
                weaponObject = Instantiate(weaponObject, Vector2.zero, Quaternion.identity);
        }

        // Update is called once per frame
        void Update()
        {

                /*
                * inputs
                */

                //keyboard

                //mouse position
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseAngle = Mathf.Atan2(mousePosition.x - bodyRbs[bodyCurrent].transform.position.x,
                        mousePosition.y - bodyRbs[bodyCurrent].transform.position.y) * Mathf.Rad2Deg;

                /*
                * movement control
                */
                Vector2 moveDir = Vector2.zero;
                if (Input.GetKey(KeyCode.A)) moveDir.x -= 1;
                if (Input.GetKey(KeyCode.D)) moveDir.x += 1;
                if (Input.GetKey(KeyCode.W)) moveDir.y += 1;
                if (Input.GetKey(KeyCode.S)) moveDir.y -= 1;

                moveDir.Normalize();
                bodyRbs[bodyCurrent].setVelocity(speed * moveDir);
                currentPos = bodyRbs[bodyCurrent].transform.position;

                // Swapping body
                if (Input.GetKeyDown(KeyCode.Space))
                {
                        // Stop the current body.
                        bodyRbs[bodyCurrent].setVelocity(Vector2.zero);
                        bodyRbs[bodyCurrent].setColor(false);
                        // Change body.
                        bodyCurrent = (bodyCurrent + 1) % bodyCount;
                        bodyRbs[bodyCurrent].setColor(true);

                }

                // Creation of new bodies (incomplete)
                if (Input.GetKeyDown(KeyCode.N)) {
                        bodyCount++;
                        GameObject newBody = Instantiate(bodyObject, Vector2.zero, Quaternion.identity);
                        bodyRbs.Add(newBody.GetComponent<BodyController>());
                }

                // Swinging melee weapon
                if (Input.GetMouseButtonDown(0)) {
                        // Check that weapon is not currently being swung       
                        if (weaponAngle == weaponBaseAngle) {
                                weaponAngle+= 50 * Time.deltaTime;
                        }
                }
                SwingFunction();
        }

        private void SwingFunction()
        {
                // Initial preswing
                if (weaponAngle > weaponBaseAngle) {
                        weaponAngle += 50 * Time.deltaTime;
                        if (weaponAngle > 110)
                        {
                                weaponAngle = -50;
                                weaponCoef *= -1;
                                // Spawn hitbox
                                Instantiate(weaponHitboxObject, (Vector2) bodyRbs[bodyCurrent].transform.position + 
                                    new Vector2(Mathf.Sin(mouseAngle * Mathf.Deg2Rad), Mathf.Cos(mouseAngle * Mathf.Deg2Rad)),
                                    Quaternion.Euler(Vector3.forward * (-mouseAngle + 45 + weaponAngle * weaponCoef)));
                        }
                }
                else {
                        weaponAngle = Mathf.Min(weaponBaseAngle, weaponAngle + 1440 * Time.deltaTime);
                }
        }

        private void LateUpdate()
        {
                // Update weapon position
                weaponObject.transform.position = bodyRbs[bodyCurrent].transform.position;
                weaponObject.transform.rotation = 
                    Quaternion.Euler(Vector3.forward * (-mouseAngle + 45 + weaponAngle * weaponCoef));
        }
}
