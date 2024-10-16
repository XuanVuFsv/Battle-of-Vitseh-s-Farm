using UnityEngine;
using VitsehLand.Scripts.Audio;
using VitsehLand.Scripts.Pattern.Singleton;

namespace VitsehLand.Scripts.Player
{
    public class InputController : Singleton<InputController>
    {
        private static InputController instance;

        public Vector2 move;
        public Vector2 look;

        public bool isIdle;
        public bool isWalk, isWalkRight, isWalkLeft, isWalkBackward, isWalkForward;
        public bool isAim;
        public bool isHoldAim;
        public bool isFire, isSingleFire, isStopFire;
        public bool isReload;
        public bool isManipulationFire;
        public bool isJump, isBrake;

        public float horizontal, vertical;
        public float rawHorizontal, rawVertical;
        public float mouseX, mouseY;
        public float fireValue = 0;

        //void MakeInstance()
        //{
        //    if (instance != null && instance != this)
        //    {
        //        Destroy(this.gameObject);
        //    }
        //    else instance = this;
        //}

        //public static InputController Instance
        //{
        //    get
        //    {
        //        return instance;
        //    }
        //}

        //private void Awake()
        //{
        //    //MakeInstance();
        //}

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (move.x != 0 || move.y != 0) AudioBuildingManager.Instance.walk.enabled = true;
            else AudioBuildingManager.Instance.walk.enabled = false;

            UpdateInputValue();
            UpdateFireValue();
            UpdateAnimationValue();
        }

        void UpdateInputValue()
        {
            move.x = horizontal = Input.GetAxis("Horizontal");
            move.y = vertical = Input.GetAxis("Vertical");
            rawHorizontal = Input.GetAxisRaw("Horizontal");
            rawVertical = Input.GetAxisRaw("Vertical");
            look.x = mouseX = Input.GetAxis("Mouse X");
            look.y = mouseY = Input.GetAxis("Mouse Y");

            isJump = Input.GetKeyDown(KeyCode.Space);
            isBrake = Input.GetKey(KeyCode.G);
            isReload = Input.GetKeyDown(KeyCode.R);
            isAim = Input.GetMouseButtonDown(1);
            isHoldAim = Input.GetMouseButton(1);
        }

        void UpdateFireValue()
        {
            if (fireValue == 0) isFire = false;

            if (Input.GetMouseButtonUp(0))
            {
                isStopFire = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                isSingleFire = true;
                //Debug.Log("Down");
            }
            else isSingleFire = false;

            if (Input.GetMouseButton(0))
            {
                isFire = true;
                isStopFire = false;
                fireValue += 0.25f;
            }
            else
            {
                fireValue -= 0.25f;
            }

            fireValue = Mathf.Clamp(fireValue, 0, 2);
        }

        void UpdateAnimationValue()
        {
            UpdateMovementAnimationValue();
            UpdateFireAnimationValue();
        }

        void UpdateMovementAnimationValue()
        {
            if (horizontal != 0 || vertical != 0)
            {
                isIdle = false;
                isWalk = true;
            }
            else
            {
                isIdle = true;
                isWalk = false;
            }
        }

        void UpdateFireAnimationValue()
        {
            if (isFire || isAim)
            {
                isManipulationFire = true;
            }
            else isManipulationFire = false;
        }
    }
}