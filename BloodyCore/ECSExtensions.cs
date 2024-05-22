using System;
using System.Runtime.InteropServices;
using Bloodstone.API;
using Il2CppInterop.Runtime;
using ProjectM;
using Stunlock.Core;
using Unity.Entities;

namespace Bloody.Core;

#pragma warning disable CS8500
public static class ECSExtensions
{
	public unsafe static void Write<T>(this Entity entity, T componentData) where T : struct
	{
		// Get the ComponentType for T
		var ct = new ComponentType(Il2CppType.Of<T>());

		// Marshal the component data to a byte array
		byte[] byteArray = StructureToByteArray(componentData);

		// Get the size of T
		int size = Marshal.SizeOf<T>();

		// Create a pointer to the byte array
		fixed (byte* p = byteArray)
		{
            // Set the component data
            Core.SystemsCore.EntityManager.SetComponentDataRaw(entity, ct.TypeIndex, p, size);
		}
	}

	// Helper function to marshal a struct to a byte array
	public static byte[] StructureToByteArray<T>(T structure) where T : struct
	{
		int size = Marshal.SizeOf(structure);
		byte[] byteArray = new byte[size];
		IntPtr ptr = Marshal.AllocHGlobal(size);

		Marshal.StructureToPtr(structure, ptr, true);
		Marshal.Copy(ptr, byteArray, 0, size);
		Marshal.FreeHGlobal(ptr);

		return byteArray;
	}

	public unsafe static T Read<T>(this Entity entity) where T : struct
	{
		// Get the ComponentType for T
		var ct = new ComponentType(Il2CppType.Of<T>());

		// Get a pointer to the raw component data
		void* rawPointer = Core.SystemsCore.EntityManager.GetComponentDataRawRO(entity, ct.TypeIndex);

		// Marshal the raw data to a T struct
		T componentData = Marshal.PtrToStructure<T>(new IntPtr(rawPointer));

		return componentData;
	}

	public static DynamicBuffer<T> ReadBuffer<T>(this Entity entity) where T : struct
	{
		return Core.SystemsCore.EntityManager.GetBuffer<T>(entity);
	}

	public static void Add<T>(this Entity entity)
	{
		var ct = new ComponentType(Il2CppType.Of<T>());
        Core.SystemsCore.EntityManager.AddComponent(entity, ct);
	}

	public static void Remove<T>(this Entity entity)
	{
		var ct = new ComponentType(Il2CppType.Of<T>());
        Core.SystemsCore.EntityManager.RemoveComponent(entity, ct);
	}

	public static bool Has<T>(this Entity entity)
	{
		var ct = new ComponentType(Il2CppType.Of<T>());
		return Core.SystemsCore.EntityManager.HasComponent(entity, ct);
	}

    public static void LogComponentTypes(this Entity entity)
    {
        var comps = Core.SystemsCore.EntityManager.GetComponentTypes(entity);
        foreach (var comp in comps)
        {
            Core.Logger.LogDebug($"{comp}");
        }
		Core.Logger.LogDebug("===");
    }

    public static void LogComponentTypes(this EntityQuery entityQuery)
    {
        var types = entityQuery.GetQueryTypes();
        foreach (var t in types)
        {
			Core.Logger.LogDebug($"Query Component Type: {t}");
        }
		Core.Logger.LogDebug($"===");
    }

    public static string LookupName(this PrefabGUID prefabGuid)
	{
		return (Core.SystemsCore.PrefabCollectionSystem.PrefabGuidToNameDictionary.ContainsKey(prefabGuid)
			? Core.SystemsCore.PrefabCollectionSystem.PrefabGuidToNameDictionary[prefabGuid]: "GUID Not Found").ToString();
	}
}
#pragma warning restore CS8500
