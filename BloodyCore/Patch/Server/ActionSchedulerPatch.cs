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
        public static int CurrentFrameCount = 0;
        public static ConcurrentQueue<Action> actionsToExecuteOnMainThread = new ConcurrentQueue<Action>();
        public static List<Timer> activeTimers = new List<Timer>();

        public static event RandomizedSpawnChainUpdateSystemEventHandler OnGameFrameUpdate; // TODO: Review why dont run

        [HarmonyPatch(typeof(RandomizedSpawnChainUpdateSystem), nameof(RandomizedSpawnChainUpdateSystem.OnUpdate))]
        [HarmonyPostfix]
        public static void Postfix()
        {
            OnGameFrameUpdate?.Invoke(); // TODO: Review why dont run
            CurrentFrameCount++;

            while (actionsToExecuteOnMainThread.TryDequeue(out Action action))
            {
                action?.Invoke();
            }
        }

        public static Timer RunActionEveryInterval(Action action, double intervalInSeconds)
        {
            return new Timer(_ =>
            {
                actionsToExecuteOnMainThread.Enqueue(action);
            }, null, TimeSpan.FromSeconds(intervalInSeconds), TimeSpan.FromSeconds(intervalInSeconds));
        }

        public static Timer RunActionOnceAfterDelay(Action action, double delayInSeconds)
        {
            Timer timer = null;

            timer = new Timer(_ =>
            {
                // Enqueue the action to be executed on the main thread
                actionsToExecuteOnMainThread.Enqueue(() =>
                {
                    action.Invoke();  // Execute the action
                    timer?.Dispose(); // Dispose of the timer after the action is executed
                });
            }, null, TimeSpan.FromSeconds(delayInSeconds), Timeout.InfiniteTimeSpan); // Prevent periodic signaling

            return timer;
        }

        public static Timer RunActionOnceAfterFrames(Action action, int frameDelay)
        {
            int startFrame = CurrentFrameCount;
            Timer timer = null;

            timer = new Timer(_ =>
            {
                if (CurrentFrameCount - startFrame >= frameDelay)
                {
                    // Enqueue the action to be executed on the main thread
                    actionsToExecuteOnMainThread.Enqueue(() =>
                    {
                        action.Invoke();  // Execute the action
                    });
                    timer?.Dispose();
                }
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(8));

            return timer;
        }

        public static Timer RunActionAtTime(Action action, DateTime scheduledTime)
        {
            // Calculate the delay in milliseconds from now until the scheduled time
            var now = DateTime.Now;
            var delay = scheduledTime - now;

            // If the scheduled time is in the past, execute immediately or adjust according to your needs
            if (delay.TotalMilliseconds < 0)
            {
                return null;
            }
            else
            {
                return RunActionOnceAfterDelay(action, delay.TotalSeconds);
            }
        }


        public static void RunActionOnMainThread(Action action)
        {
            // Enqueue the action to be executed on the main thread
            actionsToExecuteOnMainThread.Enqueue(() =>
            {
                action.Invoke();  // Execute the action
            });
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
