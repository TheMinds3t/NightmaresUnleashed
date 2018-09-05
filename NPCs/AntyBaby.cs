using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TrueEater.NPCs
{
	public class AntyBaby : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anty");
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 40;
			npc.damage = 4;
			npc.defense = 1;
			npc.knockBackResist = 0.3f;
			npc.width = 50;
			npc.height = 28;
			npc.aiStyle = 3;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath34;
			npc.noGravity = false;
			npc.noTileCollide = false;
			npc.value = 5;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 5;
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
			if (npc.ai[0] == 0)
			{
				npc.scale = 1.0f + Main.rand.NextFloat() * 0.25f;
			}

			++npc.ai[0];
			npc.TargetClosest(true);
		}

		private int updateTime = 0;
		
		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			npc.frame.Y = (int)(frameHeight * (int)((updateTime % 25) / 5));
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
        	float ret = 0.4f;
            return Main.player[Main.myPlayer].ZoneDesert ? ret * TrueEater.GetSpawnModifier(npc,spawnInfo) : 0f;
        }
	}
}