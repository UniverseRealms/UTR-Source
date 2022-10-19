/*
    Copyright (C) 2015 creepylava

    This file is part of RotMG Dungeon Generator.

    RotMG Dungeon Generator is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using DungeonGenerator.Dungeon;
using RotMG.Common;

namespace DungeonGenerator.Templates.LostHalls
{
	public class LostHallsTemplate : DungeonTemplate
    {
        internal static bool oneSpecialRoom = false;
		internal static readonly TileType Space = new TileType(0x00fe, "Space");
        internal static readonly TileType LHMainTile = new TileType(0xb01a, "LH Main Tile");
        internal static readonly TileType LHMainTileBloody = new TileType(0xb01b, "LH Main Tile Bloody");
        internal static readonly TileType LHHallTile = new TileType(0xb01d, "LH Hall Tile");
        internal static readonly TileType LHHallTileBloody = new TileType(0xb039, "LH Hall Tile Bloody");
        internal static readonly TileType LHGemTile = new TileType(0xb01c, "LH Gem Tile");
        internal static readonly TileType LHHoleTile = new TileType(0xb01e, "LH Hole Tile");
        internal static readonly TileType LHIceTile = new TileType(0xb038, "LH Ice Tile");
        internal static readonly TileType LHWaterTile = new TileType(0xb038, "LH Water Tile");

        internal static readonly ObjectType LHMainWall = new ObjectType(0xb021, "LH Main Wall");
        internal static readonly ObjectType LHCandle1 = new ObjectType(0xb01e, "LH Lit Candle Wall 1");
        internal static readonly ObjectType LHCandle2 = new ObjectType(0xb01f, "LH Lit Candle Wall 2");
        internal static readonly ObjectType LHMarbleWall = new ObjectType(0xb095, "LH Marble Wall");
        internal static readonly ObjectType LHUnlitCandleWall = new ObjectType(0xb020, "LH Unlit Candle Wall");
        internal static readonly ObjectType LHCrackedWall = new ObjectType(0xb158, "LH Cracked Wall");
        internal static readonly ObjectType LHBossWall = new ObjectType(0xb149, "LH Boss Wall");
        internal static readonly ObjectType LHTrapWall = new ObjectType(0xb144, "LH Trap Wall");
        internal static readonly ObjectType LHVineWall = new ObjectType(0xb159, "LH Vine Wall");
        internal static readonly ObjectType LHFaceWall1 = new ObjectType(0xb15a, "LH Face Wall 1");
        internal static readonly ObjectType LHFaceWall2 = new ObjectType(0xb15b, "LH Face Wall 2");
        internal static readonly ObjectType LHWaterWall = new ObjectType(0xb15c, "LH Water Wall");
        internal static readonly ObjectType LHVoidWall = new ObjectType(0xb15d, "LH Void Wall");

        //Troom
        internal static readonly ObjectType LHAgonizedTitan = new ObjectType(0xb010, "LH Agonized Titan");
        internal static readonly ObjectType LHCultPortalLocked = new ObjectType(0xb062, "LH Cult Portal Locked");

        //NormalRoom
        internal static readonly ObjectType LHRemains = new ObjectType(0xb022, "LH Remains");

        //Lost Crusaders
        internal static readonly ObjectType Commander = new ObjectType(0xb003, "LH Commander of the Crusade");
        internal static readonly ObjectType Soldier = new ObjectType(0xb000, "LH Crusade Soldier");
        internal static readonly ObjectType Shipwright = new ObjectType(0xb001, "LH Crusade Shipwright");
        internal static readonly ObjectType Explorer = new ObjectType(0xb002, "LH Crusade Explorer");

        //Oryx Infanty
        internal static readonly ObjectType ChampionOfOryx = new ObjectType(0xb007, "LH Champion of Oryx");
        internal static readonly ObjectType OryxSwordsman = new ObjectType(0xb004, "LH Oryx Swordsman");
        internal static readonly ObjectType OryxArmorbearer = new ObjectType(0xb005, "LH Oryx Armorbearer");
        internal static readonly ObjectType OryxAdmiral = new ObjectType(0xb006, "LH Oryx Admiral");

        //Grotto Beasts
        internal static readonly ObjectType GrottoBlob = new ObjectType(0xb0b3, "LH Grotto Blob");

        //Lost Golems
        internal static readonly ObjectType TormentedGolem = new ObjectType(0xb00e, "LH Tormented Golem");
        internal static readonly ObjectType GolemOfAnger = new ObjectType(0xb00b, "LH Golem of Anger");
        internal static readonly ObjectType GolemOfFear = new ObjectType(0xb00c, "LH Golem of Fear");
        internal static readonly ObjectType GolemOfSorrow = new ObjectType(0xb00d, "LH Golem of Sorrow");

        //Others
        internal static readonly ObjectType SpectralSentry = new ObjectType(0xb00f, "LH Spectral Sentry");
        internal static readonly ObjectType Pot = new ObjectType(0xb0b5, "LH Pot 1");
        internal static readonly ObjectType Pot2 = new ObjectType(0xb0b6, "LH Pot 2");
        internal static readonly ObjectType Pot3 = new ObjectType(0xb0b7, "LH Pot 3");
        internal static readonly ObjectType Pot4 = new ObjectType(0xb0b8, "LH Pot 4");
        internal static readonly ObjectType Pot5 = new ObjectType(0xb0b9, "LH Pot 5");
        internal static readonly ObjectType Pot6 = new ObjectType(0xb0ba, "LH Pot 6");
        internal static readonly ObjectType Pot7 = new ObjectType(0xb0bb, "LH Pot 7");

        //StartRoom
        internal static readonly ObjectType CowardicePortal = new ObjectType(0x0703, "Portal of Cowardice");
        internal static readonly ObjectType LHSpawnPillar = new ObjectType(0xb160, "LH Spawn Pillar");
        internal static readonly ObjectType LHSpawnWallKiller = new ObjectType(0xb161, "LH Spawn Wall Killer");

        internal static readonly DungeonTile[,] MapTemplate;

		static LostHallsTemplate()
        {
			MapTemplate = ReadTemplate(typeof(LostHallsTemplate));
		}

		public override int MaxDepth { get { return 6; } }

		NormDist targetDepth;
		public override NormDist TargetDepth { get { return targetDepth; } }

        NormDist specialRmCount;
        public override NormDist SpecialRmCount { get { return specialRmCount; } }

        NormDist specialRmDepthDist;
        public override NormDist SpecialRmDepthDist { get { return specialRmDepthDist; } }

        public override Range RoomSeparation { get { return new Range(9, 9); } }

		public override int CorridorWidth { get { return 7; } }

        public override Room CreateSpecial(int depth, Room prev)
        {
            return new TreasureRoom();
        }

        public override void Initialize()
        {
            targetDepth = new NormDist(5, 5, 10, 15, Rand.Next());
            specialRmCount = new NormDist(1.5f, 0.5f, 6, 6, Rand.Next());
            specialRmDepthDist = new NormDist(20, 30, 7, 10, Rand.Next());
        }

		public override Room CreateStart(int depth)
        {
			return new StartRoom();
		}

		public override Room CreateTarget(int depth, Room prev)
        {
			return new BossRoom();
		}

		public override Room CreateNormal(int depth, Room prev)
        {
			return new NormalRoom(17, 17);
		}

		public override MapCorridor CreateCorridor()
        {
			return new Corridor();
		}

		public override MapRender CreateOverlay()
        {
			return new Overlay();
		}
    }
}