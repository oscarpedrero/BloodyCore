using Bloody.Core.Models.v1.CustomEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bloody.Core.API.v1
{

    public delegate void CustemEventhandler(CustomEvntModel eventMode);

    public class CustomEventsHandlerSystem
    {

        public static event CustemEventhandler OnCustomEvent;

        private static List<ModCustomEvent> ModsAcceptEvent = [];

        public static bool SendEventToOtherMod(CustomEvntModel eventModel, out string result)
        {
            try
            {
                eventModel.From = Assembly.GetCallingAssembly().FullName;

                if (!TryValidateModAndTypes(eventModel))
                {
                    result = "Mod or type incorrect";
                    return false;
                }

                OnCustomEvent?.Invoke(eventModel);
                

                result = string.Empty;
                return true;

            } catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }

        public static bool RegisterModForCustomEvent(ModCustomEvent modCustomEvent, out string result)
        {
            try
            {
                modCustomEvent.RegisterBy = Assembly.GetCallingAssembly().FullName;
                
                if(modCustomEvent.Name == string.Empty || modCustomEvent.Types.Count <= 0)
                {
                    result = "Incorrect Data Model";
                    return false;
                }

                result = string.Empty;
                return true;

            } catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }

        private static bool TryValidateModAndTypes(CustomEvntModel eventModel)
        {
            try
            {
                var modModel = ModsAcceptEvent.Where(x=> x.Name == eventModel.To).FirstOrDefault();
                if(modModel == null) return false;
                var type = modModel.Types.Where(x=> x == eventModel.Type).FirstOrDefault();
                if (type == null) return false;
                return true;

            } catch (Exception ex)
            {
                return false;
            }
        }

    }
}
