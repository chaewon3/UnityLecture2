using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.State
{
	public class MonsterStateChase : MonsterBaseState
	{
        public override void Enter(IHitable hitable)
        {
            anim.SetTrigger("Chase");
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
