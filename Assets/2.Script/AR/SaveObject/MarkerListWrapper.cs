using System.Collections.Generic;


[System.Serializable]
public class MarkerListWrapper
{
    public List<MarkerData> markerDatas;
}

public class ServerMarkerListWrapper
{
    public ServerMarkerData[] markers;
}