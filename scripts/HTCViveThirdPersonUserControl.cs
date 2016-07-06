using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using VRTK;

    [RequireComponent(typeof(UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter))]
    public class HTCViveThirdPersonUserControl : MonoBehaviour
    {
        private UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        public  Transform m_HMDCamera;         // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                   // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;              // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

        [SerializeField]
        public bool foreceAllowController = true;

        [SerializeField]
        public Transform vrCameraRigg;
        public float maxAcceleration = 3f;
        public float jumpPower = 10f;

        private float acceleration = 0.05f;
        private float movementSpeed = 0f;
        private float rotationSpeed = 180f;
        private bool isJumping = false;
        private Vector2 touchAxis;
        private float triggerAxis;
        private Rigidbody rb;
        private Vector3 defaultPosition;
        private Quaternion defaultRotation;


        public void StartTouch() {
           // GameController.changePerspective();
        }

        public void EndTouch()
        {
            //GameController.changePerspective();
        }

        public void StartTouchPress()
        {
            GameController.changePerspective();
        }

        public void EndTouchPress()
        {
            GameController.changePerspective();
        }

        public void SetTouchAxis(Vector2 data)
        {
           touchAxis = data;
        }



        public void SetTriggerAxis(float data)
        {
            triggerAxis = data;
        }

        public void Reset()
        {

        }

        private void Start()
        {
            
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            
            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>();
        }


        private void Update()
        {
                Jump();
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            bool crouch = false;

            if ( (GameController.isFirstPerson || GameController.isCameraFading) && !foreceAllowController)
            {
                //Debug.Log("Controller Movement is dissabeled");
                //Stop all Caracter movements
                m_Character.Move(new Vector3(0,0,0), false, false);
                return;
            }
               

            // read inputs
            float h = touchAxis.x *  2.5f; // CrossPlatformInputManager.GetAxis("Horizontal");
            float v = touchAxis.y *  2.5f; // CrossPlatformInputManager.GetAxis("Vertical");
            

            if (m_HMDCamera.position.y < 1.5f) {
               // crouch = true;
            }

            
            // calculate move direction to pass to character
            if (!GameController.isCameraFading)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(GameController.current_CamForward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * (m_CamForward) + h * (GameController.current_CamRight);
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, isJumping);
        }


        private void Jump()
        {
            if (!isJumping && triggerAxis > 0)
            {
                isJumping = true;
                triggerAxis = 0f;
                //Debug.Log("jump");
            }
            else {
                isJumping = false;
            }
        }

        private void OnTriggerStay(Collider collider)
        {
            isJumping = false;
        }

        private void OnTriggerExit(Collider collider)
        {
            isJumping = true;
        }

    }