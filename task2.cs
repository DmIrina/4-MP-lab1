//dotnet new console --framework net6.0
namespace lab1
{
    class Task2
    {
        static void Main(string[] args)
        {
            long charCount = 0;
            byte[] charArray;

            using (FileStream fstream = File.OpenRead(@"D:\Temp\note.txt"))
            {
                charCount = fstream.Length;
                charArray = new byte[charCount];
                fstream.Read(charArray, 0, charArray.Length);
                string textFromFile = System.Text.Encoding.Default.GetString(charArray);
            }

            int wordsCount = 0;

            String[] dict1_words = new String[charCount / 2];
            int[] dict1_pages = new int[charCount / 2];
            int[] dict1_count = new int[charCount / 2];
            int uniqueWordsCount = 0;
            bool isUnique = true;
            bool isUniqueOnPage = true;
            int currentPage = 0;
            int currentCount = 0;

            String[] stopWords =    {"i", "me", "my", "myself", "we", "our", "ours", "ourselves", "you", "you're", "you've", "you'll", "you'd","wouldn't",
                                    "your", "yours", "yourself", "yourselves", "he", "him", "his", "himself", "she", "she's", "her", "hers", "herself", "s",
                                    "it", "it's", "its", "itself", "they", "them", "their", "theirs", "themselves", "what", "which", "who", "whom", "this",
                                    "that", "that'll", "these", "those", "am", "is", "are", "was", "were", "be", "been", "being", "have", "has", "had", "doesn",
                                    "having", "do", "does", "did", "doing", "a", "an", "the", "and", "but", "if", "or", "because", "as", "until", "while",
                                    "of", "at", "by", "for", "with", "about", "against", "between", "into", "through", "during", "before", "after", "above",
                                    "below", "to", "from", "up", "down", "in", "out", "on", "off", "over", "under", "again", "further", "then", "once", "here",
                                    "there", "when", "where", "why", "how", "all", "any", "both", "each", "few", "more", "most", "other", "some", "such", "no",
                                    "nor", "not", "only", "own", "same", "so", "than", "too", "very", "s", "t", "can", "will", "just", "don", "don't", "should",
                                    "should've", "now", "d", "ll", "m", "o", "re", "ve", "y", "ain", "aren", "aren't", "couldn", "couldn't", "didn", "didn't",
                                    "doesn't", "hadn", "hadn't", "hasn", "hasn't", "haven", "haven't", "isn", "isn't", "ma", "mightn", "mightn't", "mustn", "wouldn",
                                    "mustn't", "needn", "needn't", "shan", "shan't", "shouldn", "shouldn't", "wasn", "wasn't", "weren", "weren't", "won", "won't"};

            int[] countArray = new int[charCount / 2];

            // выделенное слово 
            string currentWord = "";

            int i = 0;
        startloop:
            {
                char c;
                currentPage = i / 1800 + 1;

                if (i == charCount)
                {
                    c = '.';                // END OF FILE
                }
                else
                {
                    if (charArray[i] >= 65 && charArray[i] <= 90)
                    {
                        // to lowerCase
                        c = (char)(charArray[i] + 32);
                    }
                    else
                    {
                        c = (char)charArray[i];
                    }
                }

                if (c == ' ' || c == '.' || c == ',' || c == '\r' || c == '\n' || c == ';' || c == ':' || c == '!' || c == '\''
                || c == '?' || c == '(' || c == ')' || c == '"' || c == '&' || c == '/' || c == '_' || c == '-' || c == '\t')
                {
                    // слово выделено
                    if (currentWord.Length > 1)
                    {
                        int m = 0;
                        int count = 1;          // подсчет количества выделенного слова в словаре
                        bool isStop = false;
                    stopwordsloop:

                        if (currentWord.Equals(stopWords[m]))
                        {
                            isStop = true;
                            goto endstopwordsloop;
                        }

                        m++;
                        if (m < stopWords.Length)
                        {
                            goto stopwordsloop;
                        }
                    endstopwordsloop:
                        if (isStop)
                        {
                            goto fromStopWordsCheck;
                        }

                        // проверка наличия выделеного слова в массиве
                        // словарь пустой
                        if (dict1_words[0] == null)
                        {
                            dict1_words[0] = currentWord;
                            wordsCount++;
                            dict1_pages[0] = 1;
                            dict1_count[0] = 0;
                        }

                        int j = 0;
                    checkingloop:
                        if (j >= wordsCount)
                        {
                            goto endcheckingloop;
                        }

                        if (dict1_words[j].Equals(currentWord))             // таке слово є на будь-якій сторінці
                        {
                            isUnique = false;
                            if (count < dict1_count[j])
                            {
                                count = dict1_count[j] + 1;
                            }
                            else
                            {
                                count++;
                            }

                            if (dict1_pages[j] == currentPage)
                            {
                                isUniqueOnPage = false;
                                dict1_count[j]++;
                            }
                        }

                        j++;
                        goto checkingloop;
                    endcheckingloop:
                        if (isUniqueOnPage)
                        {
                            dict1_words[wordsCount] = currentWord;
                            dict1_pages[wordsCount] = currentPage;
                            dict1_count[wordsCount] = count;
                            wordsCount++;
                        }
                        if (isUnique)
                        {
                            uniqueWordsCount++;
                        }
                        isUnique = true;
                        isUniqueOnPage = true;
                    }

                fromStopWordsCheck:
                    currentWord = "";
                }
                else
                // слово  не закончено - дополняем символом
                {
                    currentWord += c;
                }

                i++;
                if (i <= charCount)
                {
                    // i == charCount - End of file
                    goto startloop;
                }
            }

            // ---------- JOIN ----------------------------------------

            String[] dict2_words = new String[uniqueWordsCount + 1];
            String[] dict2_pages = new String[uniqueWordsCount + 1];
            int[] dict2_count = new int[uniqueWordsCount + 1];
            currentPage = 0;
            int addedWords = 0;
            int i2 = 0;

        outLoop:
            if (i2 >= wordsCount)
            {
                goto endOutLoop;
            }

            currentWord = dict1_words[i2];
            currentPage = dict1_pages[i2];
            currentCount = dict1_count[i2];

            isUnique = true;
            int j2 = 0;
        inLoop:
            if (j2 >= addedWords)
            {
                goto endInLoop;
            }

            if (currentWord == dict2_words[j2])
            {
                dict2_pages[j2] = dict2_pages[j2] + ", " + currentPage;
                if (dict2_count[j2] < currentCount)
                {
                    dict2_count[j2] = currentCount;
                }
                isUnique = false;
                goto endInLoop;
            }

            j2++;
            goto inLoop;

        endInLoop:
            if (isUnique)
            {
                dict2_pages[addedWords] = "" + currentPage;
                dict2_words[addedWords] = currentWord;
                dict2_count[addedWords] = currentCount;
                addedWords++;
            }
            isUnique = true;

            i2++;
            goto outLoop;

        endOutLoop:

            // ---------SORT ------------------------------------------

            int ii = 0;
        outerLoop:
            if (ii > dict2_words.Length)
            {
                goto endOuterLoop;
            }

            int strNum = 0;
        innerLoop:
            if (strNum >= dict2_words.Length - 1)
            {
                goto endInnerLoop;
            }

            bool isLess = true;
            int chNum = 0;


        stringComparingLoop:
            if (chNum >= dict2_words[strNum].Length)
            {
                goto endStringComparingLoop;
            }

            if (chNum >= dict2_words[strNum + 1].Length)           // вышли за границы 2-го слова - значит оно меньше
            {
                isLess = false;
                goto endStringComparingLoop;
            }

            if (dict2_words[strNum][chNum] > dict2_words[strNum + 1][chNum])
            {
                isLess = false;
                goto endStringComparingLoop;
            }
            else
            {
                if (dict2_words[strNum][chNum] < dict2_words[strNum + 1][chNum])
                {
                    goto endStringComparingLoop;
                }
            }

            chNum++;
            goto stringComparingLoop;

        endStringComparingLoop:

            if (!isLess)
            {
                String s = dict2_words[strNum];
                dict2_words[strNum] = dict2_words[strNum + 1];
                dict2_words[strNum + 1] = s;
                String p = dict2_pages[strNum];
                dict2_pages[strNum] = dict2_pages[strNum + 1];
                dict2_pages[strNum + 1] = p;
                int c = dict2_count[strNum];
                dict2_count[strNum] = dict2_count[strNum + 1];
                dict2_count[strNum + 1] = c;
            }

            strNum++;
            goto innerLoop;

        endInnerLoop:

            ii++;
            goto outerLoop;
        endOuterLoop:

            // печать
            int k = 0;
        printloop:
            if (dict2_count[k] <= 100)
            {
                Console.WriteLine(dict2_words[k] + " - " + dict2_pages[k] /* + " - "  + dict2_count[k]*/);
            }
            k++;
            if (k < uniqueWordsCount)
            {
                goto printloop;
            }

        }
    }
}