using common.resources;
using System.IO;
using System.Threading.Tasks;
using wServer.networking;
using wServer.realm.cores;
using wServer.realm.entities;
using wServer.realm.setpieces;

namespace wServer.realm.worlds.logic
{
    public class Realm : World
    {
        private Oryx _overseer;

        private readonly bool _oryxPresent;
        private readonly int _mapId;
        private Task _overseerTask;

  
        public int closeTime = CoreConstant.realmCloseInSeconds;

        public Realm(ProtoWorld proto, Client client = null) : base(proto)
        {
            _oryxPresent = true;
            _mapId = 1;
        }

        public override bool AllowedAccess(Client client)
        {
            // since map gets reset, admins not allowed to join when closed. Possible to crash server otherwise.
            return !Closed && base.AllowedAccess(client);
        }

        protected override void Init()
        {
            FromWorldMap(new MemoryStream(Manager.Resources.Worlds["Realm"].wmap[_mapId - 1]));
            SetPieces.ApplySetPieces(this);
            
            if (_oryxPresent)
            {
                _overseer = new Oryx(this);
                _overseer.Init();
            }
        }

        public override void Tick()
        {
            if (Closed) Manager.Monitor.ClosePortal(Id);
            else Manager.Monitor.OpenPortal(Id);

            base.Tick();

            if (Limbo || Deleted) return;
            if (_overseerTask == null || _overseerTask.IsCompleted)
            {
                _overseerTask = Task.Factory.StartNew(() =>
                {
                    var secondsElapsed = Program.Uptime.ElapsedMilliseconds / 1000;

                    /*if (secondsElapsed > 10 && secondsElapsed % closeTime < 60 && !IsClosing())
                        CloseRealm();*/

                    if (!IsClosing() && EventsKilled >= 15)
                        CloseRealm();

                    if (Closed && Players.Count == 0 && _overseer != null)
                    {
                        Init(); // will reset everything back to the way it was when made
                        Closed = false;
                    }

                    _overseer?.Tick();
                }).ContinueWith(e =>
                    Program.Debug(typeof(Realm), e.Exception.ToString(), true),
                    TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        public void EnemyKilled(Enemy enemy, Player killer)
        {
            if (_overseer != null && !enemy.Spawned)
            {
                _overseer.OnEnemyKilled(enemy, killer);
            }   
        }

        public override int EnterWorld(Entity entity)
        {
            var ret = base.EnterWorld(entity);
            var player = entity as Player;
            if (player != null)
                _overseer?.OnPlayerEntered(player);
            return ret;
        }

        public bool CloseRealm()
        {
            if (_overseer == null)
                return false;
            closeTime = CoreConstant.realmCloseInSeconds + 30; //60
            _overseer.InitCloseRealm();
            
            return true;
        }

        public bool IsClosing()
        {
            if (_overseer == null)
                return false;

            return _overseer.Closing;
        }
    }
}
