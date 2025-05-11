using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PluralizeService.Core;
using ProjectM;
using Unity.Entities;
using Bloody.Core.ClassGenerator.Utils;

namespace Bloody.Core.ClassGenerator
{
    internal static class ClassGenerator
    {
        private const string ModelsFolder = @"E:\MODS\BloodyCore\BloodyCore\Models\v1\Internals";

        public static void GenerateClasses()
        {
            if (VWorld.IsServer)
            {
                GenerateClasses(VWorld.Server);
                GenerateManagedTypeClasses(VWorld.Server);
            }
            else if (VWorld.IsClient)
            {
                //GenerateClasses(VWorld.Client);
                GenerateManagedTypeClasses(VWorld.Client);
            }
        }

        private static void GenerateManagedTypeClasses(World world)
        {
            var managedDataRegistry = world.GetExistingSystemManaged<GameDataSystem>().ManagedDataRegistry;
            var allTypes = new HashSet<Type>();
            foreach (var dataKV in managedDataRegistry._DataLookupMap)
            {
                allTypes.Add(Type.GetType(dataKV.value.GetIl2CppType().AssemblyQualifiedName));
            }

            var modelName = "BaseManagedDataModel";
            var codeBuilder = new CodeBuilder();
            codeBuilder.StartClassManaged(modelName);
            foreach (var componentType in allTypes)
            {
                codeBuilder.AddManagedDataType(componentType);
            }
            codeBuilder.EndClass();
            var destinationFilePath = Path.Combine(ModelsFolder, $"{modelName}.cs");
            File.WriteAllText(destinationFilePath, codeBuilder.Build());
        }

        private static void GenerateClasses(World world)
        {
            var allTypes = new Dictionary<string, ComponentType>();
            var entities = world.EntityManager.GetAllEntities();
            var count = entities.Length;
            for (var i = 0; i < entities.Length; i++)
            {
                if (i % 1000 == 0)
                {
                    Logger.LogInfo($"{i} / {count}");
                }
                var entity = entities[i];
                var componentTypes = world.EntityManager.GetComponentTypes(entity);
                foreach (var componentType in componentTypes)
                {
                    allTypes[componentType.GetManagedType().AssemblyQualifiedName] = componentType;
                }
            }

            var modelName = "BaseEntityModel";
            var codeBuilder = new CodeBuilder();
            codeBuilder.StartClassEntity(modelName);
            foreach (var componentType in allTypes.Values)
            {
                codeBuilder.Add(componentType);
            }
            codeBuilder.EndClass();
            var destinationFilePath = Path.Combine(ModelsFolder, $"{modelName}.cs");
            File.WriteAllText(destinationFilePath, codeBuilder.Build());
        }
    }

    public class CodeBuilder
    {
        private StringBuilder builder;

        public void StartClassEntity(string className)
        {
            builder = new StringBuilder($@"using System.Collections.Generic;
using Unity.Entities;

namespace Bloody.Core.Models.Internals
{{
    public class {className}
    {{
        private readonly World _world;
        private readonly Entity _entity;

        internal {className}(World world, Entity entity)
        {{
            _world = world;
            _entity = entity;
        }}
");
        }

        public void StartClassManaged(string className)
        {
            builder = new StringBuilder($@"using System.Collections.Generic;
using Unity.Entities;

namespace Bloody.Core.Models.Internals
{{
    public class {className}
    {{
        private readonly World _world;
        private readonly BaseEntityModel _entityModel;

        internal {className}(World world, BaseEntityModel entityModel)
        {{
            _world = world;
            _entityModel = entityModel;
        }}
");
        }

        public void Add(ComponentType type)
        {
            var managedType = type.GetManagedType();
            var typeName = managedType.FullName;
            var name = managedType.Name;
            if (typeName.Contains("+"))
            {
                return;
            }
            if (type.IsBuffer)
            {
                builder.AppendLine($"        public List<{typeName}> {PluralizationProvider.Pluralize(name)} => _world.EntityManager.GetBufferInternal<{typeName}>(_entity);");
            }
            else if (type.IsZeroSized)
            {
                builder.AppendLine($"        public bool {name} => _world.EntityManager.HasComponentInternal<{typeName}>(_entity);");
            }
            else
            {
                builder.AppendLine($"        public {typeName}? {name} => _world.EntityManager.TryGetComponentDataInternal<{typeName}>(_entity, out var value) ? value : null;");
            }
        }

        public void AddManagedDataType(Type type)
        {
            var typeName = type.FullName;
            var name = type.Name;
            if (typeName.Contains("+"))
            {
                return;
            }

            builder.AppendLine($"        public {typeName} {name} => _world.GetManagedComponentDataInternal<{typeName}>(_entityModel);");

        }

        public void EndClass()
        {
            builder.Append(@"    }
}");
        }

        public string Build()
        {
            return builder.ToString();
        }
    }
}
