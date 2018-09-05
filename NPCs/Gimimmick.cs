using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TrueEater.NPCs
{
	[AutoloadBossHead]
	public class Gimimmick : ModNPC
	{
		private Player target = null;
		private int updateTime = 0;
		private int attackTime = 0;
		private int attackType = 0;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gimimmick");
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 32000;
			npc.damage = 80;
			npc.defense = 10;
			npc.knockBackResist = 0.0f;
			npc.width = 116;
			npc.height = 186;
			npc.aiStyle = 87;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 50000;
			npc.timeLeft = 22500;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 12;
			music = MusicID.Boss3;
			npc.netAlways = true;
			npc.boss = true;
		}
		
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(32000 * (1.2f + numPlayers / 10.0f));
		}
		
		public override void PostAI()
		{
			++updateTime;
			if(target == null || updateTime % 60 == 0)
			{
				target = getClosestPlayer();
			}
			
			if(attackTime > 0)
			{
				if(attackType == 0)
				{
					if(attackTime < 180 && Main.rand.Next(5) < 3 && attackTime % 10 == 0)
					{
						for(int i = 0; i < 1 + Main.rand.Next(3) + (Main.expertMode ? 4 : Main.rand.Next(4)); ++i)
						{
							if(TrueEater.FireHostileProj())
							{
								spitSword();
							}
						}
					}
				}
				else if(attackType == 1)
				{
					npc.velocity.X *= 0.0f;
					if(attackTime % 60 == 0)
					{
						lashTongue();
					}
				}
			}
			else
			{
				int c = 0;
				if (npc.life / npc.lifeMax < 0.3f)
				{
					c = 5;
				}
				
				if(attackTime % 60 == -50 && Main.rand.Next(10 - c) <= 4 + (Main.expertMode ? 2 : 0))
				{
					attackTime = 200;
					attackType = 0;
				}
			}
			
			--attackTime;
		}
		
		public void spitSword()
		{
			float f = -npc.velocity.X;
			//TrueEater.PlaySound(3, npc);
			Vector2 off = new Vector2(-32,-32);
			off.X = off.X + Main.rand.Next(64);
			off.Y = off.Y + Main.rand.Next(64) - 64;
			float speed = 0.0f + (Main.expertMode ? 5.0f : 0.0f) + Main.rand.Next(15);
			speed *= 2.0f;
			Vector2 targ = new Vector2((f < 0 ? npc.Center.X + 160 + Main.rand.Next(96) : npc.Center.X - (160 + Main.rand.Next(96))) + Main.rand.Next(1000) - 500,npc.Center.Y + Main.rand.Next(1000) - 1500);
			double dire = Math.Atan2((targ.Y - npc.Center.Y), (targ.X - npc.Center.X));
			int p = Projectile.NewProjectile(npc.Center.X + off.X, npc.Center.Y + off.Y, (float)Math.Cos(dire) * speed, (float)Math.Sin(dire) * speed, mod.ProjectileType("MimicSword"), 40 + Main.rand.Next(10), 4.0f, Main.myPlayer, npc.whoAmI, 0.25f);		
		}
		
		public void lashTongue()
		{
			//TrueEater.PlaySound(3, npc);
			Vector2 off = new Vector2(npc.width * 0.5f,-64);
			float speed = 0.25f;
			double dire = Math.Atan2(((target.Center.Y + off.Y) - npc.Center.Y), ((target.Center.X + off.X) - npc.Center.X));
			Projectile.NewProjectile(npc.Center.X + off.X, npc.Center.Y + off.Y, (float)Math.Cos(dire) * speed, (float)Math.Sin(dire) * speed, mod.ProjectileType("MimicTongue"), (int)(npc.damage), 20.0f, Main.myPlayer, npc.whoAmI, 0.25f);		
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				string[] gores = {"Gores/RoughFlesh", "Gores/RoughFlesh"};
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/RoughFlesh"), 1f);
				for(int x = 0; x < 4; ++x)
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
		
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
			TrueWorld.downedGimimmick = true;
			if (Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GimmimickBag"), 1);
			}
			else
			{
				int[] items1 = {-1,-1,-1,-1,ItemID.CoinGun,mod.ItemType("MoneyShot"),mod.ItemType("JunkLauncher")};
				int[] items2 = {-1,-1,ItemID.CoinRing,ItemID.GoldRing,ItemID.LuckyCoin,mod.ItemType("TheGimick")};
				int[] swords = {-1,-1,-1,-1,ItemID.IceBlade, ItemID.BreakerBlade, ItemID.Bladetongue, ItemID.BladeofGrass, ItemID.AdamantiteSword, ItemID.MythrilSword, ItemID.CobaltSword, ItemID.PalladiumSword, ItemID.OrichalcumSword, ItemID.TitaniumSword, ItemID.Excalibur, ItemID.Muramasa, ItemID.Cutlass, ItemID.BeamSword, ItemID.CobaltChainsaw, ItemID.MythrilChainsaw, ItemID.AdamantiteChainsaw, ItemID.PalladiumChainsaw, ItemID.OrichalcumChainsaw, ItemID.TitaniumChainsaw};
				int item = Main.rand.Next(25);
				for(int i = 0; i < 4; ++i)
				{
					int ite = swords[Main.rand.Next(swords.Length)];
					
					if (ite != -1)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ite, 1);
					}
				}
				
				int it = items1[Main.rand.Next(items1.Length)];
				if(it != -1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, it, 1);
				}
				
				it = items2[Main.rand.Next(items2.Length)];
				if(it != -1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, it, 1);
				}
				

				if(Main.rand.Next(10) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GimimmickTrophy"));
				}
				//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SoulofLight, 20);
			}
		}
		
		private int frame = 0;
		private static int[] animStyles(int type)
		{
			if (type == 0)
			{
				return new int[]{0,1,2};
			}
			if (type == 1)
			{
				return new int[]{5,6,7};
			}
			if (type == 2)
			{
				return new int[]{2,3,4,3};
			}
			if (type == 3)
			{
				return new int[]{7,8,9,8};
			}
			if (type == 4)
			{
				return new int[]{10};
			}
			if (type == 5)
			{
				return new int[]{11};
			}
			return new int[0];
		}
		
		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			int anim = 0;
			float f = -npc.velocity.X;
			
			if(attackTime <= 0)
			{
				if(f < 0)
				{
					anim = 0;
				}
				else
				{
					anim = 1;
				}
			}
			
			if(attackTime > 0)
			{
				if(attackType == 0)
				{
					if(f < 0)
					{
						anim = 2;
					}
					else
					{
						anim = 3;
					}
				}
				else if(attackType == 1)
				{
					if(f < 0)
					{
						anim = 4;
					}
					else
					{
						anim = 5;
					}
				}
			}
			
			if(updateTime % 11 == 0)
			{
				++frame;
			}
			
			if(frame >= animStyles(anim).Length)
			{
				frame = 0;
			}
			
			int frames = animStyles(anim).Length;
			int curFrame = animStyles(anim)[frame];
			npc.frame.Y = frameHeight * (int)animStyles(anim)[curFrame % frames];
		}
		
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 1.25f;
			return null;
		}
	}
}