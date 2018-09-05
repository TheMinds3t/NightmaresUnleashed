using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TrueEater.Mounts
{
	public class MechaBee : ModMountData
	{
		private int laserCooldown = 20;
		
		public override void SetDefaults()
		{
			mountData.spawnDust = mod.DustType("SAtomic");
			mountData.buff = mod.BuffType("BeeMount");
			mountData.heightBoost = 20;
			mountData.fallDamage = 0.0f;
			mountData.runSpeed = 3f;
			mountData.dashSpeed = 4.5f;
			mountData.flightTimeMax = 25;
			mountData.fatigueMax = 60;
			mountData.jumpHeight = 10;
			mountData.acceleration = 0.6125f;
			mountData.jumpSpeed = 12.5f;
			mountData.blockExtraJumps = false;
			mountData.totalFrames = 10;
			mountData.constantJump = true;
			int[] array = new int[mountData.totalFrames];
			for (int l = 0; l < array.Length; l++)
			{
				array[l] = 20;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 0;
			mountData.bodyFrame = 3;
			mountData.yOffset = 13;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 2;
			mountData.standingFrameDelay = 20;
			mountData.standingFrameStart = 4;
			mountData.runningFrameCount = 4;
			mountData.runningFrameDelay = 37;
			mountData.runningFrameStart = 5;
			mountData.flyingFrameCount = 4;
			mountData.flyingFrameDelay = 2;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 4;
			mountData.inAirFrameDelay = 10;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 10;
			mountData.idleFrameDelay = 4;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.textureWidth = mountData.backTexture.Width;
				mountData.textureHeight = mountData.backTexture.Height;
			}
		}
		
		public NPC getClosestEnemy(Player npc)
		{
			NPC ret = null;

			for (int k = 0; k < 200; k++)
			{
				NPC p = Main.npc[k];
				
				if (p.active && p.lifeMax > 5)
				{
					Vector2 distance = (npc.Center - p.Center);
					if (ret == null)
					{
						ret = p;
					}
					else
					{
						Vector2 distanceRet = (npc.Center - ret.Center);
						if ((Math.Abs(distanceRet.X) + Math.Abs(distanceRet.Y)) > (Math.Abs(distance.X) + Math.Abs(distance.Y)))
						{
							ret = p;
						}
					}
				}
			}
			
			return ret;
		}

		public void fireLaser(Player player, NPC npc)
		{
			laserCooldown = 20;
			Main.PlaySound(3, (int)player.position.X, (int)player.position.Y, 0);
			Vector2 dir = npc.Center - player.Center;
			Projectile.NewProjectile(player.Center.X, player.Center.Y, dir.X * 0.025f, dir.Y * 0.025f, 20, (Main.expertMode ? 125 : 75), 0, player.whoAmI, npc.whoAmI, 0);		
		}
		
		public void updateLaserAI(Player player)
		{
			if (laserCooldown > 0)
			{
				--laserCooldown;
			}
			else
			{
				NPC n = getClosestEnemy(player);
			
				if (n != null)
				{
					Vector2 dist = (n.Center - player.Center);
					
					if (Math.Abs(dist.X) + Math.Abs(dist.Y) < 640)
					{
						fireLaser(player, n);	
					}
				}			
			}
		}
	
		public override void UpdateEffects(Player player)
		{
			
			if (Math.Abs(player.velocity.X) > 4f)
			{
				updateLaserAI(player);
				Rectangle rect = player.getRect();
				Dust.NewDust(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, mod.DustType("HAtomic"));
			}
		}
	}
}