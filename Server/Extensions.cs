namespace Server
{
    public static class Extensions
    {
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
        
        public static string GetSymbolsSeparatedBy(this string message, char separator)
        {
            var charArray = message.ToCharArray();
            return string.Join(separator, charArray);
        }
    }
}