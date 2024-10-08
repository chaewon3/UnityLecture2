using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.State
{
    public class IdleState : BaseState
    {

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
            player.text.text = $"HP : {player.currentHP}";
            player.moveDistance += Time.deltaTime;
        }
    }
}
