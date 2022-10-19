using System;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;
using wServer.realm.worlds;

namespace wServer.networking.handlers
{
	class BountyRequestHandler : PacketHandlerBase<BountyRequest>
	{
		public override PacketId ID => PacketId.BOUNTYREQUEST;

		protected override void HandlePacket(Client client, BountyRequest packet)
		{
			Handle(client, packet);
		}

		// Item names of item needed for each tier bounty
		private string[] BountyKeys = { "The Magician Tarot Card", "The Sun Tarot Card", "Death Tarot Card", "The Devil Tarot Card" };

		private string EasyWrld = "Undead Lair";
		private string MedWrld = "Snake Pit";
		private string HardWrld = "Pirate Cave";
		private string Nightmare = "World Boss";
		void Handle(Client client, BountyRequest pkt)
		{
			var easy = client.Player.Owner.Manager.Resources.Worlds[EasyWrld];

			DynamicWorld.TryGetWorld(easy, client, out var world);
			world = client.Player.Owner.Manager.AddWorld(world ?? new World(easy));


			var med = client.Player.Owner.Manager.Resources.Worlds[MedWrld];
			var world2 = client.Player.Owner.Manager.AddWorld(new World(med));

			var hard= client.Player.Owner.Manager.Resources.Worlds[HardWrld];
			var world3 = client.Player.Owner.Manager.AddWorld(new World(hard));

			var hell = client.Player.Owner.Manager.Resources.Worlds[Nightmare];
			var world4 = client.Player.Owner.Manager.AddWorld(new World(hell));


			if (client.Player?.Owner == null)
				return;
			if (client.Account.GuildId <= 0 || client.Account.GuildRank < 30)
			{
				client.Player.SendError("You're not in a guild or your rank is too low!");
				return;
			}
			string reqItem = BountyKeys[pkt.BountyId];

			bool hasItem = false;

			for (int i = 4; i < client.Player.Inventory.Length; i++)
				if (client.Player.Inventory[i] != null)
					if (client.Player.Inventory[i].DisplayName == reqItem)
					{
						client.Player.Inventory[i] = null;
						hasItem = true;
					}
			if (!hasItem)
			{
				client.Player.SendError("You need a " + reqItem +" to start this bounty");
				return;
			}
			else
			if (reqItem.Equals("The Magician Tarot Card"))
			{
				{
					client.Player.SendInfo("Easy Bounty has started! Good Luck Heros!");

					foreach (var w in client.Player.Manager.Worlds.Values)
						foreach (var p in w.Players.Values)
							p.Client.Reconnect(new Reconnect
							{
								Host = "",
								Port = 2050,
								GameId = world.Id,
								Name = world.SBName,
								IsFromArena = false
							});

				}

			}
			else if (reqItem.Equals("The Sun Tarot Card"))
			{
				{

					client.Player.SendInfo("Medium Bounty has started! Good Luck Heros!");

					foreach (var w in client.Player.Manager.Worlds.Values)
						foreach (var p in w.Players.Values)
							p.Client.Reconnect(new Reconnect
							{
								Host = "",
								Port = 2050,
								GameId = world2.Id,
								Name = world2.SBName,
								IsFromArena = false
							});

				}

			}
			else if (reqItem.Equals("Death Tarot Card"))
			{
				{

					client.Player.SendInfo("Hard Bounty has started! Good Luck Heros!");

					foreach (var w in client.Player.Manager.Worlds.Values)
						foreach (var p in w.Players.Values)
							p.Client.Reconnect(new Reconnect
							{
								Host = "",
								Port = 2050,
								GameId = world3.Id,
								Name = world3.SBName,
								IsFromArena = false
							});

				}

			}
			else if (reqItem.Equals("The Devil Tarot Card"))
			{

				client.Player.SendInfo("Hell Bounty has started! Good Luck Heros!");

				foreach (var w in client.Player.Manager.Worlds.Values)
					foreach (var p in w.Players.Values)
						p.Client.Reconnect(new Reconnect
						{
							Host = "",
							Port = 2050,
							GameId = world4.Id,
							Name = world4.SBName,
							IsFromArena = false
						});

			}

		}

	}
}