using UnityEngine;

namespace Assets.Scripts.Ships
{
    public abstract class NavalNavigation : MonoBehaviour
    {
        [Header("NavalNavigation")]
        public float AngleVelocity = 1f;
        public float YawAngle = 15;
        public float AccelerationStep = 20;
        public float EngineMaxRpm = 30000;

        protected float EngineRpm = 0;
        protected float AngleSpeed = 0;
        protected float Throttle = 0;
        protected Rigidbody Rb;

        protected bool PlotCourse()
        {
            BoxCollider bc = GetComponent<BoxCollider>();
            int id = transform.GetInstanceID();
            LayerMask lm = LayerMask.GetMask("Projectile");

            RaycastHit[] inFrontOf = Physics.BoxCastAll(transform.position, bc.bounds.extents / 2,  transform.forward, transform.rotation, bc.bounds.size.x * 3);

            if (inFrontOf.Length > 0)
            {
                foreach (RaycastHit heathen in inFrontOf)
                {
                    if (heathen.transform.gameObject.layer != LayerMask.NameToLayer("Projectile") && heathen.transform.GetInstanceID() != id)
                    {
//                        Debug.Log(bc.bounds.size.magnitude);
//                        Debug.Log(heathen.transform.gameObject.name + " " + heathen.transform.gameObject.GetInstanceID() + " " + Vector3.Distance(transform.position, heathen.transform.position));

                        return true;
                    }
                }
            }

            return false;
        }

        protected void ChangeCourse()
        {
            if (PlotCourse())
            {
                if (Mathf.Abs(AngleSpeed) < YawAngle)
                {
                    RudderRight();
                }
            }
        }

        protected Rigidbody GetRigidBody()
        {
            if (Rb)
            {
                return Rb;
            } 
            
            if (!(Rb = GetComponent<Rigidbody>()))
            {
                Rb = gameObject.AddComponent<Rigidbody>();
            }

            return Rb;
        }

        protected void AddTorque()
        {
            EngineRpm = Throttle * EngineMaxRpm;
            Vector3 speedVector = Quaternion.Euler(0, AngleSpeed, 0) * transform.forward * EngineRpm;
            GetRigidBody().AddForce(speedVector);
        }

        public void AngleDamping()
        {
            AngleSpeed = Mathf.Lerp(AngleSpeed, 0.0F, 0.02F);
        }

        public void RudderRight()
        {
            AngleSpeed -= AngleVelocity;
            AngleSpeed = Mathf.Clamp(AngleSpeed, -1f * YawAngle, YawAngle);
        }

        public void RudderLeft()
        {
            AngleSpeed += AngleVelocity;
            AngleSpeed = Mathf.Clamp(AngleSpeed, -1f * YawAngle, YawAngle);
        }

        public void ThrottleUp()
        {
            Throttle += AccelerationStep * 0.001F;
            Throttle = Mathf.Clamp(Throttle, 0f, 1f);
        }

        public void ThrottleDown()
        {
            Throttle -= AccelerationStep * 0.001F;
            Throttle = Mathf.Clamp(Throttle, -1, 0);
        }

        public void Brake()
        {
            if (Throttle > 0)
            {
                Throttle -= AccelerationStep * 0.001F;
            }
            else
            {
                Throttle += AccelerationStep * 0.001F;
            }
        }
    }
}