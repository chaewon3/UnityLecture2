using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.State
{
    public class BaseState : IState
    {
        public Player player;

        public virtual void Enter()
        {
            player.stateStay = 0;
        }

        public virtual void Exit()
        {
        }

        public void Initialize(Player player)
        {
            this.player = player;
        }

        public virtual void Update()
        {

        }
    }
}
