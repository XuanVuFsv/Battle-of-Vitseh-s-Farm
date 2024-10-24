using UnityEngine;

namespace VitsehLand.Scripts.Weapon.HandGun
{


    // Dave MovementLab - actionHandlerRope
    ///
    // Content:
    /// - drawing and animating the actionHandler rope
    /// 
    // Note:
    /// This script is assigned to the actionHandler gun
    ///
    // Credits: https://youtu.be/8nENcDnxeVE https://www.youtube.com/watch?v=TYzZsBl3OI0

    public class GrapplingRope : MonoBehaviour
    {
        [Header("References")]
        public ActionHandler actionHandler;

        [Header("Settings")]
        public int quality = 200; // how many segments the rope will be split up in
        public float damper = 14; // this slows the simulation down, so that not the entire rope is affected the same
        public float strength = 800; // how hard the simulation tries to get to the target point
        public float velocity = 15; // velocity of the animation
        public float waveCount = 3; // how many waves are being simulated
        public float waveHeight = 1;
        public AnimationCurve affectCurve;

        private SpringProperties spring; // a custom script that returns the values needed for the animation
        private LineRenderer lr;
        private Vector3 currentGrapplePosition;

        private void Awake()
        {
            // get references
            //actionHandler = GetComponent<ActionHandler>();
            lr = GetComponent<LineRenderer>();
            spring = new SpringProperties();
            spring.SetTarget(0);
        }

        //Called after Update
        private void LateUpdate()
        {
            DrawRope();
        }

        void DrawRope()
        {
            if (!actionHandler.HasShootingInputData()) return;

            // if not actionHandler, don't draw rope
            if (!actionHandler.IsActiveGrapple())
            {
                //if (actionHandler.shootingInputData.bulletSpawnPoint == null) return;

                currentGrapplePosition = actionHandler.shootingInputData.bulletSpawnPoint.position;

                // reset the simulation
                spring.Reset();

                // reset the positionCount of the lineRenderer
                if (lr.positionCount > 0)
                    lr.positionCount = 0;

                return;
            }

            //Debug.Log("Start Draw");

            if (lr.positionCount == 0)
            {
                // set the start velocity of the simulation
                spring.SetVelocity(velocity);

                // set the positionCount of the lineRenderer depending on the quality of the rope
                lr.positionCount = quality + 1;

                //Debug.Log("********************" + lr.positionCount);
            }

            // set the spring simulation
            spring.SetDamper(damper);
            spring.SetStrength(strength);
            spring.Update(Time.deltaTime);

            Vector3 grapplePoint = actionHandler.GetGrapplePoint();
            Vector3 gunTipPosition = actionHandler.shootingInputData.bulletSpawnPoint.position;

            // find the upwards direction relative to the rope
            Vector3 up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

            // lerp the currentGrapplePositin towards the grapplePoint
            currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

            //Debug.Log(lr.positionCount);
            // loop through all segments of the rope and animate them
            for (int i = 0; i < quality + 1; i++)
            {
                float delta = i / (float)quality;
                // calculate the offset of the current rope segment
                Vector3 offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value * affectCurve.Evaluate(delta);

                // lerp the lineRenderer position towards the currentGrapplePosition + the offset you just calculated
                lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
            }
        }
    }
}