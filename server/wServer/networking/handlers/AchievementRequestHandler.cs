
using System.Collections.Generic;

using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

using wServer.realm.entities;


namespace wServer.networking.handlers
{
	class AchievementRequestHandler : PacketHandlerBase<AchievementRequest>
	{
		public override PacketId ID => PacketId.ACHIEVEMENT;

		protected override void HandlePacket(Client client, AchievementRequest packet)
		{
			Handle(client, packet);
		}

		public class RewardData
		{
			public string ObjectId { get; set; }
			public int Total { get; set; }
		}

		public List<RewardData> RewardDatas { get; set; }

		public void showAchievements(Player player,int AchieveId) {

			var gifts = player.Client.Account.Gifts.ToList();


			switch (AchieveId)
			{
				case 0:
					RewardDatas = new List<RewardData>()
						{
							new RewardData() { ObjectId = "Master Eon", Total = 10 }
						};
						player.Manager.Database.UpdateFame(player.Client.Account, 100000);
						player.Manager.Database.UpdateCredit(player.Client.Account, 100000);
						

						for (var i = 0; i < RewardDatas.Count; i++)
							for (var j = 0; j < RewardDatas[i].Total; j++)
								gifts.Add(player.Manager.Resources.GameData.IdToObjectType[RewardDatas[i].ObjectId]);

						player.Manager.Chat.Announce("The Hero " + player.Name + " has completed the hidden achievement: 'Frog Hacks'");

						var notifications = new List<string>()
							{
								"Master Eon x 10",  "100,000 Gold", "100,000 Fame"

							};
						player.SendHelp("Go to your Gift Chest to claim the following rewards:");

						for (int i = 0; i < notifications.Count; i++)
							player.SendHelp($"- {notifications[i]}");
						player.UpdateCount++;
						player.Client.Account.Gifts = gifts.ToArray();
						player.Client.Account.FlushAsync();
						player.Client.Account.Reload();
					break;
				case 1:
					if (player.CurrentFame>=10000000)
					{
						RewardDatas = new List<RewardData>()
						{
							new RewardData() { ObjectId = "Potion of Attack", Total = 10 }
						};
						for (var i = 0; i < RewardDatas.Count; i++)
							for (var j = 0; j < RewardDatas[i].Total; j++)
								gifts.Add(player.Manager.Resources.GameData.IdToObjectType[RewardDatas[i].ObjectId]);
	
						player.Manager.Chat.Announce("The Hero " + player.Name + " has completed the achievement: 'Fame 10000000' ");

						var chicken = new List<string>()
							{
								"Potion of Attack x 10", "A Dildo"

							};
						player.SendHelp("Go to your Gift Chest to claim the following rewards:");

						for (int i = 0; i < chicken.Count; i++)
							player.SendHelp($"- {chicken[i]}");

						player.Client.Account.Gifts = gifts.ToArray();
						player.Client.Account.FlushAsync();
						player.Client.Account.Reload();
					}
					break;
			}
		}
		private bool once = false;
		private void Handle(Client client, AchievementRequest pkt)
		{
				switch (pkt.Name)
				{
				case 0:
					if (client.Player.Level == 20 && client.Player.Name == "Filisha" && once == false)
					{
						showAchievements(client.Player, 0);
						once = true;
					}
					else
					{
						client.Player.SendError("You do not meet the requirement or already met the requirement to claim this reward once!");
						return;
					}
					break;
				case 1:
					if (client.Player.Level == 20 && client.Player.CurrentFame >= 10000000 && once==false)
					{
						showAchievements(client.Player, 1);
						once = true;
					}
					else
					{
						client.Player.SendError("You do not meet the requirement or already met the requirement to claim this reward once!");
						return;
					}
					break;
			}
			
			
		}
	}
}