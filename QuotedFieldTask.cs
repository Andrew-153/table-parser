using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase("a \"c\"", 0, "c", 3)]
        [TestCase("\"bcd ef\" a 'x y'", 0, "bcd ef", 8)]
        [TestCase("'' \"bcd ef\" 'x y'", 0, "", 2)]
        [TestCase("\"a 'b' 'c' d\" '\"1\" \"2\" \"3\"'", 0, "a 'b' 'c' d", 13)]
        [TestCase("'\"1\" \"2\" \"3\"'", 0, "\"1\" \"2\" \"3\"", 13)]
        [TestCase("\"1 2 3'", 0, "1 2 3'", 7)]
        [TestCase("a \"c\"", 0, "c", 3)]
        [TestCase(@"\""a b\""""", 0, "a b\"", 7)]
        [TestCase(@"""QF \""""", 0, "QF \"", 7)]
        [TestCase(@"""QF \""", 0, "QF \"", 6)]
        [TestCase("b \"a'\"", 2, "a'", 4)]
        [TestCase(@"'a\' b'", 0, "a' b", 7)]
        [TestCase("'a'b", 0, "a", 3)]
        [TestCase("a'b'", 1, "b", 3)]
        [TestCase(@"b\""a'""", 1, "a'", 4)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }

        // Добавьте свои тесты
    }

    class QuotedFieldTask
    {
        static bool isMarkQuotes;   // Проверяем нашли мы первые кавычки или нет
        static char indexMark;      // Запоминаем символ найденной кавычки
        static int lengthToken;     // Формируем токен
        static StringBuilder sb;    // Формируем текст

        public static Token ReadQuotedField(string line, int startIndex)
        {
            // Инициализация полей:
            isMarkQuotes = false;
            indexMark = '\0';
            lengthToken = 0;
            sb = new StringBuilder();

            for (int i = startIndex; i < line.Length; i++)
            {
                if (ChekOnStartQuotes(line, i))    // проверка на первую кавычку
                    continue;
                else if (ChekOnEndQuotes(line, i))  // проверка на последнюю кавычку
                    break;
                if (CheckSlash(line, ref i))        // проверка на символ слэш
                    continue;
                else if (isMarkQuotes)
                {
                    sb.Append(line[i]);
                    lengthToken++;
                }
            }
            return new Token(sb.ToString(), startIndex, lengthToken);
        }

        public static bool CheckSlash(string word, ref int index)
        {
            if (word[index] == '\\' & isMarkQuotes)
            {
                sb.Append("" + word[index + 1]);
                lengthToken += 2;
                ++index;
                return true;
            }
            return false;
        }

        public static bool ChekOnStartQuotes(string word, int index)
        {
            if (!isMarkQuotes & (word[index] == "\""[0] || word[index] == "\'"[0]))
            {
                isMarkQuotes = !isMarkQuotes;
                indexMark = word[index];
                lengthToken++;
                return true;
            }
            return false;
        }

        public static bool ChekOnEndQuotes(string word, int index)
        {
            if (word[index] == indexMark)
            {
                isMarkQuotes = !isMarkQuotes;
                lengthToken++;
                return true;
            }
            return false;
        }
    }
}
