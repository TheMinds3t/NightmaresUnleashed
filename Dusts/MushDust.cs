using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TrueEater.Dusts
{
	public class MushDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noLight = true;
			dust.color = new Color(93-10+Main.rand.Next(20), 127-10+Main.rand.Next(20), 245-10+Main.rand.Next(20));
			dust.scale = 1.125f;
			dust.noGravity = true;
			dust.velocity /= 2f;
			dust.alpha = 85;
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X;
			Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), (0.0f / 255.0f), (0.0f / 255.0f), (255.0f / 255.0f));
			dust.scale -= 0.00625f;
			dust.alpha = 255 - (int)(255 * dust.scale);
			if (dust.scale < 0.0f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}