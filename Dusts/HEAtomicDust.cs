using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TrueEater.Dusts
{
	public class HEAtomicDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noLight = true;
			dust.color = new Color(180, 0, 0);
			dust.scale = 1.2f;
			dust.noGravity = true;
			dust.velocity /= 2f;
			dust.alpha = 100;
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X;
			Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), (180.0f / 255.0f), 0.0f, 0.0f);
			dust.scale -= 0.03f;
			if (dust.scale < 0.5f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}