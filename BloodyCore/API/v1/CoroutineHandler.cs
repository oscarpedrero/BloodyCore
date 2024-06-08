using BepInEx.Unity.IL2CPP.Utils.Collections;
using ProjectM.Physics;
using System;
using System.Collections;
using UnityEngine;

namespace Bloody.Core.API.v1
{
    public class CoroutineHandler
    {

        private static IgnorePhysicsDebugSystem coroutineManager = (new GameObject("CoroutineBloodyCore")).AddComponent<IgnorePhysicsDebugSystem>();

        // Method to get the CoroutineManager
        private static IgnorePhysicsDebugSystem GetCoroutineManager()
        {
            if (coroutineManager == null)
            {
                throw new InvalidOperationException("CoroutineManager not initialized.");
            }
            return coroutineManager;
        }

        // Method to start a generic coroutine with delay in seconds
        public static Coroutine StartGenericCoroutine(Action action, float delay)
        {
            return GetCoroutineManager().StartCoroutine(GenericCoroutine(action, delay).WrapToIl2Cpp());
        }

        // Generic coroutine with delay in seconds
        private static IEnumerator GenericCoroutine(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }

        // Method to start a repeating coroutine with delay in seconds
        public static Coroutine StartRepeatingCoroutine(Action action, float delay)
        {
            return GetCoroutineManager().StartCoroutine(RepeatingCoroutine(action, delay).WrapToIl2Cpp());
        }

        // Repeating coroutine with delay in seconds
        private static IEnumerator RepeatingCoroutine(Action action, float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                action?.Invoke();
            }
        }

        // Method to start a repeating coroutine with delay in seconds and a specific number of repetitions
        public static Coroutine StartRepeatingCoroutine(Action action, float delay, int repeatCount)
        {
            return GetCoroutineManager().StartCoroutine(RepeatingCoroutine(action, delay, repeatCount).WrapToIl2Cpp());
        }

        // Repeating coroutine with delay in seconds and a specific number of repetitions
        private static IEnumerator RepeatingCoroutine(Action action, float delay, int repeatCount)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                yield return new WaitForSeconds(delay);
                action?.Invoke();
            }
        }

        // Method to start a coroutine with frame intervals
        public static Coroutine StartFrameCoroutine(Action action, int frameInterval)
        {
            return GetCoroutineManager().StartCoroutine(FrameCoroutine(action, frameInterval).WrapToIl2Cpp());
        }

        // Coroutine that executes every x frames indefinitely
        private static IEnumerator FrameCoroutine(Action action, int frameInterval)
        {
            while (true)
            {
                for (int i = 0; i < frameInterval; i++)
                {
                    yield return null; // Waits for one frame
                }
                action?.Invoke();
            }
        }

        // Method to start a coroutine with frame intervals and a specific number of repetitions
        public static Coroutine StartFrameCoroutine(Action action, int frameInterval, int repeatCount)
        {
            return GetCoroutineManager().StartCoroutine(FrameCoroutine(action, frameInterval, repeatCount).WrapToIl2Cpp());
        }

        // Coroutine that executes every x frames, a specific number of times
        private static IEnumerator FrameCoroutine(Action action, int frameInterval, int repeatCount)
        {
            for (int j = 0; j < repeatCount; j++)
            {
                for (int i = 0; i < frameInterval; i++)
                {
                    yield return null; // Waits for one frame
                }
                action?.Invoke();
            }
        }

        // Method to start a coroutine with a random delay between two intervals in seconds
        public static Coroutine StartRandomIntervalCoroutine(Action action, float minDelay, float maxDelay)
        {
            return GetCoroutineManager().StartCoroutine(RandomIntervalCoroutine(action, minDelay, maxDelay).WrapToIl2Cpp());
        }

        // Coroutine that executes with a random delay between two intervals in seconds
        private static IEnumerator RandomIntervalCoroutine(Action action, float minDelay, float maxDelay)
        {
            while (true)
            {
                float delay = UnityEngine.Random.Range(minDelay, maxDelay);
                yield return new WaitForSeconds(delay);
                action?.Invoke();
            }
        }

        // Method to start a coroutine with a random delay between two intervals in seconds with a specific number of repetitions
        public static Coroutine StartRandomIntervalCoroutine(Action action, float minDelay, float maxDelay, int repeatCount)
        {
            return GetCoroutineManager().StartCoroutine(RandomIntervalCoroutine(action, minDelay, maxDelay, repeatCount).WrapToIl2Cpp());
        }

        // Coroutine that executes with a random delay between two intervals in seconds with a specific number of repetitions
        private static IEnumerator RandomIntervalCoroutine(Action action, float minDelay, float maxDelay, int repeatCount)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                float delay = UnityEngine.Random.Range(minDelay, maxDelay);
                yield return new WaitForSeconds(delay);
                action?.Invoke();
            }
        }
    }
}