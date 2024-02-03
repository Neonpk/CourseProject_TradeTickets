using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CourseProject_SellingTickets.Models;

namespace CourseProject_SellingTickets.Extensions;

public static class ObservableCollectionOrderByRefModeExtensions
{
    private static bool CompareElements<TKey>(TKey a, TKey b, SortMode? sortMode = null) where TKey : IComparable
    {
        switch (sortMode)
        {
            case SortMode.Asc:
                return a.CompareTo(b) > 0;
            
            case SortMode.Desc:
                return a.CompareTo(b) < 0;
            
            default:
                return a.CompareTo(b) > 0;
        }
    }
    
    private static void InsertionSortDesc<TSource, TKey>( ObservableCollection<TSource> source, 
        Func<TSource, TKey> sortFunc, SortMode? sortMode = null) where TKey : IComparable
    {
        TSource x;
        int j = 0; 
        
        for (int i = 0; i < source.Count; ++i)
        {
            x = source[i];
            j = i;
            while (j > 0 && CompareElements(sortFunc.Invoke(source[j - 1]), sortFunc.Invoke(x), sortMode) )
            {
                source[j] = source[j - 1];
                j--;
            }
            source[j] = x;
        }
    }
    
    public static void OrderByReferenceMode<TSource, TKey>(this ObservableCollection<TSource> source, 
        Func<TSource, TKey> sortFunc, SortMode? sortMode = null) where TKey : IComparable
    {
        InsertionSortDesc(source, sortFunc, sortMode);
    }
    
}