using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Game.Systems;

public class DestroyByContact : MonoBehaviour
{
  public GameObject explosion;
  public GameObject playerExplosion;
  private GameController gameController;
  public int scorePoints = 10;

  void Start()
  {
    GameObject gameControllerObj = GameObject.FindWithTag("GameController");
    if (gameControllerObj != null)
    {
      gameController = gameControllerObj.GetComponent<GameController>();
    }
    else
    {
      Debug.Log("Cannot find game object with tag GameController");
    }
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Boundary") || other.CompareTag("Enemy"))
    {
      return;
    }

    if (explosion != null)
    {
      Instantiate(explosion, transform.position, transform.rotation);
    }


     if (other.tag == "Player")
    {
      Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
      GameController.instance.Player.GetComponent<Playercontroller>().Die();

    }
    else
    {
      Destroy(other.gameObject);
    }



    Destroy(gameObject);
  }
}