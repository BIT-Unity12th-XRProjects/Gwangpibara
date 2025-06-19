using System;

public static class UpdateEvents
{
    public static event Action<string, string> OnMarkerStringDataUpdated;
    public static event Action<int, int> OnMarkerIntDataUpdated;
    public static event Action<MarkerSpawnType, MarkerSpawnType> OnMarkerMarkerSpawnTypeDataUpdated;
    public static event Action<MarkerType, MarkerType> OnMarkerMarkerTypeDataUpdated;
    
    public static void UpdateMarkerStringData(string oldData, string newData)
    {
        OnMarkerStringDataUpdated?.Invoke(oldData, newData);
    }

    public static void UpdateMarkerintData(int oldData, int newData)
    {
        OnMarkerIntDataUpdated?.Invoke(oldData, newData);
    }

    public static void UpdateMarkerSpawnTypeData(MarkerSpawnType oldData, MarkerSpawnType newData)
    {
        OnMarkerMarkerSpawnTypeDataUpdated?.Invoke(oldData, newData);
    }

    public static void UpdateMarkerTypeData(MarkerType oldData, MarkerType newData)
    {
        OnMarkerMarkerTypeDataUpdated?.Invoke(oldData, newData);
    }

}