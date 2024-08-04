using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.State
{
	public class MonsterStateAttack : MonsterBaseState
	{
        IHitable hitable;
        float timer = 0;
        public override void Enter(IHitable hitable)
        {
            anim.SetTrigger("Attack");
            this.hitable = hitable;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 2f)
            {
                hitable.Hit(monster.damage);
                timer = 0;
            }
        }

    }
}
