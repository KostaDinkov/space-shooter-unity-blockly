using System;
using System.Collections;
using Game.GameEvents;
using Game.Commands;
using Game.Commands.PlayerCommands;
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
    private readonly float playerSpeed = 4;
    public Boundary boundary;
    private readonly CommandQueue commandQueue = new CommandQueue();
    private GameEventManager eventManager;
    public float fireRate = 0.5f;
    private GameData gameData;

    //private float gameSpeed = 5f;
    private bool isDisabled;

    private bool isIdle = true;

    public MoveForward MoveForwardCommand;
    private float nextfire;
    public RotateLeft RotateLeftCommand;
    public RotateRight RotateRightCommand;
    public FireWeapon FireWeaponCommand;
    public GameObject shot;
    public Transform shotSpawn;
    public float speed;
    private float unitSize = 1;

    public void Awake()
    {
      gameData = GameData.Instance;

      isDisabled = false;
    }

    public void Start()
    {
      MoveForwardCommand = new MoveForward(this);
      RotateLeftCommand = new RotateLeft(this);
      RotateRightCommand = new RotateRight(this);
      FireWeaponCommand = new FireWeapon(this);
      eventManager = GameEventManager.Instance;
      eventManager.Subscribe(GameEventType.ChallangeCompleted, OnChallangeCompleted);
      eventManager.Subscribe(GameEventType.ChallangeStarted, value => isDisabled = false);
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

    /// <summary>
    /// Moves the player forward by given units distance
    /// </summary>
    /// <param name="dist">The distance to move</param>
    
    public void MoveForward(int dist = 1)
    {
      for (int i = 0; i < dist; i++)
      {
        commandQueue.Enqueue(MoveForwardCommand);    
      }
    }

    /// <summary>
    /// Rotates the player 90 degrees ccw
    /// </summary>
    public void RotateLeft()
    {
      commandQueue.Enqueue(RotateLeftCommand);
    }

    /// <summary>
    /// Rotates the player 90 degrees cw
    /// </summary>
    public void RotateRight()
    {
      commandQueue.Enqueue(RotateRightCommand);
    }

    /// <summary>
    /// Fires the player weapon once
    /// </summary>
    public void FireWeapon()
    {
      if (Time.time > nextfire)
      {
        nextfire = Time.time + fireRate;
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        GetComponent<AudioSource>().Play();
      }
    }

    /// <summary>
    ///     Moves the player squares count in the player forward direction
    /// </summary>
    /// <param name="squares">The number of grid squares to move</param>
    internal IEnumerator MoveForwardProcedure(int squares = 1)
    {
      isIdle = false;
      var endPosition = transform.position + transform.forward * gameData.GridSize * squares;
      endPosition = CheckBoundaries(endPosition);

      while (transform.position != endPosition)
      {
        transform.position =
            Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
        yield return new WaitForEndOfFrame();
      }

      isIdle = true;
    }

    internal void RotateRightProcedure(float degrees = 90)
    {
      var rotation = Quaternion.Euler(0, degrees, 0);
      StartCoroutine(RotateOverSpeed(gameObject, rotation, playerRotationSpeed));
    }

    internal void RotateLeftProcedure(float degrees = 90)
    {
      var rotation = Quaternion.Euler(0, -degrees, 0);
      StartCoroutine(RotateOverSpeed(gameObject, rotation, playerRotationSpeed));
    }


    private IEnumerator RotateOverSpeed(GameObject objectToMove, Quaternion end, float speed)
    {
      isIdle = false;
      var endRotation = objectToMove.transform.rotation * end;
      while (objectToMove.transform.rotation != endRotation)
      {
        objectToMove.transform.rotation =
            Quaternion.RotateTowards(objectToMove.transform.rotation, endRotation, speed * Time.deltaTime);
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
  }
}