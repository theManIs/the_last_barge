using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Ships
{
    public class NavalMothership : NavyBrig
    {
        public Transform InterceptorTransform;
        public int InterceptorsRate = 10;
        public Transform[] SpawnPosition;
        public float SpawnDelay = 2;

        protected FrameLocker InterceptorsDebounce = new FrameLocker();
        protected FrameLocker InterceptorsBetween = new FrameLocker();

        protected new void Start()
        {
            base.Start();

            TmpDamage = 2;
            InterceptorsDebounce.LockSeconds = InterceptorsRate;
            InterceptorsBetween.LockSeconds = SpawnDelay;

            InterceptorsDebounce.StartCountdown();

        }

        private bool _lockSpawn = false;
        private Transform _lastBoat;

        protected new void Update()
        {
            base.Update();


            if (InterceptorsDebounce.CheckTime())
            {
                if (InterceptorsBetween.CheckTime() && SpawnPosition.Length > 0)
                {
                    Animator a = GetComponentInChildren<Animator>();

                    a.SetTrigger("unload");

                    SpawnInterceptor();

                    InterceptorsBetween.StartCountdown();
                }
            }

            InterceptorsBetween.Countdown();
            InterceptorsDebounce.Countdown();
        }

        protected void SpawnInterceptor()
        {
            if (InterceptorTransform)
            {
                Transform boat = SpawnPosition.ToList().First();
                Transform obj = Instantiate(InterceptorTransform, transform);

                obj.transform.position = boat.position;
                obj.transform.rotation = boat.rotation;
//                obj.transform.position += (RandomBool() ? transform.right : -transform.right) * 20;

                StartCoroutine(LookAfterShip(obj));

                _lastBoat = obj;

                Transform[] localSpawnPosition = new Transform[SpawnPosition.Length - 1];

                for (int i = 1; i < SpawnPosition.Length; i++)
                {
                    localSpawnPosition[i - 1] = SpawnPosition[i];
                }

                SpawnPosition = localSpawnPosition;
            }
        }

        public IEnumerator LookAfterShip(Transform boat)
        {
            yield return new WaitForSeconds(10);

            if (boat)
            {
                boat.LookAt(Vector3.zero);
            }
        }

        protected bool RandomBool()
        {
            return Random.value > 0.5f;
        }
    }
}