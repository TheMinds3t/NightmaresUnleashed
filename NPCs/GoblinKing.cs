using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TrueEater.NPCs
{
	[AutoloadBossHead]
	public class GoblinKing : ModNPC
	{
		private int attackAnim = 0;
		private Player target = null;
		private int updateTime = 0;
		private bool isRunningAway = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goblin King");
		}


		public override void SetDefaults()
		{
			npc.lifeMax = 1450;
			npc.damage = 10;
			npc.defense = 1;
			npc.knockBackResist = 0.125f;
			npc.width = 80;
			npc.height = 130;
			npc.aiStyle = 14;
			npc.HitSound = SoundID.NPCHit40;
			npc.DeathSound = SoundID.NPCDeath42;
			npc.value = 50000;
			npc.boss = true;
			npc.timeLeft = 22500;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 3;
			music = MusicID.Boss1;
		}
		
		public override bool PreAI()
		{
			return true;
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				string[] gores = {"Gores/GoblinFoot", "Gores/GoblinArm"};
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GoblinBody"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GoblinHead"), 1f);
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
		
		public override void AI()
		{
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
				if (isRunningAway)
				{
					speed = -speed * 3;
				}
				
				if (target.dead || !target.active)
				{
					isRunningAway = true;
				}
				
				if (Main.expertMode)
				{
					speed *= 1.25f;
				}
				
				if (attackAnim <= 0){npc.position.X = npc.position.X + rd.X * speed + (float)Math.Cos(updateTime / 180.0f) * 2.0f;}
				if (updateTime % 70 == 0) {npc.velocity.Y = -15;}
				npc.velocity.Y += 0.6f;
			}
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
		
		public void spawnGoblin()
		{
			int[] goblins = {26,27,28,29,471};
			int gob = Main.rand.Next(4);
			if(Main.hardMode && Main.rand.Next(20) == 0)
			{
				gob = 4;
			}
			
			float targetX = npc.Center.X + (Main.rand.Next(2) * 5 - 1);
			float targetY = npc.Center.Y + (Main.rand.Next(2) * 5 - 1);
			int offX = Main.rand.Next(64) - 32;
			int offY = -Main.rand.Next(64);
			NPC.NewNPC((int)npc.Center.X + offX, (int)npc.Center.Y + 40 + offY, goblins[gob], 0, npc.whoAmI, targetX, targetY);
		}
		
		public bool isGoblin(NPC npc)
		{
			if(npc.type >= 26 && npc.type <= 29)
			{
				return true;
			}
			else
			{
				if(npc.type == 471)
				{
					return true;
				}
			}
			
			return npc.type == mod.NPCType("GoblinKing");
		}
		
		public override void PostAI()
		{
			if (Main.expertMode)
			{
				npc.defense = 0;
			}
			
			int count = 0;
			for (int k = 0; k < 200; k++)
			{
				if (Main.npc[k].active && isGoblin(Main.npc[k]) )
				{
					++count;
				}
			}
			
			if(count < (Main.expertMode ? 30 : 16) && updateTime % 90 == 0 && Main.rand.Next(5) < 3)
			{
				attackAnim = 60;
			}
			
			if (attackAnim > 0)
			{
				--attackAnim;
					
				if (attackAnim == 20 && count < (Main.expertMode ? 40 : 16))
				{
					TrueEater.PlaySound(15, npc);
					for(int i=0;i<(Main.expertMode ? 7 : 3);++i)
					{
						spawnGoblin();
					}
				}
			}
		}
		
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.LesserHealingPotion;
			TrueWorld.downedGoblinKing = true;
			if (Main.expertMode == false)
			{
				int item = Main.rand.Next(3);
				if (item == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Harpoon);
				}
				if (item == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SpikyBall, 60+Main.rand.Next(40));
				}
				if (item == 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TatteredCloth, 30 + Main.rand.Next(15));
				}
				
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldCrown);

				if(Main.rand.Next(10) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GoblinKingTrophy"));
				}

				if (Main.rand.Next(2) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.IronBar, 12 + Main.rand.Next(11));
				}
				else
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldBar, 12 + Main.rand.Next(11));
				}
			}
			else
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GoblinBag"));
			}
		}
		
		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			if (attackAnim > 0)
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
			float ret = 1.0f / 1000.0f;
			if (Main.bloodMoon) {ret += 0.01f;}
			if (Main.hardMode) {ret += 0.01f;}
			if (Main.expertMode) {ret += 19.0f / 1000.0f;}
            return spawnInfo.spawnTileY < Main.rockLayer && Main.invasionType == InvasionID.GoblinArmy && !NPC.AnyNPCs(mod.NPCType("GoblinKing")) ? ret * TrueEater.GetSpawnModifier(npc,spawnInfo) : 0f;
        }
	}
}