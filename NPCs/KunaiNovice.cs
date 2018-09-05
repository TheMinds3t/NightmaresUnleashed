using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TrueEater.NPCs
{
	public class KunaiNovice : ModNPC
	{
		private int updateTime = 0;
		private int attackTime = -100;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kunai Novice");
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 130;
			npc.damage = 4;
			npc.defense = 2;
			npc.width = 34;
			npc.height = 46;
			npc.aiStyle = 3;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 150;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 6;
		}
		
		public void fireKunai()
		{
			Player target = Main.player[Main.myPlayer];
			Vector2 off = new Vector2(0,0);
			float speed = 12.5f;
			double dire = Math.Atan2(((target.Center.Y - 64) - npc.Center.Y), (target.Center.X - npc.Center.X));
			Projectile.NewProjectile(npc.Center.X + off.X, npc.Center.Y + off.Y, (float)Math.Cos(dire) * speed, (float)Math.Sin(dire) * speed, mod.ProjectileType("NoviceKunai"), (int)(npc.damage * 1.25), 0f, Main.myPlayer, npc.whoAmI, 0.25f);		
		}
		
		public override void PostAI()
		{
			++attackTime;
			
			if (attackTime >= 20)
			{
				fireKunai();
				attackTime = -110;
			}
			
			if (attackTime >= 0)
			{
				npc.velocity.X *= 0.6f;
			}
			
			npc.velocity.X /= 1.0125f;
		}
		
		public override void FindFrame(int frameHeight)
		{
			++updateTime;
			npc.spriteDirection = npc.direction;
			
			if(attackTime > 0)
			{
				npc.frame.Y = frameHeight * 4;			
			}
			else if (npc.velocity.Y != 0.0f)
			{
				npc.frame.Y = frameHeight * 5;				
			}
			else
			{
				if (npc.velocity.X != 0.0f)
				{
					npc.frame.Y = frameHeight * ((int)(updateTime / 6) % 4);								
				}
				else
				{
					npc.frame.Y = frameHeight * 0;								
				}
			}
		}
		
		public override void NPCLoot()
		{
			if (Main.rand.Next(4) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SpitterKunai"), 3 + Main.rand.Next(10));
			}

			if (Main.rand.Next(100) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KunaiEmblem"), 1);
			}
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.spawnTileY < Main.rockLayer && !Main.dayTime ? 0.15f * TrueEater.GetSpawnModifier(npc,spawnInfo) : 0f;
        }
	}
}