using System;

namespace Bloody.Core.GameData.v1
{
    public static class GameData
    {
        public static Users Users => Core._worldDataInitialized ? Users.Instance : throw new InvalidOperationException(Core.NotInitializedError);
        public static Items Items => Core._worldDataInitialized ? Items.Instance : throw new InvalidOperationException(Core.NotInitializedError);
        public static Npcs Npcs => Core._worldDataInitialized ? Npcs.Instance : throw new InvalidOperationException(Core.NotInitializedError);
    }
}
