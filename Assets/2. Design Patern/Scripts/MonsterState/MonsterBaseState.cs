using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class MonsterBaseState
    {
        public Monster monster;
        public virtual void Enter()
        {

        }

        public virtual void Exit()
        {
        }

        public void Initialize(Monster monster)
        {
            this.monster = monster;
        }

        public virtual void Update()
        {

        }
    }
}
