using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bodyObject;


    private int bodyCount = 2;
    private List<Rigidbody2D> bodyRbs = new List<Rigidbody2D>();
    private int bodyCurrent = 0;
    private float speed = 6f;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < bodyCount; i++) {
            GameObject newBody = Instantiate(bodyObject, new Vector2(0, 0), Quaternion.identity);
            bodyRbs.Add(newBody.GetComponent<Rigidbody2D>());
        }
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
        float mouseAngle = Mathf.Atan2(mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y) * Mathf.Rad2Deg;

        /*
        * movement control
        */
        Vector2 moveDir = Vector2.zero;
        if (Input.GetKey(KeyCode.A)) moveDir.x -= 1;
        if (Input.GetKey(KeyCode.D)) moveDir.x += 1;
        if (Input.GetKey(KeyCode.W)) moveDir.y += 1;
        if (Input.GetKey(KeyCode.S)) moveDir.y -= 1;

        moveDir.Normalize();
        bodyRbs[bodyCurrent].velocity = speed * moveDir;

        // Swapping body
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Stop the current body.
            bodyRbs[bodyCurrent].velocity = Vector2.zero;
            // Change body.
            bodyCurrent = (bodyCurrent + 1) % bodyCount;
        }
    }
}
