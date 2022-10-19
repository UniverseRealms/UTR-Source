using common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using wServer.networking.packets;
using log4net;
using wServer.realm;
using common.resources;
using common;
using wServer.realm.entities;
namespace wServer.networking.packets.incoming
{
    public class EditAccountList : IncomingMessage
    {
        private static readonly Player player;
        public int AccountListId { get; set; }
        public bool Add { get; set; }
        public int ObjectId { get; set; }

        public override PacketId ID => PacketId.EDITACCOUNTLIST;
        public override Packet CreateInstance() { return new EditAccountList(); }

        protected override void Read(NReader rdr)
        {
            try
            {
                AccountListId = rdr.ReadInt32();
                Add = rdr.ReadBoolean();
                ObjectId = rdr.ReadInt32();

            }
            catch (Exception)
            {
                player.Client.Disconnect("Packet Read: EndOfStreamException");
            }
        }

        protected override void Write(NWriter wtr)
        {
            try
            {
                wtr.Write(AccountListId);
                wtr.Write(Add);
                wtr.Write(ObjectId);
            }
            catch (Exception)
            {
                player.Client.Disconnect("k.");
            }
        }
    }
}