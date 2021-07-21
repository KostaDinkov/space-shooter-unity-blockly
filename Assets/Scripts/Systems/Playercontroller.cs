using System;
using System.Collections;
using Game.GameEvents;
using Game.Commands;
using UnityEngine;

namespace Game.Systems
{
  [Serializable]
  public class Boundary
  {
    public float xMin, xMax, zMin, zMax;
  }
  /// <summary>
  /// The main functionallity of the player
  /// </summary>
  public class Playercontroller : MonoBehaviour
  {
    private readonly float playerRotationSpeed = 200;
    public float playerSpeed = 4;

    public Boundary boundary;
    private readonly CommandQueue commandQueue = new CommandQueue();
    private GameEventManager eventManager;
    public float fireRate = 0.5f;
    private GameData gameData;

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
      eventManager.Subscribe(GameEventType.ChallangeStarted, (value) => {isDisabled = false;});
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

    private void ReadInput()
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
        RotateLeft();
      }

      if (Input.GetKeyDown(KeyCode.L))
      {
        RotateRight();
      }
    }

    public void Die()
    {
      isIdle = true;
      commandQueue.Clear();
      gameObject.SetActive(false);
    }
    internal IEnumerator FireWeaponCoroutine(ICommandArgs args = null)
    {

      if (Time.time > nextfire)
      {
        nextfire = Time.time + fireRate;
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        GetComponent<AudioSource>().Play();
      }
      return null;
    }

    internal IEnumerator RotateOverSpeedCoroutine(ICommandArgs args)
    {
      var end = Quaternion.Euler(0, args.Degrees, 0);
      isIdle = false;
      var endRotation = this.transform.rotation * end;
      while (this.transform.rotation != endRotation)
      {
        this.transform.rotation =
            Quaternion.RotateTowards(this.transform.rotation, endRotation, args.Speed * Time.deltaTime);
        yield return new WaitForEndOfFrame();
      }

      isIdle = true;
    }
    internal IEnumerator MoveForwardCoroutine(ICommandArgs args)
    {
      this.isIdle = false;
      var endPosition = this.transform.position + this.transform.forward * this.gameData.GridSize * args.Distance;
      endPosition = CheckBoundaries(endPosition);

      while (this.transform.position != endPosition)
      {
        this.transform.position =
            Vector3.MoveTowards(transform.position, endPosition, args.Speed * Time.deltaTime);
        yield return new WaitForEndOfFrame();
      }

      isIdle = true;
    }

    private Vector3 CheckBoundaries(Vector3 endPosition)
    {
      if (endPosition.x > boundary.xMax)
      {
        endPosition.x = boundary.xMax;
      }

      if (endPosition.x < boundary.xMin)
      {
        endPosition.x = boundary.xMin;
      }

      if (endPosition.z < boundary.zMin)
      {
        endPosition.z = boundary.zMin;
      }

      if (endPosition.z > boundary.zMax)
      {
        endPosition.z = boundary.zMax;
      }

      return endPosition;
    }

    #region [API]
    /// <summary>
    /// Moves the player forward by given units distance
    /// </summary>
    /// <param name="dist">The distance to move, defaults to 1</param>

    public void MoveForward(int distance = 1)
    {
      var args = new CommandArgs() { Distance = distance, Speed = this.playerSpeed };
      ICommand command = new Command(this, MoveForwardCoroutine, args);
      commandQueue.Enqueue(command);
    }

    /// <summary>
    /// Rotates the player ccw by a given amount of degrees
    /// </summary>
    /// <param name="degrees">The amount of the rotation in degrees, defaults to 90</param>
    public void RotateLeft(float degrees = 90)
    {
      var args = new CommandArgs() { Degrees = -degrees, Speed = this.playerRotationSpeed };
      var command = new Command(this, RotateOverSpeedCoroutine, args);
      commandQueue.Enqueue(command);
    }

    /// <summary>
    /// Rotates the player cw by a given amount of degrees
    /// </summary>
    /// <param name="degrees">The amount of the rotation in degrees, defaults to 90</param>
    public void RotateRight(float degrees = 90)
    {
      var args = new CommandArgs() { Degrees = degrees, Speed = this.playerRotationSpeed };
      var command = new Command(this, RotateOverSpeedCoroutine, args);
      commandQueue.Enqueue(command);
    }

    /// <summary>
    /// Fires the player weapon once
    /// </summary>
    public void FireWeapon()
    {
      var fireCommand = new Command(this, FireWeaponCoroutine, new CommandArgs());
      this.commandQueue.Enqueue(fireCommand);
    }
    #endregion
  }
}