using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TrueEater.NPCs
{
	public class AntyFlybaby : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anty");
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 35;
			npc.damage = 6;
			npc.defense = 0;
			npc.knockBackResist = 0.65f;
			npc.width = 50;
			npc.height = 28;
			npc.aiStyle = 44;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath34;
			npc.noGravity = true;
			npc.noTileCollide = false;
			npc.value = 5;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 4;
			npc.npcSlots = 0.8f;
		}

		public override void NPCLoot()
		{
			if(Main.rand.Next(5) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AntLeg"), 1);
			}
			if(Main.rand.Next(8) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.AntlionMandible, 1+Main.rand.Next(2));
			}
		}

		public override void PostAI()
		{
			if(Main.rand.Next(20) == 0)
			{
				npc.velocity *= 1.05f;
			}
		}

		private int updateTime = 0;
		
		public override void FindFrame(int frameHeight)
		{
			++updateTime;
			npc.spriteDirection = npc.direction;
			npc.frame.Y = (int)(frameHeight * (int)((updateTime % 16) / 4));
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
        	float ret = 0.4f;
            return Main.player[Main.myPlayer].ZoneDesert ? ret * TrueEater.GetSpawnModifier(npc,spawnInfo) : 0f;
        }
	}
}