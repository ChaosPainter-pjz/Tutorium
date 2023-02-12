using System;
using UnityEngine;
/// <summary>
/// 物品
/// </summary>
[Serializable]
public class Article:IComparable
{
    [SerializeField] public string id;
    [SerializeField] public string name;
    [SerializeField] public int number;

    public Article Copy()
    {
        Article article = new Article(id,name,number);
        return article;
    }

    public Article(string _id,string _name,int _number)
    {
        id = _id;
        name = _name;
        number = _number;
    }

    public int CompareTo(object obj)
    {
        try
        {
            if (obj is Article art)
            {
                int thisId = int.Parse(id);
                int objId = int.Parse(art.id);
                if (thisId>objId)
                {
                    return 1;
                }
                else if (thisId<objId)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }

            }
            return 1;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
}