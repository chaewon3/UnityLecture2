using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyProject.Skill;
using MyProject.State;

namespace MyProject
{

    //���� ���� ������ ����, ���� �����̴� ������ �ƴϸ� �����ִ��� �Ǵ��ϰ� ����.
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
        // 0�̸� �����ִ� ����, 1�̸� �����̴� ����. ���� Ȯ���� �Ǹ� 2�϶� ����, 3�϶� ���� ��
        public float stateStay; // ���� ���¿� �ӹ� �ð�
        public float moveDistance; // �� �̵��ð�

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



            ////magnitude : vector�� ����  �������̸� �����ϴ� ����(condition)
            //if (moveDir.magnitude < 0.1f)
            //    ChangeState(State.Idle);
            //else
            //    ChangeState(State.Move);
        }

        //���� ����
        public void ChangeState/*Transition*/(State nextState)
        {
            if (currenState != nextState)
            {
                switch (currenState)
                {
                    case State.Idle:
                        print("������ ����");
                        break;
                    case State.Move:
                        print("�̵����� ����");
                        break;
                }

                //enter
                switch (nextState)
                {
                    case State.Idle:
                        print("������ ����");
                        break;
                    case State.Move:
                        print("�̵����� ����");
                        break;
                }
                currenState = nextState;
                stateStay = 0;
            }
        }

        public void StateUpdate()
        {
            // ���� ���¿� ���� �ൿ ����
            switch (currenState)
            {
                case State.Idle:
                    // ���� ���°� Idle�� �� �� ��
                    text.text = $"{State.Idle} state : {stateStay: 0}";
                    break;
                case State.Move:
                    // ���� ���°� Move�� �� �� ��
                    text.text = $"{State.Move} state : {stateStay.ToString("0")}";
                    break;
            }

            stateStay += Time.deltaTime;
        }
    }
}