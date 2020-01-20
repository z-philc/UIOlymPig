using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewConfig
{
    public static ViewIndex[] viewIndices = {
        ViewIndex.EmptyView,
        ViewIndex.HomeView,
        ViewIndex.LoadingView,
        ViewIndex.LevelView
    };
}

public enum ViewIndex
{
    EmptyView = 0,
    HomeView = 1,
    LoadingView = 2,
    LevelView
}

public class ViewParam
{

}
public class HomeViewParam : ViewParam
{

}
