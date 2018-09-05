using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TrueEater.NPCs
{
	[AutoloadBossHead]
	public class Implord : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Imp Lord");
		}

		public List<int> balls = new List<int>();

		public override void SetDefaults()
		{
			npc.lifeMax = 3000;
			npc.damage = 20;
			npc.defense = 1;
			npc.knockBackResist = 0.0f;
			npc.width = 50;
			npc.height = 80;
			npc.aiStyle = -1;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 50000;
			npc.boss = true;
			npc.timeLeft = 22500;
			npc.scale = 1.5f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 5;
			music = MusicID.Boss2;
		}
		
		public override void PostAI()
		{
			++npc.ai[0];
			if (npc.target < 0 || !Main.player[npc.target].active || Main.player[npc.target].statLife <= 0)
			{
				npc.TargetClosest(true);
				++npc.ai[1];

				if(npc.ai[1] >= 12)
				{
					npc.Teleport(new Vector2(64,64));
					npc.timeLeft = 2;
				}

			}
			else
			{
				if(npc.ai[0] % 60 == 0) {npc.TargetClosest(true);}
				npc.ai[1] = 0;
				Player player = Main.player[npc.target];
				if(npc.ai[0] % 400 == 1)
				{
					float ang = (Main.rand.Next(360)) * 3.14f / 180.0f;
					npc.Teleport(new Vector2(player.Center.X + (float)Math.Cos(ang) * 360.0f,player.Center.Y + (float)Math.Sin(ang) * 360.0f));
				}

				int num_balls = 12 + (Main.expertMode ? 8 : 0);


				if(balls.Count < num_balls && npc.ai[0] % 20 == 0 && Main.netMode != 1)
				{
					int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0.0f, 0.0f, ProjectileID.Fireball, (int)(npc.damage * 1.5f * (Main.expertMode ? 0.5f : 1.0f)), 5.5f, npc.target, 0, 0);	
					Main.projectile[p].timeLeft = npc.ai[0] % (40 * num_balls) < 20 * num_balls ? 500 : 800;
					Main.projectile[p].tileCollide = false;
					//Main.projectile[p].owner = npc.whoAmI;
					balls.Add(p);//Insert(0,p);
				}

				npc.direction = player.Center.X > npc.Center.X ? 1 : -1;

				for(int i = 0; i < balls.Count; ++i)
				{
					Projectile p = Main.projectile[balls[i]];
					float loc = ((360.0f / num_balls) * i) * (3.14f / 180.0f);
					float size = 200.0f + (float)Math.Cos((float)(npc.ai[0]) / 30.0f) * 30.0f + (float)Math.Cos((float)(p.timeLeft) / 10.0f) * 60.0f;
					float targX = npc.Center.X + (float)Math.Cos(loc + (float)npc.ai[0] / 40.0f) * size;
					float targY = npc.Center.Y + (float)Math.Sin(loc + (float)npc.ai[0] / 40.0f) * size;
					p.velocity.X = (targX - p.Center.X) / (1.0f * 20.0f);
					p.velocity.Y = (targY - p.Center.Y) / (1.0f * 20.0f);
					if(p.timeLeft <= 300 || !p.active || p.type != ProjectileID.Fireball || npc.ai[0] % 400 == 0)
					{
						if(npc.ai[0] % 400 == 0) {p.timeLeft = 20-i;}
						targX = player.Center.X - p.Center.X;
						targY = player.Center.Y - p.Center.Y;
						p.velocity = new Vector2(targX,targY);
						p.velocity = Vector2.Normalize(p.velocity) * 12.0f;
						balls.RemoveAt(i);
					}
				}

				float moveX = player.Center.X - npc.Center.X;
				float moveY = player.Center.Y - npc.Center.Y;
				npc.velocity.X = moveX / (160.0f);
				npc.velocity.Y = moveY / (160.0f);
			}
		}

		public override void ModifyHitByProjectile 	(Projectile projectile,	ref int damage,	ref float knockback,ref bool crit,ref int hitDirection)
		{
			if(projectile.type == ProjectileID.ImpFireball) {damage = 0;}
		} 	
		
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.HealingPotion;
			int[] loot1 = {-1,-1,-1,ItemID.ObsidianRose,ItemID.MagmaStone,ItemID.Flamelash};
			int get1 = Main.rand.Next(6);

			if(loot1[get1] != -1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, loot1[get1], 1);
			}
		}
		
		public override void FindFrame(int frameHeight)
		{
			++npc.frameCounter;
			npc.spriteDirection = npc.direction;
			npc.frame.Y = frameHeight * (int)((npc.frameCounter % 50) / 10);
		}
	}
}