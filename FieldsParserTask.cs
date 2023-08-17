using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        #region Тесты
        [TestCase("text", new[] { "text" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("a\"b c d e\"f", new[] { "a", "b c d e", "f" })]
        [TestCase("a \"b c d e\" f", new[] { "a", "b c d e", "f" })]
        [TestCase("hello  world", new[] { "hello", "world" })]
        [TestCase("\"bcd ef\"", new[] { "bcd ef" })]
        [TestCase("a", new[] { "a" })]
        [TestCase("'x y'", new[] { "x y" })]
        [TestCase("\"bcd ef\" a", new[] { "bcd ef", "a" })]
        [TestCase("\"a 'b' 'c' d\"", new[] { "a 'b' 'c' d" })]
        [TestCase("'\"1\" \"2\" \"3\"'", new[] { "\"1\" \"2\" \"3\"" })]
        [TestCase(@"b\""a'""", new[] { "b\\", "a'" })]
        [TestCase("a'b'", new[] { "a", "b" })]
        [TestCase(@"'a\' b'", new[] { "a' b" })]
        [TestCase(@"'a\' b'", new[] { "a' b" })]
        [TestCase(@"""a\"" b""", new[] { "a\" b" })]
        [TestCase(@"abc ", new[] { "abc" })]
        [TestCase(@"""def g h", new[] { "def g h" })]
        [TestCase(@"""def g h ", new[] { "def g h " })]
        [TestCase("''", new[] { "" })]
        [TestCase(@"'\\'", new[] { "\\" })]
        [TestCase("", new string[0])]
        #endregion
        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }

        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }
    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            // Локальные переменные:
            var tokens = new List<Token>();
            var token = new Token(line, 0, line.Length);
            int indexNext = 0;
            for (int k = indexNext; k < line.Length; k++)
            {
                switch (ChooseSwitch(line, k))
                {
                    // Токен получает предложение в кавычках:
                    case 2: token = ReadQuotedField(line, k); break;
                    // Токен получает простое предложение:
                    case 1: token = ReadField(line, k); break;
                    // Если нашли пробел
                    case 0: continue;
                    // Если line пустой: 
                    case -1: tokens.Add(new Token(line, 0, line.Length)); break;
                }
                k = token.GetIndexNextToToken() - 1;
                tokens.Add(token);
            }
            return tokens;
        }

        /// <summary>
        /// Метод работает с "простыми" предложениями.
        /// Возвращает строку без кавычек.
        /// </summary>
        /// <param name="line">Строковое значение (текст)</param>
        /// <param name="startIndex">Индекс начальной позиции</param>
        /// <returns></returns>
        private static Token ReadField(string line, int startIndex)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = startIndex; i < line.Length; i++)
            {
                if (CheckOnExpectedChar(line[i])) { break; }
                sb.Append(line[i]);
            }
            string result = sb.ToString();
            return new Token(result, startIndex, result.Length);
        }

        /// <summary>
        /// Метод работает с предложениями в которых
        /// есть кавычки.
        /// Возвращает строку с данными, которые 
        ///  были в кавычках.        
        /// <param name="line">Строковое значение (текст)</param>
        /// <param name="startIndex">Индекс начальной позиции</param>
        /// <returns></returns>
        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }

        /// <summary>
        /// Метод сверяет символ с группой символов.
        /// Если находит соответствие, то возвращает True,
        /// иначе False.
        /// </summary>
        /// <param name="sybm">Проверяемый символ</param>
        /// <returns></returns>
        public static bool CheckOnExpectedChar(char symbol)
        {
            return symbol.ToString().IndexOfAny(new char[] { '\'', '\"', ' ' }) != -1;
        }

        public static int ChooseSwitch(string line, int index)
        {
            if (!CheckOnExpectedChar(line[index])) return 1;
            else if (line == null | line == "") return -1;
            else if (line[index] == ' ') return 0;
            else return 2;
        }
    }
}