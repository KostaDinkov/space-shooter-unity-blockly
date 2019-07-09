using System.Dynamic;
using System;
using Game.Systems;
using UnityEngine;

namespace Game.Commands
{

  public class Command:ICommand
  {
    protected Func<ICommandArgs,object> action;
    protected ICommandArgs args;

    protected Playercontroller pc;

    public Command(Playercontroller pc, Func <ICommandArgs,object> action, ICommandArgs args)
    {
      this.action = action;
      this.args = args;
      this.pc = pc;
    }

    public  void Execute(){
      Debug.Log(action.Method.Name);
      this.pc.StartCoroutine(action.Method.Name,args);
    }
  }
}


