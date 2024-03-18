 using System;
using System.Collections.Generic;
using System.Text;

namespace CommsLib
{
    /// <summary>
    /// Public enumerable containing information about packet headers
    /// </summary>
    public enum CommandID : int
    {
        Handshake = 0x01,
        ServerRemoteControlModeOn = 0x02,
        ServerRemoteControlModeOff = 0x03,
        Data = 0x04,
        TextBoxContents = 0x05,
        ServerEchoData = 0x06,
    };
}
