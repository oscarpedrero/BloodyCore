﻿using System;
using System.Linq;

namespace Bloody.Core.Helper.v1
{
    public class UtilsCore
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ0123456789!$%&/()=";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
