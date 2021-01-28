using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/Actitons/PlayerSetTargetInput")]
    public class PlayerCharacterSetTargetInput : CharacterAction
    {
        public bool touchInput;
        public LayerMask layer;
        int layerMask = 1 << 8;
        public Camera mainCamera;

        public override void OnInitialize(CharacterThinker character)
        {
            mainCamera = Camera.main;
        
        }
        public override void OnFixedUpdate(CharacterThinker character)
        {
            
        }

        public override void OnUpdate(CharacterThinker character)
        {
            if (touchInput)
            {
                Touch[] myTouches = Input.touches;
                if (Input.touchCount == 1)
                {
                    //for (int i = 0; i < Input.touchCount; i++)
                    //{
                    if (myTouches[0].phase == TouchPhase.Stationary || myTouches[0].phase == TouchPhase.Moved)
                    {
                        Ray mouseRay = GenerateMouseRay();
                        RaycastHit hit;

                        if (Physics.Raycast(mouseRay.origin, mouseRay.direction,  out hit, 100, layer))
                        {
                            // GameObject temp = Instantiate(target, hit.point, Quaternion.identity);
                            Debug.DrawRay(mouseRay.origin, mouseRay.direction * hit.distance, Color.yellow);
                            character.target = hit.point;
                        }

                    }
                    //}
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    var inputDirection = new Vector3(character.player.GetAxis("LookHorizontal"), 0, character.player.GetAxis("LookVertical"));
                    inputDirection.Normalize();
                    inputDirection = Camera.main.transform.TransformDirection(inputDirection);
                    inputDirection.y = 0.0f;

                    var globalPosition = character.bpHolder.bodyPartsName["hip"].transform.position + inputDirection * 2;

                    globalPosition.y = 1.8f;

                    character.target = globalPosition;
                   // character.inputDirection = 
                }
                    
                //if (Input.GetMouseButton(0))
                //{
                //    Ray mouseRay = GenerateMouseRay();
                //    RaycastHit hit;

                //    if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit, 100, layer))
                //    {
                //        // GameObject temp = Instantiate(target, hit.point, Quaternion.identity);
                //        Debug.DrawRay(mouseRay.origin, mouseRay.direction * hit.distance, Color.yellow);
                //        Debug.Log("mouseRay.origin : " + mouseRay.origin);
                //        Debug.Log("hit.point : " + hit.point);
                //        Debug.Log("--------------");
                //        character.target = hit.point;
                //    }

                //}
            }
            
        }

        Ray GenerateMouseRay()
        {
            Vector3 mousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.farClipPlane);
            Vector3 mousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane);

            
            Vector3 mousePosFarW = mainCamera.ScreenToWorldPoint(mousePosFar);
            Vector3 mousePosNearW = mainCamera.ScreenToWorldPoint(mousePosNear);
            //mainCamera.viewport

            Debug.Log("mainCamera.name : " + mainCamera.name);
            Debug.Log("mainCamera.farClipPlane : " + mainCamera.farClipPlane);
            Debug.Log("mainCamera.nearClipPlane : " + mainCamera.nearClipPlane);

            

            Debug.Log("mousePosFar : " + mousePosFar);
            Debug.Log("mousePosNear : " + mousePosNear);
            Debug.Log("mousePosFarW : " + mousePosFarW);
            Debug.Log("mousePosNearW : " + mousePosNearW);
            
            Debug.Log("mousePosNear : " + mousePosNear);
            Debug.Log("mousePosNearW : " + mousePosNearW);
            Ray mouseRay = new Ray(mousePosNearW, mousePosFarW - mousePosNearW);

            Debug.Log("mouseRay.origin : " + mouseRay.origin);

            Debug.Log("--------------");
            return mouseRay;



        }
    }
}
