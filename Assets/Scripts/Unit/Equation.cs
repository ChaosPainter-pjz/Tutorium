using UnityEngine;
/// <summary>
/// 算式
/// </summary>
public static class Equation
{
    /// <summary>
    /// 根据能力值生成成绩
    /// </summary>
    /// <param name="input">能力值</param>
    /// <returns>成绩值</returns>
    public static int ComputationalResults(int input)
    {
        if (input >= 1000) return 100;
        var fenShu = Mathf.Pow(1f - Mathf.Pow(input - 1f, 2f), 0.5f);
        var max = Mathf.CeilToInt(fenShu);
        var chaZhi = max - fenShu;
        var a = Random.Range(0f, 1f);
        if (chaZhi<a)
        {
            max--;
        }
        return max;
    }
    /// <summary>
    /// Convert Arabic numerals to Chinese numerals
    /// </summary>
    public static readonly string[] CaToCh = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
}