using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public interface ICsvData
    {
        bool ParseOneRaw(string raw);
    }

    public static class CsvParser
    {
        internal static readonly char[] DataSplitSeparators = new char[] {'\t'};
        internal static readonly char[] DataTrimSeparators = new char[] {'\"'};

        public static List<T> ParseData<T>(string dataTableString) where T : ICsvData
        {
            var list = new List<T>();

            var position = 0;
            string dataRowString;
            while ((dataRowString = dataTableString.ReadLine(ref position)) != null)
            {
                if (dataRowString[0] == '#')
                {
                    continue;
                }

                var data = Activator.CreateInstance<T>();

                if (data.ParseOneRaw(dataRowString))
                {
                    list.Add(data);
                }
            }

            return list;
        }

        /// <summary>
        /// 从指定字符串中的指定位置处开始读取一行。
        /// </summary>
        /// <param name="rawString">指定的字符串。</param>
        /// <param name="position">从指定位置处开始读取一行，读取后将返回下一行开始的位置。</param>
        /// <returns>读取的一行字符串。</returns>
        private static string ReadLine(this string rawString, ref int position)
        {
            if (position < 0)
            {
                return null;
            }

            int length = rawString.Length;
            int offset = position;
            while (offset < length)
            {
                char ch = rawString[offset];
                switch (ch)
                {
                    case '\r':
                    case '\n':
                        if (offset > position)
                        {
                            string line = rawString.Substring(position, offset - position);
                            position = offset + 1;
                            if ((ch == '\r') && (position < length) && (rawString[position] == '\n'))
                            {
                                position++;
                            }

                            return line;
                        }

                        offset++;
                        position++;
                        break;

                    default:
                        offset++;
                        break;
                }
            }

            if (offset > position)
            {
                string line = rawString.Substring(position, offset - position);
                position = offset;
                return line;
            }

            return null;
        }
    }
}