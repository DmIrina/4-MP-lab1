using System.Collections;
using System.Text;

namespace lab1
{
    class Task1
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
            }


            // заполненость массивов wordsArray и countArray - размер словаря 
            int wordsCount = 0;

            // словарь неповторяющихся слов
            String[] wordsArray = new String[charCount / 2];

            String[] stopWords = new String[]{"i", "me", "my", "myself", "we", "our", "ours", "ourselves", "you", "you're", "you've", "you'll", "you'd","wouldn't",
                                            "your", "yours", "yourself", "yourselves", "he", "him", "his", "himself", "she", "she's", "her", "hers", "herself",
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

            // количество повторов слова (по индексу)
            int[] countArray = new int[charCount / 2];

            // выделенное слово 
            string currentWord = "";

            int i = 0;
        startloop:
            {
                char c;

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


                // символы разделители слов иои конец текста
                if (c == ' ' || c == '.' || c == ',' || c == '\r' || c == '\n' || c == ';' || c == ':' || c == '!' || c == '\''
                || c == '?' || c == '(' || c == ')' || c == '"' || c == '&' || c == '/' || c == '_' || c == '-' || c == '\t')
                {
                    // слово выделено
                    if (currentWord.Length > 0)
                    {

                        int m = 0;
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
                        ;

                        if (isStop)
                        {
                            goto endcheckingloop;
                        }

                        // проверка наличия выделеного слова в массиве
                        int j = 0;

                    checkingloop:
                        if (wordsArray[0] == null)          
                        {
                            // словарь пустой
                            wordsArray[0] = currentWord;
                            countArray[0]++;
                            wordsCount++;
                            goto endcheckingloop;
                        }

                        if (wordsArray[j].Equals(currentWord))
                        {
                            // такое слово уже есть в словаре (wordsArray)
                            countArray[j]++;
                            goto endcheckingloop;
                        }
                        else
                        {
                            j++;
                            if (j < wordsCount)
                            {
                                goto checkingloop;
                            }
                            else
                            {
                                //  слово в словаре отсутствует, добавляем
                                wordsArray[wordsCount] = currentWord;
                                countArray[wordsCount] = 1;
                                //  размер словаря 
                                wordsCount++;
                            }
                        }

                    endcheckingloop:
                        ;
                    }
                    // checkingloop ends

                    currentWord = "";
                }


                else
                // слово  не закончено - дополняем символом
                {
                    currentWord += c;
                }

                // "for"
                i++;
                if (i <= charCount)
                {
                    // i == charCount - End of file - изображаем
                    goto startloop;
                }
            }

            i = 0;
            String[] sortedWords = new String[wordsCount];
            int[] sortedCounts = new int[wordsCount];

        fillingloop:

            int l = 0;
            int maxValue = 0;
            int maxIndex = 0;

        sortingloop:

            if (countArray[l] > maxValue)

            {
                maxValue = countArray[l];
                maxIndex = l;
            }
            l++;

            if (l < wordsCount)
            {
                goto sortingloop;
            }
        // sortloop ends

            sortedCounts[i] = maxValue;
            sortedWords[i] = wordsArray[maxIndex];
            countArray[maxIndex] = -1;
            

            i++;
            if (i < wordsCount)
            {
                goto fillingloop;
            }


            // печать указанного количества символов
            int k = 0;
            int N = 25;
        printloop:

            Console.WriteLine(sortedWords[k] + " - " + sortedCounts[k]);
            k++;
            if (k < N)
            {
                goto printloop;
            }

        }
    }
}