using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StringTool : MonoBehaviour
{
    static StringBuilder stringBuilder = new StringBuilder();
    public static string ConnectString(string a, string b)
    {
        stringBuilder.Clear();

        stringBuilder.Append(a);
        stringBuilder.Append(b);
        return stringBuilder.ToString();
    }

     public static string ConnectString(params string[] strings)
    {
        stringBuilder.Clear();
        
        foreach (string s in strings)
        {
            stringBuilder.Append(s);
        }
        
        return stringBuilder.ToString();
    }
}
