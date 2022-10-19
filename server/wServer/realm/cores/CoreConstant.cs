namespace wServer.realm.cores
{
    /// <summary>
    /// Utilities for core constants.
    /// </summary>
    public struct CoreConstant
    {
        /// <summary>
        /// Represents the rate of fire for all enemies <see cref="logic.behaviors.Shoot"/>
        /// behaviors.
        /// <para>
        /// To setup a prodlike rate of fire for enemies, use value "2f" on <see cref="enemyRateOfFire"/>.
        /// </para>
        /// </summary>
        public const float enemyRateOfFire = 1f;

        /// <summary>
        /// Represents in milliseconds, 1 tick of flush task.
        /// </summary>
        public const int flushTickMs = 50;

        /// <summary>
        /// Represents in milliseconds, 1 tick of monitor task.
        /// </summary>
        public const int monitorTickMs = 200;

        /// <summary>
        /// Represents in milliseconds, 1 tick of pending packet task.
        /// </summary>
        public const int packetTickMs = 20;

        /// <summary>
        /// Represents in seconds the elapsed time required to close the realm.
        /// </summary>
        public const int realmCloseInSeconds = 1800;

        /// <summary>
        /// Represents in milliseconds the elapsed time required to close the world.
        /// </summary>
        public const int worldCloseInMilliseconds = 60000;

        /// <summary>
        /// Represents in milliseconds, 1 tick of world logic task.
        /// </summary>
        public const float worldLogicTickMs = 50;

        /// <summary>
        /// Represents in milliseconds, 1 tick of world and timer tasks.
        /// </summary>
        public const float worldTickMs = 200;
    }
}
