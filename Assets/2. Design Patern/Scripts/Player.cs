using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyProject.Skill;
using MyProject.State;

namespace MyProject
{

    //상태 패턴 구현을 위해, 현재 움직이는 중인지 아니면 멈춰있는지 판단하고 싶음.
    public class Player : MonoBehaviour
    {
        public enum State
        {
            Idle = 0,
            Move = 1
            //jump = 2,
            //attack = 3
        }

        private CharacterController cc;
        public float moveSpeed = 5;
        public TextMeshPro text;
        public State currenState;
        // 0이면 멈춰있는 상태, 1이면 움직이는 상태. 추후 확장이 되면 2일때 공격, 3일때 쩜프 등
        public float stateStay; // 현재 상태에 머문 시간
        public float moveDistance; // 총 이동시간

        public Transform shotPoint;
        private SkillContext skillcontext;
        private StateMachine stateMachine;

        private void Awake()
        {
            cc = GetComponent<CharacterController>();
            skillcontext = GetComponentInChildren<SkillContext>();
            stateMachine = GetComponent<StateMachine>();
            SkillBehaviour[] skills = skillcontext.GetComponentsInChildren<SkillBehaviour>();

            foreach(SkillBehaviour sk in skills)
            {
                skillcontext.AddSkill(sk);
            }
            skillcontext.SetCurrentSkill(0);
        }

        private void Start()
        {
            currenState = State.Idle;
        }

        private void Update()
        {
            Move();
            //StateUpdate();
            if(Input.GetButtonDown("Fire1"))
            {
                skillcontext.UseSkill();
            }
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                skillcontext.SetCurrentSkill(0);
            }
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                skillcontext.SetCurrentSkill(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                skillcontext.SetCurrentSkill(2);
            }

        }

        public void Move()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            Vector3 moveDir = new Vector3(x, 0, y);

            cc.Move(moveDir * moveSpeed * Time.deltaTime);


            if(moveDir.magnitude < 0.1f)
            {
                stateMachine.Transition(stateMachine.idleState);
            }
            else
            {
                stateMachine.Transition(stateMachine.moveState);
            }



            ////magnitude : vector의 길이  상태전이를 결정하는 조건(condition)
            //if (moveDir.magnitude < 0.1f)
            //    ChangeState(State.Idle);
            //else
            //    ChangeState(State.Move);
        }

        //상태 전이
        public void ChangeState/*Transition*/(State nextState)
        {
            if (currenState != nextState)
            {
                switch (currenState)
                {
                    case State.Idle:
                        print("대기상태 종료");
                        break;
                    case State.Move:
                        print("이동상태 종료");
                        break;
                }

                //enter
                switch (nextState)
                {
                    case State.Idle:
                        print("대기상태 시작");
                        break;
                    case State.Move:
                        print("이동상태 시작");
                        break;
                }
                currenState = nextState;
                stateStay = 0;
            }
        }

        public void StateUpdate()
        {
            // 현재 상태에 따른 행동 정의
            switch (currenState)
            {
                case State.Idle:
                    // 현재 상태가 Idle일 때 할 것
                    text.text = $"{State.Idle} state : {stateStay: 0}";
                    break;
                case State.Move:
                    // 현재 상태가 Move일 떄 할 것
                    text.text = $"{State.Move} state : {stateStay.ToString("0")}";
                    break;
            }

            stateStay += Time.deltaTime;
        }
    }
}