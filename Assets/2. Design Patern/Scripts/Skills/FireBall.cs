using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Skill
{
	public class FireBall : SkillBehaviour
	{
		public FireballProjectile projectile; // ����ü������
		public float projectileSpeed;

		public override void Apply()
		{
			print("���̾ ����");
		}
		public override void Use()
		{
		 	Transform shotpoint = context.owner.shotPoint;

			Vector3 shotDir = shotpoint.forward;

			var obj = Instantiate(projectile, shotpoint.position, shotpoint.rotation);

			obj.SetProjectile(projectileSpeed);
			
			Destroy(obj, 3);

			print("���̾ �߻�");
		}
		public override void Remove()
		{
			print("���̾ ����");
		}
	}
}
