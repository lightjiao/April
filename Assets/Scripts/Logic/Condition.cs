using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Branch
    {
        public Condition Condition;
        public List<int> EventPool;
        public string TextContent;

        public static Branch Parse(string condition, string content)
        {
            try
            {
                var inst = new Branch
                {
                    Condition = Condition.Parse(condition)
                };

                if (int.TryParse(content.Replace("|", ""), out _))
                {
                    inst.EventPool = new List<int>();
                    var list = content.Split('|');
                    foreach (var str in list)
                    {
                        if (string.IsNullOrEmpty(str)) continue;
                        inst.EventPool.Add(int.Parse(str));
                    }
                }
                else
                {
                    inst.TextContent = content;
                }

                return inst;
            }
            catch (Exception e)
            {
                Debug.LogError(e + condition + content);
                return null;
            }
        }
    }

    public class Condition
    {
        public bool IsNot;
        public string ConditionType;
        public string Operator;
        public int OperatorValue;

        public Condition AndCondition;

        public bool Cal()
        {
            var returnResult = false;

            if (CalAsExpression(out var result))
            {
                returnResult = result;
            }

            if (CalAsChance(out result))
            {
                returnResult = result;
            }

            if (CalAsEvent(out result))
            {
                returnResult = result;
            }

            return (IsNot ? !returnResult : returnResult) && (AndCondition?.Cal() ?? true);
        }

        private bool CalAsExpression(out bool result)
        {
            result = false;

            int? variableValue = ConditionType switch
            {
                ConstStr.食物 => GameManager.Properties.Food.Value,
                ConstStr.心情 => GameManager.Properties.San.Value,
                ConstStr.天数 => GameManager.Properties.Day.Value,
                ConstStr.家境 => GameManager.Properties.Money.Value,
                _ => null
            };

            if (variableValue == null)
            {
                return false;
            }

            result = Operator switch
            {
                ">" => variableValue > OperatorValue,
                "<" => variableValue < OperatorValue,
                "=" => variableValue == OperatorValue,
                _ => result
            };

            return true;
        }

        private bool CalAsChance(out bool result)
        {
            result = false;

            int? variableValue = ConditionType switch
            {
                ConstStr.抢菜概率 => GameManager.Properties.ChangeOfQiangCai.Value,
                ConstStr.团购概率 => GameManager.Properties.ChangeOfTuanGou.Value,
                ConstStr.公司空投 => GameManager.Properties.ChangeOfGongsiKongTou.Value,
                ConstStr.染病概率 => GameManager.Properties.ChangeOfSick.Value,
                _ => null
            };

            if (variableValue == null)
            {
                return false;
            }

            result = Random.Range(0, 100) < variableValue;

            return true;
        }

        private bool CalAsEvent(out bool result)
        {
            result = false;

            if (ConditionType != ConstStr.事件) return false;

            result = GameManager.HappenedEvent.Contains(OperatorValue);

            return true;
        }

        public static Condition Parse(string conditionString)
        {
            // 去除空格
            conditionString = conditionString.Replace(" ", "");
            if (string.IsNullOrEmpty(conditionString))
            {
                return null;
            }

            try
            {
                var expressionList = conditionString.Split("&&");
                Condition condition = null;
                foreach (var str in expressionList)
                {
                    if (string.IsNullOrEmpty(str)) continue;

                    var newCondition = new Condition
                    {
                        AndCondition = condition
                    };

                    const string pattern = @"【.*】";
                    var match = Regex.Match(str, pattern);
                    newCondition.ConditionType = match.Value;

                    var list = str.Split(newCondition.ConditionType).ToList();

                    newCondition.IsNot = list[0] == "!";
                    if (!string.IsNullOrEmpty(list[1]))
                    {
                        if (int.TryParse(list[1], out var intValue))
                        {
                            newCondition.OperatorValue = intValue;
                        }
                        else
                        {
                            newCondition.Operator = list[1][..1];
                            newCondition.OperatorValue = int.Parse(list[1][1..]);
                        }
                    }

                    condition = newCondition;
                }

                return condition;
            }
            catch (Exception e)
            {
                Debug.LogError(e + conditionString);
                return null;
            }
        }
    }
}