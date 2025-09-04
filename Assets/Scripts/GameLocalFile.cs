using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public static class GameLocalFile 
    {
       public static  string FILE_DATAFASHION ="Assets/Resources/Art/_FashionItems/DataFashion.json";
     //  public const  string FILE_DATAFASHION ="Assets/Resources/Art/_FashionItems/DataFashion.json";
     public static string FILE_DATAITEM = "Assets/Resources/Data/DataItems.json";
    // public static string FILE_LOAD_DATAITEM = "Assets/Resources/Art/_FashionItems/LoadData.json";
       public static string FilePathGame16(int indexLevel)
       {
           return $"Assets/Resources/Art/game16/LevelData_16/DataLevel_{indexLevel}.json";
       }

       public static string ItemLoaded(int id)
       {
           return $"Assets/Resources/Art/_FashionItems/{id}";
       }
    }
    
}
