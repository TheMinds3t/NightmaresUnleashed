using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TrueEater.NPCs
{
	public class GrasperHead : Worm
	{
		public static int MAXGRASPERLENGTH = 8;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grasper");
		}

		public override void SetDefaults()
		{
			npc.noGravity = true;
			npc.noTileCollide = true;
			head = true;
			flies = true;
			minLength = MAXGRASPERLENGTH;
			maxLength = MAXGRASPERLENGTH;
			bodyType = mod.NPCType("GrasperBody");
			tailType = mod.NPCType("GrasperTail");
			speed = 12.5f;
			turnSpeed = 0.145f;
			npc.lifeMax = 35000;
			npc.damage = 250;
			npc.defense = 0;
			npc.knockBackResist = 0.1f;
			npc.width = 96;
			npc.height = 96;
			npc.aiStyle = -1;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.scale = 1.5f;
			npc.value = 150;
			music = MusicID.Boss2;
			npc.boss = true;
			npc.netAlways = true;
			npc.timeLeft = 22400;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 1;
		}
		
		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			//npc.frame.Y = frameHeight * (int)((updateTime % 40) / 10);
		}

        public override void Init()
		{
		}

		public override bool ShouldRun()
		{
			return Main.player[npc.target].dead;
		}

		public override bool ChasePlayer()
		{
			return ai2 <= 0;
		}

		public override void CustomBehavior()
		{
			if((float)npc.life / (float)npc.lifeMax < 0.5f)
			{
				ai1 = 1;
				ai2 = 120;
				npc.knockBackResist = 0.0f;
				ai3 = npc.position.X;
				ai4 = npc.position.Y;
				if (Main.expertMode)
				{
					speed = 15.0f;
					npc.defense = npc.defense * 2;
				}
			}

			if (ai2 >= 0 && ai1 == 1)
			{
				--ai2;
				if(ai2 == 60)
				{
					ai1 = 2;
					TrueEater.PlaySound(SoundID.Roar, npc);
				}
				if (ai2 <= 0)
				{
					ai3 = 0;
					ai4 = 0;
				}
				else
				{
					float size = (float)Math.Abs(120 -ai2) / 4.0f;
					float offset = (float)ai2 / (size + 1.0f) * 6.28f * 5.0f; // Do 5 circles in radians
					npc.position = new Vector2(ai3, ai4) + new Vector2((float)Math.Cos(offset) * size, (float)Math.Sin(offset) * size);					
				}
			}

			if (ai1 == 2)
			{
				float ang = npc.rotation-(3.14f / 2.0f)+Main.rand.NextFloat() * 0.1f - 0.05f;
				float speed = 0.1f;
				int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y-32, (float)Math.Cos(ang) * speed, (float)Math.Sin(ang) * speed, ProjectileID.CursedFlameHostile, 50 + Main.rand.Next(26), 4.0f, Main.myPlayer, npc.whoAmI, 0.25f);																				
			}

			if(Main.player[npc.target] != null && Main.player[npc.target].statLife > 0 && !Main.player[npc.target].dead)
			{
				Player target = Main.player[npc.target];
			}
		}

		public void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
			TrueWorld.downedGrasper = true;

			if (!Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CursedLick"), 1);				
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CursedFlame, 5 + Main.rand.Next(10));
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SoulofFlight, 5 + Main.rand.Next(5));
				if (Main.rand.Next(10) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GrasperTrophy"), 1);
				}
			}
			else
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GrasperBag"), 1);				
			}
		}
	}
	public class GrasperBody : Worm
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grasper");
		}

		public override void SetDefaults()
		{
			npc.noGravity = true;
			npc.noTileCollide = true;
			flies = true;
			headType = mod.NPCType("GrasperHead");
			bodyType = npc.type;
			tailType = mod.NPCType("GrasperTail");
			npc.lifeMax = 35000;
			npc.damage = 100;
			npc.defense = 60;
			npc.knockBackResist = 0.1f;
			npc.width = 64;
			npc.height = 64;
			npc.aiStyle = -1;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 150;
			npc.boss = false;
			npc.netAlways = true;
			npc.timeLeft = 22400;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 2;
		}
		
		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			npc.frame.Y = frameHeight * (int)(npc.ai[2] % 2);
		}
		public override void CustomBehavior()
		{
			float scale = 1.0f + npc.ai[2] / (float)(GrasperHead.MAXGRASPERLENGTH - 1) * 0.95f;
			npc.scale = scale;
			if(npc.ai[2] == 0)
			{
				npc.width = 32;
				npc.height = 32;
			}
			//TrueEater.SendMessage(npc.whoAmI + " is this far from the tail : " + npc.ai[2]);
		}
	}
	public class GrasperTail : Worm
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grasper");
		}

		public override void SetDefaults()
		{
			npc.noGravity = true;
			npc.noTileCollide = true;
			flies = true;
			tail = true;
			headType = mod.NPCType("GrasperHead");
			bodyType = mod.NPCType("GrasperBody");
			tailType = npc.type;
			npc.lifeMax = 35000;
			npc.damage = 150;
			npc.defense = -10;
			npc.knockBackResist = 0.1f;
			npc.width = 40;
			npc.height = 40;
			npc.aiStyle = -1;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.boss = false;
			npc.netAlways = true;
			npc.timeLeft = 22400;
			npc.value = 150;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 1;
		}
		
		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			//npc.frame.Y = frameHeight * (int)((updateTime % 40) / 10);
		}

		public override void CustomBehavior()
		{
			if(npc.realLife <= 0)
			{
				return;
			}
			if((float)Main.npc[npc.realLife].life / (float)Main.npc[npc.realLife].lifeMax < 0.5f)
			{
				npc.knockBackResist = 0.0f;
				ai4 = 1.0f;
			}
			if(ai4 == 1.0f)
			{
				++ai1;
				if(ai1 >= 10 - (Main.expertMode ? 5 : 0) && TrueEater.FireHostileProj())
				{
					ai1 = 0;

					float ang = npc.rotation+(3.14f / 2.0f)+Main.rand.NextFloat() * 0.25f - 0.125f;
					float speed = (Main.expertMode ? 0.1f : 1.0f) * (4.5f+Main.rand.NextFloat() * 3.0f);
					int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y-32, (float)Math.Cos(ang) * speed, (float)Math.Sin(ang) * speed, ProjectileID.CursedFlameHostile, 50 + Main.rand.Next(26), 4.0f, Main.myPlayer, npc.whoAmI, 0.25f);	
					Main.projectile[p].timeLeft = Main.expertMode ? 360 : 1000;
				}

			}
			if(Main.player[npc.target] != null && Main.player[npc.target].statLife > 0 && !Main.player[npc.target].dead && ai4 < 1.0f)
			{
				Player target = Main.player[npc.target];
				npc.rotation = (ai3 + (float)(Math.Atan2(target.Center.Y - npc.Center.Y, target.Center.X - npc.Center.X)-(3.14f / 2.0f)) * 7.0f) / 8.0f;
				ai3 = npc.rotation;
				++ai1;
				if(ai1 >= 45 || ai3 >= 0.0f && ai1 >= 15)
				{
					++ai2;
					ai3 -= 0.0675f;
					if (ai2 >= 10)
					{
						ai3 = 1.0f;
					}
					if (ai3 >= 0.0f)
					{
						ai2 = -1;						
					}
					ai1 = 0;
					float ang = npc.rotation+(3.14f / 2.0f)+Main.rand.NextFloat() * 0.1f - 0.05f;
					float speed = 10.5f+Main.rand.NextFloat() * 2.0f;
					int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y-32, (float)Math.Cos(ang) * speed, (float)Math.Sin(ang) * speed, ProjectileID.CursedFlameHostile, 50 + Main.rand.Next(26), 4.0f, Main.myPlayer, npc.whoAmI, 0.25f);																
					Main.projectile[p].tileCollide = Main.expertMode ? false : true;
				}
			}
		}
	}
}