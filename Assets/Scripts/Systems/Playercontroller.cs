using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.GameEvents;
using Game.Commands;
using Game.SpaceObject;
using UnityEngine;
using Random = UnityEngine.Random;


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

        [SerializeField] private float playerRotationSpeed = 200;
        [SerializeField] private float playerSpeed = 4;
        [SerializeField] private float fireRate = 0.5f;
        //[SerializeField] private float gameSpeed = 5f;
        [SerializeField] private float unitSize = 1;
        [SerializeField] private Boundary boundary;
        [SerializeField] private GameObject lastScanned;
        [SerializeField] private List<GameObject> cargoBay;
        [SerializeField] private int cargoBayCapacity = 5;
        [SerializeField] private GameObject shot;

        //remove this?
        //[SerializeField] private Transform shotSpawn;
        [SerializeField] private bool isDisabled;
        [SerializeField] private bool isIdle = true;
        private GameEventManager eventManager;
        private GameData gameData;
        private float nextFire;
        private List<int> freeSlots;
        
        #endregion

        public void Awake()
        {
            this.gameData = GameData.Instance;
            this.isDisabled = false;
            this.cargoBay = new List<GameObject>(new GameObject[this.cargoBayCapacity]);
            this.freeSlots = Enumerable.Range(0, this.cargoBayCapacity).ToList();
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
        //TODO - IMPORTANT - implement cancellation for the async tasks
        /// <summary>
        /// Fires the player weapon once
        /// </summary>
        public async UniTask<string> FireWeaponAsync()
        {
          
            await UniTask.Delay((int)(this.fireRate * 1000));
            this.nextFire = Time.time + fireRate;
            Instantiate(shot, this.transform.position + new Vector3(0, 1, 0), this.transform.rotation);
            return "shot fired";
        }

        /// <summary>
        /// Scans for objects 1 unit ahead of the player
        /// </summary>
        /// <returns>The the space object type</returns>
        public async UniTask<string> ScanAheadAsync()
        {
            this.isIdle = false;
            await UniTask.Delay(1000);

            //TODO play scan animation
            var objectInFront = GetObjectInFront();
            if (objectInFront != null)
            {
                this.lastScanned = objectInFront;
                this.isIdle = true;
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

        public async UniTask<string> PickupObject()
        {
            var objectAhead = this.GetObjectInFront();
            var spaceObject = objectAhead.GetComponent<ISpaceObject>();
            //Play animation
            await UniTask.Delay(1000);
            if (objectAhead && spaceObject.IsIdentified && spaceObject.SpaceObjectType != SpaceObjectType.Asteroid)
            {
                int slot = GetRandomCargoSlot();
                if (slot >= 0)
                {
                    this.cargoBay[slot] = objectAhead;
                    objectAhead.SetActive(false);
                    Debug.Log($"Adding cargo {spaceObject.SpaceObjectType} at slot {slot}");
                    return spaceObject.SpaceObjectType.ToString();
                }
                return "No space left in cargo bay";
            }
          
            return "No collectable object found";
        }

        public async UniTask<string> UnloadCargoAt(int slotIndex)
        {
            //Todo play animation
            await UniTask.Delay(1000);
            var unloadPosition = this.transform.position + transform.TransformDirection(Vector3.forward) * 2;
            if (this.GetObjectInFront() == null && InBounds(unloadPosition))
            {
                var cargo = this.cargoBay[slotIndex];
                this.freeSlots.Add(slotIndex);
                this.cargoBay[slotIndex] = null;
                cargo.transform.position = unloadPosition;
                cargo.SetActive(true);
                return cargo.GetComponent<ISpaceObject>().SpaceObjectType.ToString();
            }
            return "Location occupied";
        }

        public string[] GetCargo()
        {
            var result = new List<string>();
            foreach (var cargo in this.cargoBay)
            {
                if (cargo == null)
                {
                    result.Add("null");
                }
                else
                {
                    result.Add(cargo.GetComponent<ISpaceObject>().SpaceObjectType.ToString());
                }
            }

            return result.ToArray();
        }

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

        #endregion

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

        private GameObject GetObjectInFront()
        {
            Debug.DrawRay(this.transform.position, transform.TransformDirection(Vector3.forward) * 2, Color.yellow, 1,
                false);
            if (Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.forward),
                out var hitResult, 2))
            {
                Debug.Log($"<color=orange>Object in front:</color> {hitResult.collider.gameObject.name}");
                this.lastScanned = hitResult.transform.gameObject;
                return hitResult.transform.gameObject;
            }
            return null;
        }
    }
}