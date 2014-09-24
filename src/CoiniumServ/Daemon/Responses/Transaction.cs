﻿#region License
// 
//     CoiniumServ - Crypto Currency Mining Pool Server Software
//     Copyright (C) 2013 - 2014, CoiniumServ Project - http://www.coinium.org
//     http://www.coiniumserv.com - https://github.com/CoiniumServ/CoiniumServ
// 
//     This software is dual-licensed: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
//    
//     For the terms of this license, see licenses/gpl_v3.txt.
// 
//     Alternatively, you can license this software under a commercial
//     license or white-label it as set out in licenses/commercial.txt.
// 
#endregion
using System.Collections.Generic;
using System.Linq;

namespace CoiniumServ.Daemon.Responses
{
    public class Transaction
    {
        public double Amount { get; set; } // seems it's set to 0 for immature transactions or generation transactions.

        /// <summary>
        /// As Amount may not always return the total transaction amount, TotalAmount calculates and return the value using transaction details
        /// </summary>
        public double TotalAmount { get { return Details.Sum(item => item.Amount); } }

        public int Confirmations { get; set; }

        public bool Generated { get; set; }

        public string BlockHash { get; set; }

        public int BlockIndex { get; set; }

        public int BlockTime { get; set; }

        public string TxId { get; set; }

        public string NormTxId { get; set; }

        public int Time { get; set; }
        public int TimeReceived { get; set; }

        public List<TransactionDetail> Details { get; set; }


        /// <summary>
        /// Returns the transaction detail that contains the output for pool.
        /// </summary>
        /// <param name="poolAddress"></param>
        /// <param name="poolAccount"></param>
        /// <param name="acceptFirstOutput"></param>
        /// <returns></returns>
        public TransactionDetail GetPoolOutput(string poolAddress, string poolAccount, bool acceptFirstOutput = false)
        {
            if (Details == null) // make sure we have valid outputs.
                return null;

            // kinda weird stuff goin here;
            // bitcoin variants;
            // case 1) some of bitcoin variants can include the "address" in the transaction detail and we can basically find the output comparing against it.
            // case 2) some other bitcoin variants can include "address account" name in transaction detail and we again find the output comparing against it.
            // case 3) peercoin variants is where things get complicated, even if you set an account name to an address, they peercoin variants will refuse use the name in details. 
            //         for peercoin variants, acceptFirstOutput parameter can make it work by just returning the very first output of the transaction.

            // check for case 1.
            if (Details.Any(x => x.Address == poolAddress))
                return Details.First(x => x.Address == poolAddress); // return the output that matches pool address.

            // check for case 2.
            if (Details.Any(x => x.Account == poolAccount))
                return Details.First(x => x.Account == poolAccount); // return the output that matches pool account.

            // case 3 - if we can't match pool address or pool account, just return the very first output given that acceptFirstOutput is true.
            if (acceptFirstOutput)
                return Details.FirstOrDefault();
            
            return null;
        }

        // not sure if fields below even exists / used
        //public double Fee { get; set; }
        //public string Comment { get; set; }
        //public string To { get; set; }        
    }
}
