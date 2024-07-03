using System;
using System.Collections.Generic;

public interface IItemsDataProvider
{
    void LoadItems(Action<Dictionary<string, Item>> loaded);
}