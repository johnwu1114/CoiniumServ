﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coinium.Core.Coin.Daemon.Responses;

namespace Coinium.Core.Coin.Daemon
{
    public interface IDaemonClient
    {
        BlockTemplate GetBlockTemplate(params object[] @params);

        Work Getwork();

        bool Getwork(string data);

        ValidateAddress ValidateAddress(string walletAddress);

    }
}
