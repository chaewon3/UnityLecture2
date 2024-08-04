using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.State
{
	public class MonsterStateIdle : MonsterBaseState
	{
        public override void Enter(IHitable hitable)
        {
            anim.SetTrigger("Idle");
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
        }

    }
}
