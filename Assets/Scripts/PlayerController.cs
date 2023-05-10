using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Body control
    public GameObject bodyObject;
    private int bodyCount = 2;
    private List<BodyController> bodyRbs = new List<BodyController>();
    private int bodyCurrent = 0;
    private float speed = 6f;

    // Weapon control
    public GameObject weaponObject;
    private float weaponAngle = 50;
    
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
        if (Input.GetMouseButtonDown(0)) {
            bodyCount++;
            GameObject newBody = Instantiate(bodyObject, new Vector2(0, 0), Quaternion.identity);
            bodyRbs.Add(newBody.GetComponent<BodyController>());
        }
    }

    private void LateUpdate()
    {
        // Update weapon position
        weaponObject.transform.position = bodyRbs[bodyCurrent].transform.position;
        weaponObject.transform.rotation = Quaternion.Euler(Vector3.forward * (-mouseAngle + 45 + weaponAngle));
    }
}
