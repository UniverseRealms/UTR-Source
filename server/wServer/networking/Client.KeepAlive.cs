using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.networking
{
    public partial class Client
    {
        private long _pingTime = -1;
        private long _pongTime = -1;
        private int _serial;

        private const int PingPeriod = 3000;
        private const int DcThresold = 15000;

        public bool KeepAlive(int position, int count)
        {
            if (_pingTime == -1)
            {
                _pingTime = Manager.Core.getTotalTickCount() - PingPeriod;
                _pongTime = Manager.Core.getTotalTickCount();
            }

            // check for disconnect timeout
            if (Manager.Core.getTotalTickCount() - _pongTime > DcThresold)
            {
                Disconnect("Queue connection timeout. (KeepAlive)");
                return false;
            }

            if (Manager.Core.getTotalTickCount() - _pingTime < PingPeriod)
                return true;

            _pingTime = Manager.Core.getTotalTickCount();
            _serial = (int)_pingTime;

            SendPacket(new QueuePing()
            {
                Serial = _serial,
                Position = position,
                Count = count
            });

            return true;
        }

        public void Pong(QueuePong pongPkt)
        {
            if (pongPkt.Serial == _serial)
                _pongTime = Manager.Core.getTotalTickCount();
        }
    }
}
