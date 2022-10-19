using System;

namespace wServer.realm.cores
{
    [Flags]
    public enum CoreType
    {
        /// <summary>
        /// Flag that represents a flush action for <see cref="CoreThread"/>
        /// at <see cref="CoreManager"/> to process client flush actions.
        /// </summary>
        Flush,

        /// <summary>
        /// Flag that represents a monitor action for <see cref="CoreThread"/>
        /// at <see cref="CoreManager"/> to process <see cref="ConnectManager"/>,
        /// <see cref="PortalMonitor"/> and <see cref="common.ISManager"/> monitors.
        /// </summary>
        Monitor,

        /// <summary>
        /// Flag that represents a world action for <see cref="CoreThread"/>
        /// at <see cref="worlds.World"/> to process a tick on world.
        /// </summary>
        World,

        /// <summary>
        /// Flag that represents a world logic action for <see cref="CoreThread"/>
        /// at <see cref="worlds.World"/> to process a logic tick on world.
        /// </summary>
        WorldLogic,

        /// <summary>
        /// Flag that represents a world timer action for <see cref="CoreThread"/>
        /// at <see cref="worlds.World"/> to process a timer tick on world.
        /// </summary>
        WorldTimer
    }
}
