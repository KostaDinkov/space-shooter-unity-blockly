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
      gameData = GameData.Instance;

      isDisabled = false;
    }

    public void Start()
    {
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

    public void MoveForward(int distance = 1)
    {
      commandQueue.Enqueue(new MoveForwardCommand(this, distance, this.playerSpeed));
    }

    internal IEnumerator MoveForwardCoroutine(GameObject objectToMove, int distance, float speed)
    {
      isIdle = false;
      var endPosition = transform.position + transform.forward * gameData.GridSize * distance;
      endPosition = CheckBoundaries(endPosition);

      while (transform.position != endPosition)
      {
        transform.position =
            Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
        yield return new WaitForEndOfFrame();
      }

      isIdle = true;
    }
    
    /// <summary>
    /// Rotates the player 90 degrees ccw
    /// </summary>
    public void RotateLeft(float degrees = 90)
    {
      commandQueue.Enqueue(new RotateLeftCommand(this, degrees, this.playerRotationSpeed));
    }

    /// <summary>
    /// Rotates the player 90 degrees cw
    /// </summary>
    public void RotateRight(float degrees = 90)
    {
      commandQueue.Enqueue(new RotateRightCommand(this, degrees, this.playerRotationSpeed));
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



    internal IEnumerator RotateOverSpeedCoroutine(GameObject objectToMove, Quaternion end, float speed)
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