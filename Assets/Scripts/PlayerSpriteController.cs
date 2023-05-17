using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
        private SpriteRenderer playerSR;
        public Sprite[] animSprites;
        private float animationTimer = 0;
        private float animationSpeed = 1.5f;

        // Fishing rod?
        private GameObject fishingRod;
        // Start is called before the first frame update
        void Start()
        {
                playerSR = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
        }
        public void UpdateAnimation(float mouseAngle, bool moving)
        {
                //direction looking
                playerSR.sprite = animSprites[(int)Math.Floor((mouseAngle + 202.5) / 45)];
                //movement animation
                if (moving)
                {
                        animationTimer += 360 * animationSpeed * Time.deltaTime;
                        if (animationTimer >= 360) animationTimer -= 360;
                }
                else
                {
                        //smoothing animation
                        //TODO: force at least one final hop (or none)
                        animationTimer = Math.Min(animationTimer + 360 * animationSpeed * Time.deltaTime, animationTimer - animationTimer % 180 + 179);
                }
                //bouncing
                transform.rotation = Quaternion.AngleAxis(10 * (float)Math.Sin(animationTimer * Math.PI / 180), Vector3.forward);
                transform.localPosition = fishingRod.transform.localPosition = 
                    new Vector2(0, 0.5f * (float)Math.Abs(Math.Sin(animationTimer * Math.PI / 180)));
        }

        // Update alpha of sprite
        public void SetAlpha(int alphaValue)
        {
                Color temp = playerSR.color;
                temp.a = alphaValue;
                playerSR.color = temp;
        }

        public void SetRod(GameObject rod)
        {
                fishingRod = rod;
        }
}