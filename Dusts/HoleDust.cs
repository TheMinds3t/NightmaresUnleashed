using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace TrueEater.Dusts
{
	public class HoleDust : ModDust
	{
		public static bool BlackHoleEatsBlocks = false;
		
		public override void OnSpawn(Dust dust)
		{
			dust.noLight = true;
			dust.color = new Color(255, 100, 200);
			dust.scale = 1.5f;
			dust.noGravity = true;
			dust.velocity /= 2f;
			dust.alpha = 100;
		}
		
		public NPC GetClosestNPC(Dust dust)
		{
			Vector2 position = dust.position;
			NPC ret = null;
			for (int k = 0; k < 255; k++)
			{
				NPC p = Main.npc[k];
				if (p != null && p.active)
				{
					Vector2 distance = (position - p.Center);
					if (ret == null)
					{
						ret = p;
					}
					else
					{
						Vector2 distanceRet = (position - ret.Center);
						if ((Math.Abs(distanceRet.X) + Math.Abs(distanceRet.Y)) > (Math.Abs(distance.X) + Math.Abs(distance.Y)))
						{
							ret = p;
						}
					}
				}
			}
			
			return ret;
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X;
			Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), (255.0f / 255.0f), (100.0f / 255.0f), (200.0f / 255.0f));
			dust.scale -= 0.0625f;
			dust.alpha = 255 - (int)(255 * dust.scale);
			if (dust.scale < 0.0f)
			{
				dust.active = false;
			}
			
			if(BlackHoleEatsBlocks)
			{
				Main.tile[(int)(dust.position.X / 16f),(int)(dust.position.Y / 16f)].active(false);
				WorldGen.hardUpdateWorld((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f));
			}
			
			return false;
		}
	}
}