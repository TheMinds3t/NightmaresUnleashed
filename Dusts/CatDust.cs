using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TrueEater.Dusts
{
	public class CatDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noLight = true;
			dust.color = new Color(255, 0, 0);
			dust.scale = 1.5f;
			dust.noGravity = true;
			dust.velocity /= 2f;
			dust.alpha = 100;
		}

		public override bool Update(Dust dust)
		{
			dust.color = new Color((dust.position.X % 510) / 2, dust.position.X % 255, (dust.position.X % 1020) / 4);
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X;
			Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 1.0f, 1.0f, 1.0f);
			dust.scale -= 0.05f;
			if (dust.scale < 0.25f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}