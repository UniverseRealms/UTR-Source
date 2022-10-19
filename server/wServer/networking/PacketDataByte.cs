namespace wServer.networking
{
    /// <summary>
    /// Represents an enum of packets data bytes identified through
    /// some network tests.
    ///
    /// <para>
    /// Note: don't change values of these parameters, it could result in further issues along server-side protection.
    /// </para>
    /// </summary>
    public enum PacketDataByte : int
    {
        /// <summary>
        /// Burst of bytes that represents a pending Policy Server request file once a web request is processed on client-side.
        /// </summary>
        RequestPolicyFile = 1014001516,

        /// <summary>
        /// Burst of bytes that represents a failed connection that by-passed <see cref="RequestPolicyFile"/> process.
        /// This is usually called when all server-side protection was aborted during Policy Server request file.
        /// </summary>
        UntrustedConnection = 352,

        /// <summary>
        /// Burst of bytes that represents a successfully connection after <see cref="RequestPolicyFile"/> process.
        /// </summary>
        TrustedConnection = 273
    }
}
