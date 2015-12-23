/*
using System;
using System.Collections.Generic;
using InfoService.RecentlyAddedWatched.Data;

namespace InfoService.RecentlyAddedWatched.Interfaces
{
    interface IRecentlyAddedWatchedProvider<T> where T : RecentlyItem
    {
        List<T> GetRecentlyAdded();
        List<T> GetRecentlyWatched();

        event EventHandler<NewItemHandler<T>> OnNewItem;
        //event NewItemHandler OnNewItem;
        //delegate void NewItemHandler(object sender, T item);
    }

    //public delegate void NewItemHandler(object sender, T item);
}
*/