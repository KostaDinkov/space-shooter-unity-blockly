using System;
using System.Collections;
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
    /// The main functionallity of the player
    /// </summary>
    public class Playercontroller : MonoBehaviour
    {
        private readonly float playerRotationSpeed = 200;
        public float playerSpeed = 4;
        private GameObject lastScanned;
        public Boundary boundary;
        private readonly CommandQueue commandQueue = new CommandQueue();
        private GameEventManager eventManager;
        public float fireRate = 0.5f;
        private GameData gameData;
        public List<GameObject> CargoBay;

        //private float gameSpeed = 5f;
        private bool isDisabled;

        private bool isIdle = true;


        private float nextfire;

        public GameObject shot;
        public Transform shotSpawn;

        private float unitSize = 1;

        public void Awake()
        {
            this.gameData = GameData.Instance;

            this.isDisabled = false;
        }

        public void Start()
        {
            eventManager = GameEventManager.Instance;
            eventManager.Subscribe(GameEventType.ChallangeCompleted, OnChallangeCompleted);
            eventManager.Subscribe(GameEventType.ChallangeStarted, (value) => { isDisabled = false; });
        }

        private void Update()
        {
            //execute the next command in the command queue
            if (isIdle && !commandQueue.IsEmpty())
            {
                commandQueue.Execute();
            }

            var particleSystems = GetComponentsInChildren<ParticleSystem>();
            foreach (var psys in particleSystems)
            {
                var newMain = psys.main;
                newMain.startRotation = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
            }

            ReadInput();
        }

        private void OnChallangeCompleted(int value)
        {
            isDisabled = true;
            commandQueue.Clear();
        }

        private async void ReadInput()
        {
            if (isDisabled)
            {
                return;
            }

            if (Input.GetButton("Fire1"))
            {
                FireWeapon();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                //test sequence
                MoveForward();
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                //RotateLeft();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                //RotateRight();
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                var result = await this.ScanAhead();
                Debug.Log(result);
            }
        }

        public void Die()
        {
            isIdle = true;
            commandQueue.Clear();
            gameObject.SetActive(false);
        }

        #region [Coroutines]

        internal async Task<string> FireWeaponAsync()
        {
            if (Time.time > nextfire)
            {
                nextfire = Time.time + fireRate;
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                GetComponent<AudioSource>().Play();
            }

            await Task.Delay(1000);
            return "shot fired";
        }

        internal async Task<string> ScanAheadAsync()
        {
            this.isIdle = false;
            await Task.Delay(3000);
            RaycastHit hitResult;
            this.StartCoroutine(ScanAheadCoroutine(new CommandArgs()));
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2, Color.yellow, 2, false);
            if (Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.forward), out hitResult, 2))
            {

                Debug.Log(hitResult.collider.gameObject.name);
                this.lastScanned = hitResult.transform.gameObject;
                
                return this.lastScanned.GetComponent<ISpaceObject>().SpaceObjectType.ToString();
            }

            this.isIdle = true;
            return "Scanner found no object";


        }

        internal IEnumerator ScanAheadCoroutine(ICommandArgs args)
        {
            //play scanning animation
            yield return new WaitForSeconds(1);
            this.isIdle = true;
        }

        internal async Task<string> RotateOverSpeedAsync(object args)
        {

            var end = Quaternion.Euler(0, ((CommandArgs)args).Degrees, 0);
            isIdle = false;
            var endRotation = this.transform.rotation * end;
            while (this.transform.rotation != endRotation)
            {
                this.transform.rotation =
                    Quaternion.RotateTowards(this.transform.rotation, endRotation, ((CommandArgs)args).Speed * Time.deltaTime);
                await UniTask.WaitForEndOfFrame();
            }

            isIdle = true;
            return "rotated";

        }

        internal async Task<string> MoveForwardAsync()
        {
            this.isIdle = false;
            var endPosition = this.transform.position + this.transform.forward * this.gameData.GridSize *1;
            endPosition = CheckBoundaries(endPosition);

            while (this.transform.position != endPosition)
            {
                this.transform.position =
                    Vector3.MoveTowards(transform.position, endPosition, 1 * Time.deltaTime);
                await UniTask.Delay(10);

            }

            isIdle = true;
            return "finished moving";
        }

        private Vector3 CheckBoundaries(Vector3 endPosition)
        {
            if (endPosition.x > boundary.xMax) endPosition.x = boundary.xMax;
            if (endPosition.x < boundary.xMin) endPosition.x = boundary.xMin;
            if (endPosition.y < boundary.yMin) endPosition.y = boundary.yMin;
            if (endPosition.y > boundary.yMax) endPosition.y = boundary.yMax;
            return endPosition;
        }

        #endregion

        #region [API]

        /// <summary>
        /// Moves the player forward by given units distance
        /// </summary>
        /// <param name="dist">The distance to move, defaults to 1</param>
        public async Task<string> MoveForward()
        {
            var task = new Task<Task<string>>(this.MoveForwardAsync);
            commandQueue.Enqueue(task);
            var taskResult = await task.Unwrap();
            return taskResult;
        }

        /// <summary>
        /// Rotates the player ccw by a given amount of degrees
        /// </summary>
        /// <param name="degrees">The amount of the rotation in degrees, defaults to 90</param>
        public async Task<string> RotateLeft(float degrees = 90)
        {
            var args = new CommandArgs() { Degrees = -degrees, Speed = this.playerRotationSpeed };
            var task = new Task<Task<string>>(RotateOverSpeedAsync, args);
            commandQueue.Enqueue(task);
            var taskResult = await task.Unwrap();
            return taskResult;
        }

        /// <summary>
        /// Rotates the player cw by a given amount of degrees
        /// </summary>
        /// <param name="degrees">The amount of the rotation in degrees, defaults to 90</param>
        public async Task<string> RotateRight()
        {
            var task = new Task<Task<string>>(this.RotateOverSpeedAsync, new CommandArgs{Degrees = 90, Speed = this.playerRotationSpeed});
            commandQueue.Enqueue(task);
            var taskResult = await task.Unwrap();
            return taskResult;
        }

        /// <summary>
        /// Fires the player weapon once
        /// </summary>
        public async Task<string> FireWeapon()
        {
            var task = new Task<Task<string>>(FireWeaponAsync);
            this.commandQueue.Enqueue(task);
            var taskResult = await task.Unwrap();
            return taskResult;
        }


        public async Task<string> ScanAhead()
        {
            
            var task = new Task<Task<string>>(ScanAheadAsync);
            this.commandQueue.Enqueue(task);
            var taskResult = await task.Unwrap();
            return taskResult;

        }

        

        #endregion

        
    }
}