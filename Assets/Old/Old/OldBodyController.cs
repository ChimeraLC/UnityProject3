using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldBodyController : MonoBehaviour
{
    private Rigidbody2D bodyRb;
    private SpriteRenderer bodySprite;
    // Start is called before the first frame update
    void Start()
    {
        bodyRb = GetComponent<Rigidbody2D>();
        bodySprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setVelocity(Vector2 v) {
        bodyRb.velocity = v;
    }

    public void setColor(bool state) {
        if (state)
            bodySprite.color = Color.blue;
        else
            bodySprite.color = Color.white;
    }
}
