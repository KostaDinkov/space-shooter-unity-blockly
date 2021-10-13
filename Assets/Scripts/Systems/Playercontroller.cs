using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assets.Scripts.Exceptions;
using Assets.Scripts.Systems;
using Scripts.Commands;
using Scripts.Exceptions;
using Scripts.GameEvents;
using Scripts.SpaceObject;
using Cysharp.Threading.Tasks;
using Scripts.Behaviours;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;


namespace Scripts.Systems
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

        [SerializeField] private float playerRotationSpeed = 200;
        [SerializeField] private float playerSpeed = 4;

        [SerializeField] private float fireRate = 0.5f;

        //[SerializeField] private float gameSpeed = 5f;
        [SerializeField] private float unitSize = 1;
        [SerializeField] private Boundary boundary;
        [SerializeField] private GameObject lastScanned;
        [SerializeField] private List<GameObject> cargoBay;
        [SerializeField] private int cargoBayCapacity = 5;
        [SerializeField] private GameObject lastPickedUp;
        [SerializeField] private GameObject shot;

        //remove this?
        //[SerializeField] private Transform shotSpawn;
        [SerializeField] private bool isDisabled;

        [SerializeField] private bool isAlive;

        [SerializeField] private bool isIdle = true;
        private GameEventManager eventManager;
        private GameData gameData;
        private float nextFire;
        private List<int> freeSlots;
        private UniTask<string> currentTask;
        private InGameLogger logger;
        private ParticleSystem scannerPs;
        private ParticleSystem pickupPs;

        #endregion

        public void Awake()
        {
            this.gameData = GameData.Instance;
            this.isDisabled = false;
            this.isAlive = true;
            this.cargoBay = new List<GameObject>(new GameObject[this.cargoBayCapacity]);
            this.freeSlots = Enumerable.Range(0, this.cargoBayCapacity).ToList();
            this.logger = new InGameLogger();
            this.scannerPs = this.transform.GetChild(3).GetComponent<ParticleSystem>();
            this.pickupPs = this.transform.GetChild(2).GetComponent<ParticleSystem>();
        }

        public void Start()
        {
            this.eventManager = GameEventManager.Instance;
            this.eventManager.Subscribe(GameEventType.ProblemCompleted, this.OnProblemCompleted);
            this.eventManager.Subscribe(GameEventType.ProblemStarted, (value) => { this.isDisabled = false; });
        }


        private void Update()
        {
            this.ReadInput();
            if (this.currentTask.Status == UniTaskStatus.Faulted)
            {
                this.currentTask = new UniTask<string>();
            }
        }

        private void OnProblemCompleted(object args)
        {
            this.isDisabled = true;
        }

        private void ReadInput()
        {
            if (this.isDisabled)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space) && this.currentTask.Status == UniTaskStatus.Succeeded)
            {
                this.currentTask = this.FireWeaponAsync();
            }

            if (Input.GetKeyDown(KeyCode.I) && this.currentTask.Status == UniTaskStatus.Succeeded)
            {
                this.currentTask = this.MoveForwardAsync();
            }

            if (Input.GetKeyDown(KeyCode.J) && this.currentTask.Status == UniTaskStatus.Succeeded)
            {
                this.currentTask = this.RotateLeftAsync();
            }

            if (Input.GetKeyDown(KeyCode.L) && this.currentTask.Status == UniTaskStatus.Succeeded)
            {
                this.currentTask = this.RotateRightAsync();
            }

            if (Input.GetKeyDown(KeyCode.U) && this.currentTask.Status == UniTaskStatus.Succeeded)
            {
                this.currentTask = this.ScanAheadAsync();
            }

            if (Input.GetKeyDown(KeyCode.O) && this.currentTask.Status == UniTaskStatus.Succeeded)
            {
                try
                {
                    this.currentTask = this.PickupObjectAsync();
                }
                catch (Exception e) when (!(e is OperationCanceledException))
                {
                    Debug.LogException(e);
                }
            }

            if (Input.GetKeyDown(KeyCode.P) && this.currentTask.Status == UniTaskStatus.Succeeded)
            {
                this.currentTask = this.UnloadCargoAt(this.cargoBay.IndexOf(this.lastPickedUp));
            }
        }


        public void Die()
        {
            this.isAlive = false;
            this.gameObject.SetActive(false);
        }

        #region [API]

        //TODO - IMPORTANT - implement cancellation for the async tasks
        /// <summary>
        /// Fires the player weapon once
        /// </summary>
        public async UniTask<string> FireWeaponAsync()
        {
            if (!this.isAlive) throw new PlayerDiedException();

            this.nextFire = Time.time + this.fireRate;
            Instantiate(this.shot, this.transform.position + this.transform.forward * 2, this.transform.rotation);
            await UniTask.Delay((int) (this.fireRate * 1000));
            return "shot fired";
        }

        /// <summary>
        /// Scans for objects 1 unit ahead of the player
        /// </summary>
        /// <returns>The the space object type</returns>
        public async UniTask<string> ScanAheadAsync()
        {
            this.scannerPs.Play();
            await UniTask.Delay(1000);

            var objectInFront = this.GetObjectInFront();

            if (objectInFront != null)
            {
                this.lastScanned = objectInFront;
                var unidentifiedObject = objectInFront.GetComponent<UnidentifiedObject>();
                if (unidentifiedObject != null)
                {
                    unidentifiedObject.Identify();
                }
                //Debug.Log(objectInFront.GetComponent<SpaceObject.SpaceObject>().SpaceObjectType.ToString());
                return this.lastScanned.GetComponent<SpaceObject.SpaceObject>().SpaceObjectType.ToString();
            }

            return "Scanner found no object";
        }

        /// <summary>
        /// Moves the player forward by given units distance
        /// </summary>
        /// <param name="dist">The distance to move, defaults to 1</param>
        public async UniTask<string> MoveForwardAsync(int dist = 1)
        {
            if (!this.isAlive) throw new PlayerDiedException();

            var endPosition = this.transform.position + this.transform.forward * GameData.GridSize * dist;
            endPosition = this.CheckBoundaries(endPosition);

            while (this.transform.position != endPosition && this.isAlive)
            {
                this.transform.position =
                    Vector3.MoveTowards(this.transform.position, endPosition, this.playerSpeed * Time.deltaTime);
                await UniTask.WaitForEndOfFrame(this.GetCancellationTokenOnDestroy());
            }

            return "finished moving";
        }

        /// <summary>
        /// Rotates the player ccw by a given amount of degrees
        /// </summary>
        /// <param name="degrees">The amount of the rotation in degrees, defaults to 90</param>
        public async UniTask<string> RotateLeftAsync(float degrees = 90)
        {
            if (!this.isAlive) throw new PlayerDiedException();
            var args = new CommandArgs() {Degrees = -degrees, Speed = this.playerRotationSpeed};
            return await this.RotateOverSpeedAsync(args);
        }

        /// <summary>
        /// Rotates the player cw by a given amount of degrees
        /// </summary>
        /// <param name="degrees">The amount of the rotation in degrees, defaults to 90</param>
        public async UniTask<string> RotateRightAsync(float degrees = 90)
        {
            if (!this.isAlive) throw new PlayerDiedException();
            var args = new CommandArgs {Degrees = degrees, Speed = this.playerRotationSpeed};
            return await this.RotateOverSpeedAsync(args);
        }

        public async UniTask<string> PickupObjectAsync()
        {
            if (!this.isAlive) throw new PlayerDiedException();
            var objectAhead = this.GetObjectInFront();
            if (!objectAhead)
            {
                await this.PlayPickupPs(false, false);
                Debug.LogError("Няма намерен обект за товарене!");
                return null;
            }

            var spaceObject = objectAhead.GetComponent<SpaceObject.SpaceObject>();

            if (!spaceObject.IsCollectable)
            {
                await this.PlayPickupPs(false, false);
                Debug.LogWarning("Обектът не може да бъде натоварен!");

                return null;
            }

            int slot = this.GetRandomCargoSlot();
            if (slot < 0)
            {
                await this.PlayPickupPs(false, false);
                Debug.LogError("Товарното помещение е пълно!");

                return null;
            }

            await this.PlayPickupPs(true, false);

            this.cargoBay[slot] = objectAhead;
            objectAhead.SetActive(false);
            this.lastPickedUp = objectAhead;
            Debug.Log($"Добавяне на обект: {spaceObject.SpaceObjectType} на позиция: {slot}");

            return spaceObject.SpaceObjectType.ToString();
        }

        public async UniTask<string> UnloadCargoAt(int slotIndex)
        {
            if (!this.isAlive) throw new PlayerDiedException();

            var unloadPosition = this.transform.position + this.transform.TransformDirection(Vector3.forward) * 2;
            if (this.GetObjectInFront() == null && this.InBounds(unloadPosition))
            {
                await this.PlayPickupPs(success: true, unloading: true);

                var cargo = this.cargoBay[slotIndex];
                this.freeSlots.Add(slotIndex);
                this.cargoBay[slotIndex] = null;
                cargo.transform.position = unloadPosition;
                cargo.SetActive(true);
                return cargo.GetComponent<SpaceObject.SpaceObject>().SpaceObjectType.ToString();
            }

            await this.PlayPickupPs(false, true);
            
            Debug.LogError("Мястото пред дрона не е свободно!");
            return null;
        }

        public string[] GetCargo()
        {
            if (!this.isAlive) throw new PlayerDiedException();
            var result = new List<string>();
            foreach (var cargo in this.cargoBay)
            {
                if (cargo == null)
                {
                    result.Add("null");
                }
                else
                {
                    result.Add(cargo.GetComponent<SpaceObject.SpaceObject>().SpaceObjectType.ToString());
                }
            }

            return result.ToArray();
        }


        public void Print(object msg)
        {
            this.logger.Log(msg.ToString());
        }

        #endregion

        private bool InBounds(Vector3 position)
        {
            if (position.x < this.boundary.xMin ||
                position.x > this.boundary.xMax ||
                position.y < this.boundary.yMin ||
                position.y > this.boundary.xMax)
            {
                return false;
            }

            return true;
        }

        private int GetRandomCargoSlot()
        {
            if (this.freeSlots.Count > 0)
            {
                var slotIndex = Random.Range(0, this.freeSlots.Count);
                var slot = this.freeSlots[slotIndex];
                this.freeSlots.RemoveAt(slotIndex);
                return slot;
            }

            return -1;
        }

        private Vector3 CheckBoundaries(Vector3 endPosition)
        {
            if (endPosition.x > this.boundary.xMax) endPosition.x = this.boundary.xMax;
            if (endPosition.x < this.boundary.xMin) endPosition.x = this.boundary.xMin;
            if (endPosition.y < this.boundary.yMin) endPosition.y = this.boundary.yMin;
            if (endPosition.y > this.boundary.yMax) endPosition.y = this.boundary.yMax;
            return endPosition;
        }

        private async UniTask<string> RotateOverSpeedAsync(object args)
        {
            var end = Quaternion.Euler(0, ((CommandArgs) args).Degrees, 0);

            var endRotation = this.transform.rotation * end;
            while (this.transform.rotation != endRotation)
            {
                this.transform.rotation =
                    Quaternion.RotateTowards(this.transform.rotation, endRotation,
                        ((CommandArgs) args).Speed * Time.deltaTime);
                await UniTask.WaitForEndOfFrame();
            }

            return "rotated";
        }

        private async UniTask PlayPickupPs(bool success, bool unloading)
        {
            var originalPsPosition = this.pickupPs.transform.localPosition;
            var main = this.pickupPs.main;
            if (unloading)
            {
                this.pickupPs.transform.localPosition = new Vector3(0, 0, 0);
                this.pickupPs.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            if (!success)
            {
                main.startColor = new Color(1, 0, 0.1276541f);
            }
            else
            {
                main.startColor = new Color(0.09811318f, 1, 0.7056843f);
            }

            this.pickupPs.Play();
            await UniTask.Delay(1000);
            this.pickupPs.transform.localPosition = originalPsPosition;
            this.pickupPs.transform.localRotation = Quaternion.Euler(0, -180, 0);
        }

        private GameObject GetObjectInFront()
        {
            Debug.DrawRay(this.transform.position, this.transform.TransformDirection(Vector3.forward) * 2, Color.yellow,
                1,
                false);
            return Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.forward),
                out var hitResult, 2)
                ? hitResult.transform.gameObject
                : null;
        }
    }
}