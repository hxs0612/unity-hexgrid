using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthGenerator : GeneratorInterface
{
    private int[,] mapData;
    private int width;
    private int height;
    private int landCount;
    private Factors factors;

    public int[,] GenerateMapData(int width, int height, Factors factors) {
        this.width = width;
        this.height = height;
        this.factors = factors;
        landCount = (int) Math.Floor(width * height * factors.landFactor);
        CreateOcean();
        CreateIce();
        CreatePlains();
        CreateCoast();
        CreateSnowAndTundra();
        CreateDesert();
        CreateGrassland();
        CreateWoods();
        CreateHills();
        CreateMountain();
        return mapData;
    }

    private void CreateOcean() {
        mapData = new int[width, height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                mapData[x, y] = Constants.TILE_OCEAN;
            }
        }
    }

    private void CreateIce() {
        for (int x = 0; x < width; x++) {
            mapData[x, 0] = Constants.TILE_ICE;
            mapData[x, 1] = GetRandom() < factors.iceFactor ? Constants.TILE_ICE : Constants.TILE_OCEAN;
            mapData[x, height - 2] = GetRandom() < factors.iceFactor ? Constants.TILE_ICE : Constants.TILE_OCEAN;
            mapData[x, height - 1] = Constants.TILE_ICE;
        }
    }

    private void CreatePlains() {
        List<Point> plainsRange = new List<Point>();
        int count = landCount;
        int targetLimit = (int) (count * factors.plainsPiecemeal);

        // 初始生长点
        for (int i = 0; i < factors.initialPlains; i++) {
            Point point = GetRandomPoint();
            AddTargetRange(plainsRange, point.x, point.y, Constants.TILE_OCEAN);
        }

        while(true) {
            int random = (int) Math.Floor(UnityEngine.Random.Range(0f, plainsRange.Count - 1f));
            Point target = plainsRange[random];
            int x = target.x;
            int y = target.y;
            plainsRange.RemoveAt(random);
            mapData[x, y] = Constants.TILE_PLAINS;
            count--;

            if (count <= 0) {
                break;
            }
            if (plainsRange.Count > targetLimit) {
                continue;
            }
            AddAround(plainsRange, x, y, Constants.TILE_OCEAN);
        }
    }

    private void CreateCoast() {
        List<Point> coastRange = new List<Point>();
        List<Point> coastRange2 = new List<Point>();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (mapData[x, y] == Constants.TILE_PLAINS) {
                    AddAround(coastRange, x, y, Constants.TILE_OCEAN);
                    AddAround2(coastRange2, x, y, Constants.TILE_OCEAN);
                }
            }
        }
        foreach (Point target in coastRange) {
            mapData[target.x, target.y] = Constants.TILE_COAST;
        }
        foreach (Point target in coastRange2) {
            if (GetRandom() < factors.oceanFactor) {
                mapData[target.x, target.y] = Constants.TILE_COAST;
            }
        }
    }

    private void CreateSnowAndTundra() {
        int snowHeight = (int) Math.Ceiling(height * factors.snowFactor);
        List<Point> snowRange = new List<Point>();
        List<Point> tundraRange = new List<Point>();
        for (int x = 0; x < width; x++) {
            int delta = 0;
            for (int y = 0; y < snowHeight; y++) {
                if (mapData[x, y] == Constants.TILE_PLAINS) {
                    if (GetRandom() < 1 - delta / snowHeight) {
                        mapData[x, y] = (GetRandom() > factors.tundraFactor) ? Constants.TILE_SNOW : Constants.TILE_TUNDRA;
                        delta++;
                        AddAround(snowRange, x, y, Constants.TILE_PLAINS);
                    } else {
                        break;
                    }
                }
            }
            delta = 0;
            for (int y = height - 1; y >= height - snowHeight; y--) {
                if (mapData[x, y] == Constants.TILE_PLAINS) {
                    if (GetRandom() < 1 - delta / snowHeight) {
                        mapData[x, y] = (GetRandom() > factors.tundraFactor) ? Constants.TILE_SNOW : Constants.TILE_TUNDRA;
                        delta++;
                        AddAround(snowRange, x, y, Constants.TILE_PLAINS);
                    } else {
                        break;
                    }
                }
            }
        }
        foreach (Point point in snowRange) {
            if (mapData[point.x, point.y] == Constants.TILE_PLAINS && GetRandom() > factors.tundraFactor) {
                mapData[point.x, point.y] = Constants.TILE_TUNDRA;
                AddAround(tundraRange, point.x, point.y, Constants.TILE_PLAINS);
            }
        }
        foreach (Point point in tundraRange) {
            if (mapData[point.x, point.y] == Constants.TILE_PLAINS && GetRandom() > factors.tundraFactor) {
                mapData[point.x, point.y] = Constants.TILE_TUNDRA;
            }
        }
    }

    private void CreateDesert() {
        List<Point> desertRange = new List<Point>();
        int count = (int) (landCount * factors.desertFactor);
        while (true) {
            Point point = desertRange.Count == 0 ? GetRandomPoint() : desertRange[(int) (desertRange.Count * GetRandom())];
            if (mapData[point.x, point.y] == Constants.TILE_PLAINS) {
                mapData[point.x, point.y] = GetRandom() > factors.oasisFactor ? Constants.TILE_DESERT : Constants.TILE_OASIS;
                desertRange.Remove(point);
                count--;
                if (count <= 0) {
                    break;
                }
                // 几率创建一个新的沙漠
                if (GetRandom() < factors.desertPiecemeal) {
                    desertRange = new List<Point>();
                    continue;
                }
                // 限制沙漠生长
                if (desertRange.Count > factors.maxDesertSize) {
                    continue;
                }
                AddAround(desertRange, point.x, point.y, Constants.TILE_PLAINS);
            }
        }
    }

    private void CreateGrassland() {
        List<Point> grasslandRange = new List<Point>();
        int count = (int) (landCount * factors.grasslandFactor);
        while (true) {
            Point point = grasslandRange.Count == 0 ? GetRandomPoint() : grasslandRange[(int) (grasslandRange.Count * GetRandom())];
            if (mapData[point.x, point.y] == Constants.TILE_PLAINS) {
                mapData[point.x, point.y] = GetRandom() > factors.marshFactor ? Constants.TILE_GRASSLAND : Constants.TILE_MARSH;
                grasslandRange.Remove(point);
                count--;
                if (count <= 0) {
                    break;
                }
                if (GetRandom() < factors.grasslandPiecemeal) {
                    grasslandRange = new List<Point>();
                    continue;
                }
                if (grasslandRange.Count > factors.maxGrasslandSize) {
                    continue;
                }
                AddAround(grasslandRange, point.x, point.y, Constants.TILE_PLAINS);
            }
        }
    }

    private void CreateWoods() {
        List<Point> woodsRange = new List<Point>();
        int count = (int) (landCount * factors.woodsFactor);
        while (true) {
            Point point = woodsRange.Count == 0 ? GetRandomPoint() : woodsRange[(int) (woodsRange.Count * GetRandom())];
            if (mapData[point.x, point.y] == Constants.TILE_PLAINS) {
                mapData[point.x, point.y] = (GetRandom() < factors.rainforestFactor) ? Constants.TILE_WOODS : Constants.TILE_RAINFOREST;
                woodsRange.Remove(point);
                count--;
                if (count <= 0) {
                    break;
                }
                if (GetRandom() < factors.woodsPiecemeal) {
                    woodsRange = new List<Point>();
                    continue;
                }
                if (woodsRange.Count > factors.maxWoodsSize) {
                    continue;
                }
                AddAround(woodsRange, point.x, point.y, Constants.TILE_PLAINS);
            }
        }
    }

    private void CreateHills() {
        int count = (int) (landCount * factors.hillsFactor);
        while (count > 0) {
            Point point = GetRandomPoint();
            switch (mapData[point.x, point.y]) {
                case Constants.TILE_PLAINS:
                    mapData[point.x, point.y] = Constants.TILE_PLAINS_HILLS;
                    count--;
                    break;
                case Constants.TILE_SNOW:
                    mapData[point.x, point.y] = Constants.TILE_SNOW_HILLS;
                    count--;
                    break;
                case Constants.TILE_TUNDRA:
                    mapData[point.x, point.y] = Constants.TILE_TUNDRA_HILLS;
                    count--;
                    break;
                case Constants.TILE_DESERT:
                    mapData[point.x, point.y] = Constants.TILE_DESERT_HILLS;
                    count--;
                    break;
                case Constants.TILE_GRASSLAND:
                    mapData[point.x, point.y] = Constants.TILE_GRASSLAND_HILLS;
                    count--;
                    break;
            }
        }
    }

    private void CreateMountain() {
        List<Point> mountainRange = new List<Point>();
        int count = (int) (landCount * factors.mountainFactor);
        while (true) {
            Point point = mountainRange.Count == 0 ? GetRandomPoint() : mountainRange[(int) (mountainRange.Count * GetRandom())];
            if (mapData[point.x, point.y] == Constants.TILE_PLAINS) {
                mapData[point.x, point.y] = Constants.TILE_MOUNTAINS;
                mountainRange.Remove(point);
                count--;
                if (count <= 0) {
                    break;
                }
                if (GetRandom() < factors.mountainPiecemeal) {
                    mountainRange = new List<Point>();
                    continue;
                }
                if (mountainRange.Count > factors.maxMoutainSize) {
                    continue;
                }
                AddAround(mountainRange, point.x, point.y, Constants.TILE_PLAINS);
            }
        }
    }

    private double GetRandom() {
        double random = UnityEngine.Random.Range(0f, 1f);
        return random;
    }

    private Point GetRandomPoint() {
        return new Point((int) (width * GetRandom()), (int) ((height - 1) * GetRandom()));
    }

    private void AddTargetRange(List<Point> targetRange, int x, int y, int type) {
        x = (x + width) % width;
        if (y < 0 || y >= height) {
            return;
        }
        if (mapData[x, y] == type) {
            Point target = new Point(x, y);
            if (!targetRange.Contains(target)) {
                targetRange.Add(target);
            }
        }
    }

    private void AddAround(List<Point> targetRange, int x, int y, int type) {
        if (y % 2 == 0) {
            AddTargetRange(targetRange, x + 1, y, type);
            AddTargetRange(targetRange, x, y - 1, type);
            AddTargetRange(targetRange, x - 1, y - 1, type);
            AddTargetRange(targetRange, x - 1, y, type);
            AddTargetRange(targetRange, x - 1, y + 1, type);
            AddTargetRange(targetRange, x, y + 1, type);
        } else {
            AddTargetRange(targetRange, x + 1, y, type);
            AddTargetRange(targetRange, x + 1, y - 1, type);
            AddTargetRange(targetRange, x, y - 1, type);
            AddTargetRange(targetRange, x - 1, y, type);
            AddTargetRange(targetRange, x, y + 1, type);
            AddTargetRange(targetRange, x + 1, y + 1, type);
        }
    }

    private void AddAround2(List<Point> targetRange, int x, int y, int type) {
        if (y % 2 == 0) {
            AddTargetRange(targetRange, x + 2, y, type);
            AddTargetRange(targetRange, x + 1, y - 1, type);
            AddTargetRange(targetRange, x + 1, y - 2, type);
            AddTargetRange(targetRange, x, y - 2, type);
            AddTargetRange(targetRange, x - 1, y - 2, type);
            AddTargetRange(targetRange, x - 2, y - 1, type);
            AddTargetRange(targetRange, x - 2, y, type);
            AddTargetRange(targetRange, x - 2, y + 1, type);
            AddTargetRange(targetRange, x - 1, y + 2, type);
            AddTargetRange(targetRange, x, y + 2, type);
            AddTargetRange(targetRange, x + 1, y + 2, type);
            AddTargetRange(targetRange, x + 1, y + 1, type);
        } else {
            AddTargetRange(targetRange, x + 2, y, type);
            AddTargetRange(targetRange, x + 2, y - 1, type);
            AddTargetRange(targetRange, x + 1, y - 2, type);
            AddTargetRange(targetRange, x, y - 2, type);
            AddTargetRange(targetRange, x - 1, y - 2, type);
            AddTargetRange(targetRange, x - 1, y - 1, type);
            AddTargetRange(targetRange, x - 2, y, type);
            AddTargetRange(targetRange, x - 1, y + 1, type);
            AddTargetRange(targetRange, x - 1, y + 2, type);
            AddTargetRange(targetRange, x, y + 2, type);
            AddTargetRange(targetRange, x + 1, y + 2, type);
            AddTargetRange(targetRange, x + 2, y + 1, type);
        }
    }
}
