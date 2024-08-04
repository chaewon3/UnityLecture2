using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyProject.State;

namespace MyProject
{
    public class Monster : MonoBehaviour
    {
        public float damage = 3;

        public Collider trigger;
        private Rigidbody rig;
        private CharacterController target;

        private Vector3 position;
        private MonsterStateMachine statemachine;

        private void Awake()
        {
            rig = GetComponent<Rigidbody>();
            statemachine = GetComponent<MonsterStateMachine>();
            position = transform.position;
        }
        private void Start()
        {
            GameManager.instance.GameOver += gameover;
        }

        private void OnDestroy()
        {
            GameManager.instance.GameOver -= gameover;
        }

        private void Update()
        {
            if (target == null)
                return;

            Vector3 lookRotate = target.transform.position;
            lookRotate.y = rig.transform.position.y;

            rig.transform.LookAt(lookRotate);
                       

            Vector3 direction = target.transform.position - rig.transform.position;
            if (direction.magnitude <= 2)
            {
                statemachine.Transition(statemachine.attackState);
            }
            else
            {
                statemachine.Transition(statemachine.chaseState);
                rig.transform.position += rig.transform.forward * 2f * Time.deltaTime;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                StopAllCoroutines();
                target = other.GetComponent<CharacterController>();
                statemachine.hitable = other.GetComponent<IHitable>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                statemachine.Transition(statemachine.idleState);
                target = null;
                StartCoroutine(GotoStartPosition());
            }
        }

        IEnumerator GotoStartPosition()
        {
            Vector3 lookRotate = position;

            Vector3 direction;
            do
            {
                lookRotate.y = rig.transform.position.y;

                rig.transform.LookAt(lookRotate);

                direction = position - rig.transform.position;
                rig.transform.position += rig.transform.forward * 2f * Time.deltaTime;
                yield return null;
            } while (direction.magnitude > 0.3f);

        }

        public void gameover(bool isdead)
        {
            //이부분이 왜 안되는지 모르겠어요
            trigger.enabled = false;
            statemachine.Transition(statemachine.idleState);
        }
    }
}
