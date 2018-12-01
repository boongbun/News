using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using News.Models;

namespace News.Common
{
    public class SimSoFunction
    {
        #region CommonCheck

        public static bool KiemTraKieuSo(string soucreStr, string checkStr)
        {
            bool result = false;

            switch (checkStr.ToLower())
            {
                case "*68":
                    result = RepeatGroupNumerStringOnLast68(soucreStr, 2);
                    break;
                case "*368":
                    result = RepeatGroupNumerStringOnLast368(soucreStr, 2);
                    break;
                case "*xx":
                    result = RepeatGroupNumerStringOnLast(soucreStr, 2);
                    break;
                case "*xxx":
                    result = RepeatGroupNumerStringOnLast(soucreStr, 3);
                    break;
                case "*xxxx":
                    result = RepeatGroupNumerStringOnLast(soucreStr, 4);
                    break;
                case "*xxxxx":
                    result = RepeatGroupNumerStringOnLast(soucreStr, 5);
                    break;
                case "*xxxxxx":
                    result = RepeatGroupNumerStringOnLast(soucreStr, 6);
                    break;
                case "*xxxxxxx":
                    result = RepeatGroupNumerStringOnLast(soucreStr, 7);
                    break;
                case "*xxxxxxxx":
                    result = RepeatGroupNumerStringOnLast(soucreStr, 8);
                    break;
                case "*xxxxxxxxx":
                    result = RepeatGroupNumerStringOnLast(soucreStr, 9);
                    break;
                case "xx":
                    result = RepeatNumerString(soucreStr, 2);
                    break;
                case "xxx":
                    result = RepeatNumerString(soucreStr, 3);
                    break;
                case "xxxx":
                    result = RepeatNumerString(soucreStr, 4);
                    break;
                case "xxxxx":
                    result = RepeatNumerString(soucreStr, 5);
                    break;
                case "xxxxxx":
                    result = RepeatNumerString(soucreStr, 6);
                    break;
                case "xxxxxxx":
                    result = RepeatNumerString(soucreStr, 7);
                    break;
                case "xxxxxxxx":
                    result = RepeatNumerString(soucreStr, 8);
                    break;
                case "xxxxxxxxx":
                    result = RepeatNumerString(soucreStr, 9);
                    break;
                case "abab":
                    result = RepeatGroupNumerString(soucreStr, 2);
                    break;
                case "abcabc":
                    result = RepeatGroupNumerString(soucreStr, 3);
                    break;
                case "abcdabcd":
                    result = RepeatGroupNumerString(soucreStr, 4);
                    break;
                case "abcdeabcde":
                    result = RepeatGroupNumerString(soucreStr, 5);
                    break;
            }

            return result;
        }

        public static bool RepeatNumerString(string repeatedWord, int timeAppear)
        {
            bool lastcount = false;
            try
            {
                if (string.IsNullOrEmpty(repeatedWord))
                {
                    return false;
                }

                repeatedWord = CheckStringInput(repeatedWord);

                int[] demLuot = new int[10];
                int countDemLuot = 0;
                int countXuatHien = 1;

                char previousLetter = repeatedWord[0];
                for (int i = 1; i < repeatedWord.Count(); i++)
                {
                    if (repeatedWord[i] == previousLetter)
                    {
                        countXuatHien++;
                        demLuot[countDemLuot] = countXuatHien;
                    }
                    else
                    {
                        previousLetter = repeatedWord[i];
                        countXuatHien = 1;
                        countDemLuot++;
                    }
                }

                lastcount = demLuot.Contains(timeAppear);

                //var res = repeatedWord.GroupBy(p => p).Select(p => new { Count = p.Count(), Char = p.Key })
                //    .GroupBy(p => p.Count, p => p.Char).OrderByDescending(p => p.Key).First();
                //lastcount = res.Key;
                demLuot = null;
                repeatedWord = null;
            }
            catch
            {
                return lastcount;
            }


            return lastcount;
        }

        public static bool RepeatGroupNumerString(string repeatedWord, int timesAppear)
        {
            bool lastcount = false;
            try
            {
                if (string.IsNullOrEmpty(repeatedWord))
                {
                    return false;
                }

                repeatedWord = CheckStringInput(repeatedWord);

                int[] demLuot = new int[10];
                int countDemLuot = 0;
                int countXuatHien = 1;

                string previousLetter = string.Empty;
                for (int i = 0; i < repeatedWord.Count(); i++)
                {
                    for (int j = i; j < timesAppear + i; j++)
                    {
                        if (j >= repeatedWord.Count()) continue;
                        previousLetter = previousLetter + repeatedWord[j];
                    }

                    if (previousLetter.Length >= timesAppear)
                    {
                        if (Regex.Matches(repeatedWord, previousLetter).Count > 1)
                        {

                            demLuot[countDemLuot] = countXuatHien;
                            countDemLuot++;
                        }
                    }


                    previousLetter = string.Empty;
                }

                lastcount = demLuot.Contains(1);

                demLuot = null;
                repeatedWord = null;
            }
            catch (Exception e)
            {
                var error = e.Message;
                return lastcount = false;
            }


            return lastcount;
        }

        public static bool RepeatGroupNumerStringOnLast(string repeatedWord, int timesAppear)
        {
            try
            {
                bool result = false;
                if (string.IsNullOrEmpty(repeatedWord))
                {
                    return false;
                }
                repeatedWord = CheckStringInput(repeatedWord);

                var dataArray = ConvertStringToIntArray(repeatedWord);
                int countContinous = 1;
                int demIn = 1;
                int demOut = 1;
                for (int i = dataArray.Length - 1; i >= 0; --i)
                {
                    if (dataArray[i] == dataArray[i - 1])
                    {
                        if (demIn == demOut)
                        {
                            countContinous++;
                        }

                        if (countContinous >= timesAppear)
                        {
                            return true;
                        }

                        demIn++;
                    }

                    demOut++;
                }

                result = countContinous >= timesAppear;

                return result;
            }
            catch (Exception e)
            {
                var error = e.Message;
                return false;
            }
        }
        public static bool RepeatGroupNumerStringOnLast368(string repeatedWord, int timesAppear)
        {
            try
            {
                bool result = false;
                var compareData = ConvertStringToIntArray("368");
                if (string.IsNullOrEmpty(repeatedWord))
                {
                    return false;
                }
                repeatedWord = CheckStringInput(repeatedWord);

                var dataArray = ConvertStringToIntArray(repeatedWord);
                int countContinous = 1;
                int demIn = 1;
                int demOut = 1;
                for (int i = dataArray.Length - 1; i >= 0; --i)
                {
                    if ((dataArray[i] == dataArray[i - 1]) && compareData.Contains(dataArray[i]))
                    {
                        if (demIn == demOut)
                        {
                            countContinous++;
                        }

                        if (countContinous >= timesAppear)
                        {
                            return true;
                        }

                        demIn++;
                    }

                    demOut++;
                }

                result = countContinous >= timesAppear;

                return result;
            }
            catch (Exception e)
            {
                var error = e.Message;
                return false;
            }
        }

        public static bool RepeatGroupNumerStringOnLast68(string repeatedWord, int timesAppear)
        {
            try
            {
                bool result = false;
                var compareData = ConvertStringToIntArray("68");
                if (string.IsNullOrEmpty(repeatedWord))
                {
                    return false;
                }
                repeatedWord = CheckStringInput(repeatedWord);

                var dataArray = ConvertStringToIntArray(repeatedWord);
                int countContinous = 1;
                int demIn = 1;
                int demOut = 1;
                for (int i = dataArray.Length - 1; i >= 0; --i)
                {
                    if ((dataArray[i] == dataArray[i - 1]) && compareData.Contains(dataArray[i]))
                    {
                        if (demIn == demOut)
                        {
                            countContinous++;
                        }

                        if (countContinous >= timesAppear)
                        {
                            return true;
                        }

                        demIn++;
                    }

                    demOut++;
                }

                result = countContinous >= timesAppear;

                return result;
            }
            catch (Exception e)
            {
                var error = e.Message;
                return false;
            }
        }

        public static bool RepeatGroupNumerStringOnLastMust68(string repeatedWord, int timesAppear)
        {
            try
            {
                bool result = false;
                var compareData = ConvertStringToIntArray("68");
                if (string.IsNullOrEmpty(repeatedWord))
                {
                    return false;
                }
                repeatedWord = CheckStringInput(repeatedWord);

                var dataArray = ConvertStringToIntArray(repeatedWord);
                int countContinous = 1;
                int demIn = 1;
                int demOut = 1;
                for (int i = dataArray.Length - 1; i >= 0; --i)
                {
                    if ((dataArray[i] == dataArray[i - 1]) && compareData.Contains(dataArray[i]))
                    {
                        if (demIn == demOut)
                        {
                            countContinous++;
                        }

                        if (countContinous >= timesAppear)
                        {
                            return true;
                        }

                        demIn++;
                    }

                    demOut++;
                }

                result = countContinous >= timesAppear;

                return result;
            }
            catch (Exception e)
            {
                var error = e.Message;
                return false;
            }
        }




        #endregion

        #region DefaultDataAndFunction

        private static string CheckStringInput(string inputStr)
        {
            try
            {
                if (string.IsNullOrEmpty(inputStr))
                {
                    inputStr = string.Empty;
                }
                else
                {
                    inputStr = inputStr.Replace(" ", "");
                    inputStr = inputStr.Replace(".", "");
                    inputStr = inputStr.Replace(",", "");
                    inputStr = inputStr.Replace(";", "");
                }
            }
            catch
            {
                inputStr = string.Empty;
            }
            return inputStr;
        }

        private static int[] ConvertStringToIntArray(string inputStr)
        {
            try
            {
                if (string.IsNullOrEmpty(inputStr))
                {
                    return null;
                }
                else
                {
                    //var lengthString = inputStr.Length;
                    //int[] result = new int[lengthString];
                    //for (int i = 0; i < inputStr.Length; i++)
                    //{
                    //    result[i] = inputStr[i] - '0';
                    //}

                    //return result;

                    return inputStr.Select(x => int.Parse(x.ToString())).ToArray();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static List<SimSoModel> ListDefautAppropriateNumberCheck = new List<SimSoModel>()
        {
            new SimSoModel(){AppropriateNumberCheck = "xx", TenLoaiSim = "Nhị hoa"},
            new SimSoModel(){AppropriateNumberCheck = "xxx", TenLoaiSim = "Tam hoa"},
            new SimSoModel(){AppropriateNumberCheck = "xxxx", TenLoaiSim = "Tứ quý"},
            new SimSoModel(){AppropriateNumberCheck = "xxxxx", TenLoaiSim = "Ngũ quý"},
            new SimSoModel(){AppropriateNumberCheck = "xxxxxx", TenLoaiSim = "Lục quý"},
            new SimSoModel(){AppropriateNumberCheck = "xxxxxxx", TenLoaiSim = "Thất quý"},
            new SimSoModel(){AppropriateNumberCheck = "xxxxxxxx", TenLoaiSim = "Bát quý"},
            new SimSoModel(){AppropriateNumberCheck = "xxxxxxxxx", TenLoaiSim = "Cửu quý"},
            new SimSoModel(){AppropriateNumberCheck = "abab", TenLoaiSim = "Gánh đôi"},
            new SimSoModel(){AppropriateNumberCheck = "abcabc", TenLoaiSim = "Gánh tam"},
            new SimSoModel(){AppropriateNumberCheck = "abcdabcd", TenLoaiSim = "Gánh tứ"},
            new SimSoModel(){AppropriateNumberCheck = "abcdeabcde", TenLoaiSim = "Gánh ngũ"},
        };

        public static List<SimSoModel> ListDefautAppropriateNumberCheck1 = new List<SimSoModel>()
        {
            new SimSoModel(){AppropriateNumberCheck = "*368", TenLoaiSim = "Cuối 368"},
            new SimSoModel(){AppropriateNumberCheck = "*68", TenLoaiSim = "Cuối 68"},

            new SimSoModel(){AppropriateNumberCheck = "*xx", TenLoaiSim = "Cuối Nhị hoa"},
            new SimSoModel(){AppropriateNumberCheck = "*xxx", TenLoaiSim = "Cuối Tam hoa"},
            new SimSoModel(){AppropriateNumberCheck = "*xxxx", TenLoaiSim = "Cuối Tứ quý"},
            new SimSoModel(){AppropriateNumberCheck = "*xxxxx", TenLoaiSim = "Cuối Ngũ quý"},
            new SimSoModel(){AppropriateNumberCheck = "*xxxxxx", TenLoaiSim = "Cuối Lục quý"},
            new SimSoModel(){AppropriateNumberCheck = "*xxxxxxx", TenLoaiSim = "Cuối Thất quý"},
            new SimSoModel(){AppropriateNumberCheck = "*xxxxxxxx", TenLoaiSim = "Cuối Bát quý"},
            new SimSoModel(){AppropriateNumberCheck = "*xxxxxxxxx", TenLoaiSim = "Cuối Cửu quý"},
        };

        public static List<SimSoModel> ListDefautAppropriateNumberCheck2 = new List<SimSoModel>()
        {
            new SimSoModel(){AppropriateNumberCheck = "*6868", TenLoaiSim = "6868"},
            new SimSoModel(){AppropriateNumberCheck = "*6688", TenLoaiSim = "6688"},
            new SimSoModel(){AppropriateNumberCheck = "*6688", TenLoaiSim = "6688"},


        };
        #endregion

    }
}