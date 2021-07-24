using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.GameEvents;
using Game.Commands;
using Game.SpaceObject;
using UnityEngine;

namespace Game.Systems
{
    [Serializable]
    public class Boundary
    {
        public float xMin, xMax, yMin, yMax;
    }

    /// <summary>
    /// The main functionality of the player
    /// </summary>
    public class Playercontroller : MonoBehaviour
    {
        #region [Fields]

        private readonly float playerRotationSpeed = 200;
        public float playerSpeed = 4;
        private GameObject lastScanned;
        public Boundary boundary;

        private GameEventManager eventManager;
        public float fireRate = 0.5f;
        private GameData gameData;
        public List<GameObject> CargoBay;

        //private float gameSpeed = 5f;
        private bool isDisabled;

        private bool isIdle = true;
        private float nextFire;

        public GameObject shot;
        public Transform shotSpawn;

        private float unitSize = 1;

        #endregion

        public void Awake()
        {
            this.gameData = GameData.Instance;
            this.isDisabled = false;
        }

        public void Start()
        {
            eventManager = GameEventManager.Instance;
            eventManager.Subscribe(GameEventType.ChallangeCompleted, this.OnChallengeCompleted);
            eventManager.Subscribe(GameEventType.ChallangeStarted, (value) => { isDisabled = false; });
        }

        private void Update()
        {
            ReadInput();
        }

        private void OnChallengeCompleted(int value)
        {
            isDisabled = true;
        }

        private void ReadInput()
        {
            if (isDisabled)
            {
                return;
            }

            if (Input.GetButton("Fire1"))
            {
                //FireWeapon();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                //test sequence
                //MoveForward();
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                //RotateLeftAsync();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                //RotateRightAsync();
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                //var result = await this.ScanAhead();
                //Debug.Log(result);
            }
        }

        public void Die()
        {
            isIdle = true;
            gameObject.SetActive(false);
        }

        #region [API]

        /// <summary>
        /// Fires the player weapon once
        /// </summary>
        public async UniTask<string> FireWeaponAsync()
        {
            if (Time.time > this.nextFire)
            {
                this.nextFire = Time.time + fireRate;
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

                //GetComponent<AudioSource>().Play();
            }

            return "shot fired";
        }

        /// <summary>
        /// Scans for objects 1 unit ahead of the player
        /// </summary>
        /// <returns>The the space object type</returns>
        public async UniTask<string> ScanAheadAsync()
        {
            this.isIdle = false;
            await Task.Delay(3000);

            //TODO play scan animation
            Debug.DrawRay(this.transform.position, transform.TransformDirection(Vector3.forward) * 2, Color.yellow, 2,
                false);
            if (Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.forward),
                out var hitResult, 2))
            {
                Debug.Log(hitResult.collider.gameObject.name);
                this.lastScanned = hitResult.transform.gameObject;

                return this.lastScanned.GetComponent<ISpaceObject>().SpaceObjectType.ToString();
            }

            this.isIdle = true;
            return "Scanner found no object";
        }


        /// <summary>
        /// Moves the player forward by given units distance
        /// </summary>
        /// <param name="dist">The distance to move, defaults to 1</param>
        public async UniTask<string> MoveForwardAsync(int dist = 1)
        {
            this.isIdle = false;
            var endPosition = this.transform.position + this.transform.forward * this.gameData.GridSize * dist;
            endPosition = CheckBoundaries(endPosition);

            while (this.transform.position != endPosition)
            {
                this.transform.position =
                    Vector3.MoveTowards(transform.position, endPosition, this.playerSpeed * Time.deltaTime);
                await UniTask.WaitForEndOfFrame();
            }

            isIdle = true;
            return "finished moving";
        }

        /// <summary>
        /// Rotates the player ccw by a given amount of degrees
        /// </summary>
        /// <param name="degrees">The amount of the rotation in degrees, defaults to 90</param>
        public async UniTask<string> RotateLeftAsync(float degrees = 90)
        {
            var args = new CommandArgs() {Degrees = -degrees, Speed = this.playerRotationSpeed};
            return await RotateOverSpeedAsync(args);
        }

        /// <summary>
        /// Rotates the player cw by a given amount of degrees
        /// </summary>
        /// <param name="degrees">The amount of the rotation in degrees, defaults to 90</param>
        public async UniTask<string> RotateRightAsync(float degrees = 90)
        {
            var args = new CommandArgs {Degrees = degrees, Speed = this.playerRotationSpeed};
            return await this.RotateOverSpeedAsync(args);
        }

        #endregion

        private Vector3 CheckBoundaries(Vector3 endPosition)
        {
            if (endPosition.x > boundary.xMax) endPosition.x = boundary.xMax;
            if (endPosition.x < boundary.xMin) endPosition.x = boundary.xMin;
            if (endPosition.y < boundary.yMin) endPosition.y = boundary.yMin;
            if (endPosition.y > boundary.yMax) endPosition.y = boundary.yMax;
            return endPosition;
        }

        private async UniTask<string> RotateOverSpeedAsync(object args)
        {
            var end = Quaternion.Euler(0, ((CommandArgs) args).Degrees, 0);
            isIdle = false;
            var endRotation = this.transform.rotation * end;
            while (this.transform.rotation != endRotation)
            {
                this.transform.rotation =
                    Quaternion.RotateTowards(this.transform.rotation, endRotation,
                        ((CommandArgs) args).Speed * Time.deltaTime);
                await UniTask.WaitForEndOfFrame();
            }

            isIdle = true;
            return "rotated";
        }
    }
}