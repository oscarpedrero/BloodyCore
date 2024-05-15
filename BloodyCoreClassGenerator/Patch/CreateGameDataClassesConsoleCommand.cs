using System;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using ProjectM;

namespace Bloody.Core.ClassGenerator.Patch;

public class CreateGameDataClassesConsoleCommand : Engine.Console.DefaultConsoleCommand
{
    //Required for Il2Cpp internals
    public CreateGameDataClassesConsoleCommand(IntPtr ptr) : base(ptr) { }

    //Required to create object from C#
    public CreateGameDataClassesConsoleCommand() : base(ClassInjector.DerivedConstructorPointer<CreateGameDataClassesConsoleCommand>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    //Method parameters correspond to command arguments
    public void Execute()
    {
        Plugin.Logger.LogMessage($"Starting to execute {nameof(CreateGameDataClassesConsoleCommand)}.");

        ClassGenerator.GenerateClasses();


        Plugin.Logger.LogMessage($"{nameof(CreateGameDataClassesConsoleCommand)} has been executed.");
    }

    public void Register(ClientConsoleCommandSystem system)
    {
        //Method to call when command is run
        var executingMethod = Il2CppType.Of<CreateGameDataClassesConsoleCommand>().GetMethod("Execute");
        //Types of Action must match the executing method
        var delegateType = Il2CppType.Of<Il2CppSystem.Action>();

        system.RegisterCommand("CreateGameDataClasses", "Dumps game data in json format", executingMethod.CreateDelegate(delegateType, this));

        Plugin.Logger.LogMessage($"{nameof(CreateGameDataClassesConsoleCommand)} is now registered.");
    }
}