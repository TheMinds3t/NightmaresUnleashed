using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TrueEater.NPCs
{
	public class Corrgoth : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrgoth");
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 4000;
			npc.damage = 100;
			npc.defense = 34;
			npc.knockBackResist = 0.0f;
			npc.width = 68;
			npc.height = 68;
			npc.aiStyle = 22;
			npc.HitSound = SoundID.NPCHit13;
			npc.DeathSound = SoundID.NPCDeath19;
			npc.value = 1500;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.scale = 2;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 4;
		}
		
		public override bool CheckDead()
		{
			if(npc.life <= 0)
			{
				for(int i = 0; i < Main.rand.Next(6) + 5; ++i)
				{
					int danger = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Shogeye"), 0, npc.whoAmI, npc.Center.X, npc.Center.Y);
					Main.npc[danger].velocity.X += Main.rand.Next(8) - 4.0f;
					Main.npc[danger].velocity.Y += Main.rand.Next(8) - 4.0f;
					Main.npc[danger].localAI[0] = 2.0f;
				}
			}
			
			return true;
		}
		
		private int updateTime = 0;
		
		public override void FindFrame(int frameHeight)
		{
			++updateTime;
			npc.spriteDirection = npc.direction;
			npc.frame.Y = frameHeight * (int)((updateTime % 40) / 10);
		}
		
		 public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			float ret = 1.0f / 1200.0f;
            return Main.player[Main.myPlayer].ZoneCorrupt && Main.hardMode ? ret * TrueEater.GetSpawnModifier(npc,spawnInfo) : 0f;
        }
		
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Gel, 5 + Main.rand.Next(6));
			
			if(Main.rand.Next(3) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CorrGothEye"), 1 + Main.rand.Next(2));
			}
		}
	}
}