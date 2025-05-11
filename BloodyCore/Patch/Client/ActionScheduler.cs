using HarmonyLib;
using ProjectM;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Bloody.Core.Patch.Client
{
    [HarmonyPatch(typeof(ClientBootstrapSystem), nameof(ClientBootstrapSystem.OnUpdate))]
    public static class ClientActionScheduler
    {
        public static int CurrentFrameCount = 0;
        public static Action OnGameFrameUpdate;
        public static ConcurrentQueue<Action> actionsToExecuteOnMainThread = new ConcurrentQueue<Action>();
        private static List<Timer> activeTimers = new List<Timer>();

        public static void Postfix()
        {
            OnGameFrameUpdate?.Invoke();

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

        public static void RunActionOnMainThread(Action action)
        {
            // Enqueue the action to be executed on the main thread
            actionsToExecuteOnMainThread.Enqueue(() =>
            {
                action.Invoke();  // Execute the action
            });
        }
    }
}
