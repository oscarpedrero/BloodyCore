using Bloody.Core.API;
using HarmonyLib;
using ProjectM;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bloody.Core.Patch.Server
{
    [HarmonyPatch]
    public class ActionSchedulerPatch
    {
        

        public static event RandomizedSpawnChainUpdateSystemEventHandler OnGameFrameUpdate; 

        [HarmonyPatch(typeof(RandomizedSpawnChainUpdateSystem), nameof(RandomizedSpawnChainUpdateSystem.OnUpdate))]
        [HarmonyPostfix]
        public static void Postfix()
        {
            OnGameFrameUpdate?.Invoke();
        }

        

        /* Como Usar en frames
        var action = () =>
        {
            var _tradeOutputBuffer = e.ReadBuffer<TradeOutput>();
            var _traderEntryBuffer = e.ReadBuffer<TraderEntry>();
            var _tradeCostBuffer = e.ReadBuffer<TradeCost>();
            _tradeOutputBuffer.Clear();
            _traderEntryBuffer.Clear();
            _tradeCostBuffer.Clear();

            var i = 0;
            foreach (var item in TraderItemDtos)
            {
                if (i > 25)
                {
                    break;
                }
                _tradeOutputBuffer.Add(new TradeOutput
                {
                    Amount = (ushort)item.OutputAmount,
                    Item = item.OutputItem,
                });

                _tradeCostBuffer.Add(new TradeCost
                {
                    Amount = (ushort)item.InputAmount,
                    Item = item.InputItem,
                });

                _traderEntryBuffer.Add(new TraderEntry
                {
                    RechargeInterval = -1,
                    CostCount = 1,
                    CostStartIndex = (byte)i,
                    FullRechargeTime = -1,
                    OutputCount = 1,
                    OutputStartIndex = (byte)i,
                    StockAmount = (ushort)item.StockAmount,
                });
                i++;
            }
        };
        ActionScheduler.RunActionOnceAfterFrames(action, 3); //delay clearing buffer until after it is populated 
        */
    }
    
}
