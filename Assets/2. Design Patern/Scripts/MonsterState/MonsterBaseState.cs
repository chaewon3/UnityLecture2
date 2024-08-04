using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.State
{
    public class MonsterBaseState
    {
        public Monster monster;
        public Animator anim;
        public virtual void Enter(IHitable hitable)
        {
        }

        public virtual void Exit()
        {
        }

        public void Initialize(Monster monster)
        {
            this.monster = monster;
            anim = monster.GetComponent<Animator>();
        }

        public virtual void Update()
        {

        }
    }
}
