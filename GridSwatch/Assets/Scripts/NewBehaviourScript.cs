using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class NewBehaviourScript : MonoBehaviour
{
    void ReadFile(string path)
    {
        string[] lines = File.ReadAllLines(path);

        Dictionary<int, string> wordmap = new Dictionary<int, string>();
        foreach (string line in lines)
        {
            string value = line.Split(" ")[0];
            int key = int.Parse(line.Split(" ")[1]);
            wordmap.Add(key, value);
        }

        List<int> sortedKeys = new List<int>(wordmap.Keys);
        quickSort(sortedKeys, 0, sortedKeys.Count);

        List<int> pyramid = new List<int>();
        for (int i = 0; i < sortedKeys.Count; i++)
        {
            pyramid.Add(sortedKeys[i+1]);
        }

        foreach (int wordIndex in pyramid)
        {
            Console.Write(wordmap[wordIndex]);
        }
        
    }
    
    void swap(List<int> arr, int i, int j)
    {
        int temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }
    
    int partition(List<int> arr, int low, int high)
    {
        int pivot = arr[high];
        
        int i = (low - 1);

        for (int j = low; j <= high - 1; j++) {

            if (arr[j] < pivot) 
            {

                i++;
                swap(arr, i, j);
            }
        }
        swap(arr, i + 1, high);
        return (i + 1);
    }

    void quickSort(List<int> arr, int low, int high)
    {
        if (low < high) {
            int pi = partition(arr, low, high);
            quickSort(arr, low, pi - 1);
            quickSort(arr, pi + 1, high);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
