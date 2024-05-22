using Unity.Entities;
using Bloody.Core.Models.v1;

namespace Bloody.Core.GameData.v1
{
    public class Npcs
    {
        private Npcs() { }

        private static Npcs _instance;
        internal static Npcs Instance => _instance ??= new Npcs();

        public NpcModel FromEntity(Entity npcEntity)
        {
            return new NpcModel(npcEntity);
        }
    }
}