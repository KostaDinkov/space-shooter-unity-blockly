using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Authentication;
using Cysharp.Threading.Tasks.Triggers;

public class Auth: MonoBehaviour
{
    // Start is called before the first frame update
    private Authientication auth;
    private void Awake()
    {
        this.auth = new Authientication();
        Authentication.Authientication.CreateApplication();

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Login()
    {
        this.auth.CallGraphButton_Click();
    }
}
