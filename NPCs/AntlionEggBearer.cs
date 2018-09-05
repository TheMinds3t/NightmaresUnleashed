using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TrueEater.NPCs
{
	public class AntlionEggBearer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Antlion Queen");
		}
		public override void SetDefaults()
		{
			npc.lifeMax = 1200;
			npc.damage = 10;
			npc.defense = 1;
			npc.knockBackResist = 0.0f;
			npc.width = 34;
			npc.height = 46;
			npc.aiStyle = 0;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.noGravity = false;
			npc.noTileCollide = false;
			npc.value = 25000;
			npc.HitSound = SoundID.NPCHit31;
			npc.DeathSound = SoundID.NPCDeath34;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 5;
			npc.boss = true;
		}
		
		private int updateTime = 0;

		public override void PostAI()
		{
			++npc.ai[2];
			
			if (npc.ai[2] % (Main.expertMode ? 90 - (Main.player[Main.myPlayer].statLifeMax / 500 * 80) : 100 - (Main.player[Main.myPlayer].statLifeMax / 500 * 80)) == 0)
			{
				int type = mod.NPCType("AntyBaby");
				int ai = 0;

				if (Main.rand.Next(2) == 0)
				{
					type = mod.NPCType("AntyFlybaby");
					ai = 1;
				}

				if (npc.ai[ai] < (Main.expertMode ? 6 : 4))
				{
					int np = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, type, 0, npc.whoAmI, npc.Center.X, npc.Center.Y);
					Main.npc[np].ai[0] = npc.whoAmI;
					npc.ai[ai] += 1;
				}
			}
		}
		
		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			npc.frame.Y = (int)(frameHeight * (int)((updateTime % 50) / 10));
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.LesserHealingPotion;
			TrueWorld.downedAntlionQueen = true;
			if (Main.expertMode == false)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.AntlionMandible, 1 + Main.rand.Next(2));

				if (Main.rand.Next(20) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AntlionFood"), 1);
				}

				int item = Main.rand.Next(5);
				int ret = -1;

				switch(item)
				{
					case 0: ret = mod.ItemType("AntlionEggSack"); break;
					case 1: ret = mod.ItemType("AntlionHead"); break;
					case 2: ret = mod.ItemType("AntlionChest"); break;
					case 3: ret = mod.ItemType("AntlionLegs"); break;
					default: ret = ItemID.Sandgun; break;
				}

				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ret, 1);
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AntLeg"), 3 + Main.rand.Next(4));
				
				if(Main.rand.Next(10) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AntlionQueenTrophy"));
				}
			}
			else
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AntQueenBag"));
			}
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			float ret = 1.0f / 250.0f;
			if (Main.bloodMoon) {ret += 0.05f;}
			if (Main.hardMode) {ret += 0.05f;}
			if (Main.expertMode) {ret += 9.0f / 250.0f;}
			ret /= 2.0f;
            return 0f;//spawnInfo.spawnTileY < Main.rockLayer && !Main.dayTime ? ret : 0f;
        }
	}
}