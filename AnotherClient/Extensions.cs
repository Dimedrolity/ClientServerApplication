﻿namespace AnotherClient
{
    public static class Extensions
    {
        public static void FillWithZeros(this byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = 0;
            }
        }
        
        public static int ToIntLittleEndian(this byte[] bytes)
        {
            var number = 0;
            var length = bytes.Length;

            for (int i = length - 1; i >= 0; i--)
            {
                number = bytes[i] | number << 8;
            }

            return number;
        }
    }
}