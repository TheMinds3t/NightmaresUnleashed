using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TrueEater.NPCs
{
	public class HippyHarpy : ModNPC
	{
		private Player target = null;
		private int updateTime = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hippy Harpy");
		}


		public override void SetDefaults()
		{
			npc.lifeMax = 950;
			npc.damage = 32;
			npc.defense = 5;
			npc.knockBackResist = 0.375f;
			npc.width = 84;
			npc.height = 86;
			npc.aiStyle = 14;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 1500;
			npc.rarity = 1;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 4;
		}
		
		public override bool PreAI()
		{
			return true;
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				string[] gores = {"Gores/ZombieFoot", "Gores/ZombieArm"};
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ZombieHead"), 1f);
				for(int x = 0; x < 2; ++x)
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot(gores[Main.rand.Next(gores.Length)]), 1f);
				}
			}
		}
		
		public Player getClosestPlayer()
		{
			Player ret = null;
			for (int k = 0; k < 255; k++)
			{
				Player p = Main.player[k];
				if (p.active)
				{
					Vector2 distance = (npc.Center - p.Center);
					if (ret == null)
					{
						ret = p;
					}
					else
					{
						Vector2 distanceRet = (npc.Center - ret.Center);
						if ((abs(distanceRet.X) + abs(distanceRet.Y)) > (abs(distance.X) + abs(distance.Y)))
						{
							ret = p;
						}
					}
				}
			}
			
			return ret;
		}
		
		public float abs(float num)
		{
			if (num < 0)
			{
				return -num;
			}
			else
			{
				return num;
			}
		}
		
		public float clamp(float min, float max, float cur)
		{
			if (cur < min){cur = min;}
			if (cur > max){cur = max;}
			return cur;
		}
		
		public void fireFeather()
		{
			Main.PlaySound(3, (int)npc.position.X, (int)npc.position.Y, 0);
			Vector2 off = new Vector2(-32,-32);
			off.X = off.X + Main.rand.Next(64);
			off.Y = off.Y + Main.rand.Next(64);
			float speed = 4.0f;
			double dire = Math.Atan2((target.Center.Y - npc.Center.Y), (target.Center.X - npc.Center.X));
			Projectile.NewProjectile(npc.Center.X + off.X, npc.Center.Y + off.Y, (float)Math.Cos(dire) * speed, (float)Math.Sin(dire) * speed, ProjectileID.HarpyFeather, (int)(npc.damage * 0.375f), 0f, Main.myPlayer, npc.whoAmI, 0.25f);		
		}
		
		public override void PostAI()
		{
			if (Main.hardMode)
			{
				npc.defense = 12;
			}

			++updateTime;
			
			if (target == null)
			{
				target = getClosestPlayer();
			}
			else
			{
				Vector2 distance = (target.Center - npc.Center);
				Vector2 rd = distance;
				
				if (target.dead || !target.active)
				{
				}
				
				if (updateTime % 120 > 90)
				{
					if(updateTime % (Main.expertMode ? 3 : 4) == 2 && TrueEater.FireHostileProj())
					{
						fireFeather();
					}
				}
			}
		}
		
		public override void NPCLoot()
		{
			int item = Main.rand.Next(100);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Feather, 6 + Main.rand.Next(5));
			
			if (item > 1 && item < 4)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GiantHarpyFeather, 1);
			}
			else if(item == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HarpyNecklace"), 1);
			}
		}
		
		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			npc.frame.Y = frameHeight * (int)((updateTime % 20) / 5);
		}
	}
}