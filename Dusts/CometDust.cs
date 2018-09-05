using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TrueEater.Dusts
{
	public class CometDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noLight = true;
			dust.color = new Color(56, 56, 56);
			dust.scale = 1.5f;
			dust.noGravity = true;
			dust.velocity /= 2f;
			dust.alpha = 100;
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X;
			Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), (112.0f / 255.0f), (112.0f / 255.0f), (112.0f / 255.0f));
			dust.scale -= 0.0625f;
			dust.alpha = 255 - (int)(255 * dust.scale);
			if (dust.scale < 0.0f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}