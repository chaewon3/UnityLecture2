using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.State
{
    public class MoveState : BaseState
    {
        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            Debug.Log("�̵����� ����");
        }

        public override void Update()
        {
            player.text.text = $"{GetType().Name} : {player.stateStay:n0}";
        }

    }
}
