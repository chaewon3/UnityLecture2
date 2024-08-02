using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Skill
{
	public class FireBall : SkillBehaviour
	{
		public FireballProjectile projectile; // 투사체프리펩
		public float projectileSpeed;

		public override void Apply()
		{
			print("파이어볼 장착");
		}
		public override void Use()
		{
		 	Transform shotpoint = context.owner.shotPoint;

			Vector3 shotDir = shotpoint.forward;

			var obj = Instantiate(projectile, shotpoint.position, shotpoint.rotation);

			obj.SetProjectile(projectileSpeed);
			
			Destroy(obj, 3);

			print("파이어볼 발사");
		}
		public override void Remove()
		{
			print("파이어볼 제거");
		}
	}
}
