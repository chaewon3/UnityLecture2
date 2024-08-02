using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.State
{
    [RequireComponent(typeof(Monster))]
    public class MonsterStateMachine : MonoBehaviour
    {
        public MonsterStateIdle idleState;
        public MonsterStateChase chaseState;
        public MonsterStateAttack attackState;
        public Monster monster;

        public MonsterBaseState currentState;

        private void Start()
        {
            idleState = new MonsterStateIdle();
            chaseState = new MonsterStateChase();
            attackState = new MonsterStateAttack();
            idleState.Initialize(monster);
            chaseState.Initialize(monster);
            attackState.Initialize(monster);

            //시작 할 때 idle state
            currentState = idleState;
            idleState.Enter();
        }

        public void Transition(MonsterBaseState state)
        {
            if (currentState == state)
                return;
            currentState.Exit();
            currentState = state;

            currentState.Enter();
        }

        private void Update()
        {
            currentState.Update();
        }

        private void Reset()
        {
            monster = GetComponent<Monster>();
        }


    }
}
