using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TrueEater.NPCs
{
	//ported from my tAPI mod because I'm lazy
	public class BigZombie : ModNPC
	{
		private Player target = null;
		private int updateTime = 0;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zombie Brute");
		}
	
		public override void SetDefaults()
		{
			npc.lifeMax = 950;
			npc.damage = 18;
			npc.defense = 2;
			npc.knockBackResist = 0.0f;
			npc.width = 54;
			npc.height = 68;
			npc.aiStyle = 3;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 1500;
			npc.timeLeft = 22500;
			npc.rarity = 1;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 3;
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
		
		public override void PostAI()
		{
			if (Main.hardMode)
			{
				npc.defense = 25;
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
				float speed = 0.0005f;
				
				if (target.dead || !target.active)
				{
				}
				
				if (Main.expertMode)
				{
					speed *= 1.25f;
				}
				
				if (updateTime % 160 == 0) 
				{
					if (Math.Abs(rd.X) > 160.0f)
					{
						if(rd.X > 0)
						{
							rd.X = 160.0f;							
						}
						else
						{
							rd.X = -160.0f;							
						}
					}
					
					npc.velocity.X += rd.X * speed * 50;
					npc.velocity.Y -= 7.0f;
				}
			}
		}
		
		public override void NPCLoot()
		{
			if (Main.expertMode == true && (!Main.hardMode || Main.rand.Next(10) == 0))
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ZombieLeg"));
			}
			
			int item = Main.rand.Next(3);
			if (item == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.ZombieArm);
			}
			if (item == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Main.expertMode ? ItemID.GreaterHealingPotion : ItemID.HealingPotion, 5+Main.rand.Next(8));
			}
			
			if (Main.rand.Next(2) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.IronBar, 1 + Main.rand.Next(11));
			}
			else
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.LeadBar, 1 + Main.rand.Next(11));
			}
		}
		
		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			if (npc.velocity.Y < 0)
			{
				npc.frame.Y = frameHeight * 2;
			}
			else
			{
				npc.frame.Y = frameHeight * (int)((updateTime % 30) / 15);
			}
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			float ret = 1.0f / 2400.0f;
			if (Main.bloodMoon) {ret += 0.0125f;}
			if (Main.hardMode) {ret += 0.0025f;}
			if (Main.expertMode) {ret += 59.0f / 2400.0f;}
            return spawnInfo.spawnTileY < Main.rockLayer && !Main.dayTime && NPC.downedBoss3 && !NPC.AnyNPCs(mod.NPCType("BigZombie")) ? ret * TrueEater.GetSpawnModifier(npc,spawnInfo) : 0f;
        }
	}
}