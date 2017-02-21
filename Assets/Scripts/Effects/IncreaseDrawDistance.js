var detailDistance: int = 250;
var basemapDistance: int = 2000;
var refreshEveryFrame : boolean = false;

function Start ()
{
		Terrain.activeTerrain.detailObjectDistance = detailDistance;
		Terrain.activeTerrain.basemapDistance = basemapDistance;
}

function Update()
{
    if (refreshEveryFrame)
    {
        if (Terrain.activeTerrain.detailObjectDistance != detailDistance)
        {
            Terrain.activeTerrain.detailObjectDistance = detailDistance;
        }
        if (Terrain.activeTerrain.basemapDistance != basemapDistance)
        {
            Terrain.activeTerrain.basemapDistance = basemapDistance;
        }
    }
}