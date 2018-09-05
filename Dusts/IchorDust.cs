using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TrueEater.Dusts
{
	public class IchorDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.color = new Color(255, 255, 0);
			dust.scale = 1.2f;
			dust.velocity /= 2f;
			dust.alpha = 100;
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X;
			Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), (255f / 255f), (231f / 255f), (91f / 255f));
			dust.scale -= 0.03f;
			dust.velocity.Y += 0.1f;
			if (dust.scale < 0.5f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}