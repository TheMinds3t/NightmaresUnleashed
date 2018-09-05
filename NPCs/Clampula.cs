using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TrueEater.NPCs
{
	[AutoloadBossHead]
	public class Clampula : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clampula");
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 2200;
			npc.damage = 40;
			npc.defense = 5;
			npc.knockBackResist = 0.0f;
			npc.width = 150;
			npc.height = 112;
			npc.aiStyle = 3;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 50000;
			npc.netAlways = true;
			npc.boss = true;
			npc.timeLeft = 22400;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(2200 * (1.2f + numPlayers / 10.0f));
			npc.damage = (int)(npc.damage * 0.75f);
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			TrueWorld.downedClampula = true;
			potionType = ItemID.HealingPotion;
			if (Main.expertMode == false)
			{
				int item = Main.rand.Next(6);
				if (item == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SharkFin, 2 + Main.rand.Next(5));
				}
				else if (item == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.BlackInk, 2 + Main.rand.Next(5));
				}
				else if (item == 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Seashell, 2 + Main.rand.Next(5));
				}
				else if (item == 3)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Ruby, 2 + Main.rand.Next(5));
				}
				else if (item == 4)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Sapphire, 2 + Main.rand.Next(5));
				}
				else if (item == 5)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Amethyst, 2 + Main.rand.Next(5));
				}

				item = Main.rand.Next(5);
				if (item == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Starfish, 2 + Main.rand.Next(5));
				}
				else if (item == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Glowstick, 2 + Main.rand.Next(5));
				}
				else if (item == 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Diamond, 2 + Main.rand.Next(5));
				}
				else if (item == 3)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Emerald, 2 + Main.rand.Next(5));
				}
				else if (item == 4)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Topaz, 2 + Main.rand.Next(5));
				}

				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Coral, 12 + Main.rand.Next(11));
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ClampulaPearl"), 1);
				
				if(Main.rand.Next(10) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ClampulaTrophy"));
				}
			}
			else
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ClampulaBag"));
			}
		}

		public void fire()
		{
			Player target = getClosestPlayer();
			//TrueEater.PlaySound(3, npc);
			Vector2 off = new Vector2(0,-npc.height / 2+4);
			float speed = 3.5f;
			double dire = Math.Atan2((target.Center.Y - (npc.Center.Y + off.Y)), (target.Center.X - (npc.Center.X + off.X)));
			Projectile.NewProjectile(npc.Center.X + off.X, npc.Center.Y + off.Y, (float)Math.Cos(dire) * speed, (float)Math.Sin(dire) * speed, mod.ProjectileType("ClamPearl"), (int)(npc.damage * 0.25f), 5.5f, Main.myPlayer, npc.whoAmI, 0.25f);		
			dire = dire + (double)((Main.rand.NextFloat() * 90.0f - 45.0f) / (180.0f / 3.14f)); speed = 3.5f + Main.rand.NextFloat() * 2.0f - 1.0f;
			Projectile.NewProjectile(npc.Center.X + off.X, npc.Center.Y + off.Y, (float)Math.Cos(dire) * speed, (float)Math.Sin(dire) * speed, mod.ProjectileType("ClamPearl"), (int)(npc.damage * 0.25f), 5.5f, Main.myPlayer, npc.whoAmI, 0.25f);		
			dire = dire + (double)((Main.rand.NextFloat() * 90.0f - 45.0f) / (180.0f / 3.14f)); speed = 3.5f + Main.rand.NextFloat() * 2.0f - 1.0f;
			Projectile.NewProjectile(npc.Center.X + off.X, npc.Center.Y + off.Y, (float)Math.Cos(dire) * speed, (float)Math.Sin(dire) * speed, mod.ProjectileType("ClamPearl"), (int)(npc.damage * 0.25f), 5.5f, Main.myPlayer, npc.whoAmI, 0.25f);		
			dire = dire + (double)((Main.rand.NextFloat() * 90.0f - 45.0f) / (180.0f / 3.14f)); speed = 3.5f + Main.rand.NextFloat() * 2.0f - 1.0f;
			Projectile.NewProjectile(npc.Center.X + off.X, npc.Center.Y + off.Y, (float)Math.Cos(dire) * speed, (float)Math.Sin(dire) * speed, mod.ProjectileType("ClamPearl"), (int)(npc.damage * 0.25f), 5.5f, Main.myPlayer, npc.whoAmI, 0.25f);		
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
						if ((Math.Abs(distanceRet.X) + Math.Abs(distanceRet.Y)) > (Math.Abs(distance.X) + Math.Abs(distance.Y)))
						{
							ret = p;
						}
					}
				}
			}
			
			return ret;
		}
		
		private int updateTime = 0;
		
		public override void PostAI()
		{
			++updateTime;
			
			if (updateTime % 60 == 0)
			{
				Player targ = getClosestPlayer();

				if ((Math.Abs(targ.Center.X - npc.Center.X) + Math.Abs(targ.Center.Y - npc.Center.Y)) / 2 < 2000)
				{
					float d = targ.Center.X - npc.Center.X;
					if (d > 1000.0f)
					{
						d = 1000.0f;
					}
					if (d < -1000.0f)
					{
						d = -1000.0f;
					}

					npc.velocity.X += d / (100.0f);
				}
				else
				{
					npc.velocity.X += Main.rand.NextFloat() * 10.0f - 5.0f;
				}
				npc.velocity.Y += Main.rand.NextFloat() * -10.0f - 10.0f;
			}

			if (updateTime % 60 == 30)
			{
				npc.velocity.Y -= Main.rand.NextFloat() * -10.0f - 10.0f;
			}

			if (updateTime % 60 == 0 && getClosestPlayer() != null && TrueEater.FireHostileProj())
			{
				if (Main.expertMode)
				{
					fire();
					fire();
					fire();
					fire();
				}
				
				fire();
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			npc.frame.Y = (int)(frameHeight * (int)((updateTime % 40) / 10));
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			float ret = 1.0f / 90.0f;
			if (Main.bloodMoon) {ret += 0.1f;}
			if (Main.hardMode) {ret += 0.1f;}
			if (Main.expertMode) {ret += 10.0f / 90.0f;}
			ret /= 2.0f;
            return 0.0f;//spawnInfo.spawnTileY < Main.rockLayer && !Main.dayTime ? ret : 0f;
        }
	}
}