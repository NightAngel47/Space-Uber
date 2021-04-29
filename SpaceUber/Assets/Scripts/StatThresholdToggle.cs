using System;

[Serializable]
public class StatThresholdToggle
{
    public ResourceDataTypes stat;
    public int threshold;

    public bool MeetsThreshold(ShipStats ship)
    {
        int shipStat = 0;
        switch (stat)
        {
            case ResourceDataTypes._HullDurability:
                shipStat = (int)ship.ShipHealthCurrent.x;
                break;
            case ResourceDataTypes._Energy:
                shipStat = (int)ship.Energy.x;
                break;
            case ResourceDataTypes._Crew:
                shipStat = (int)ship.CrewCurrent.x;
                break;
            case ResourceDataTypes._Food:
                shipStat = ship.Food;
                break;
            case ResourceDataTypes._FoodPerTick:
                shipStat = ship.FoodPerTick;
                break;
            case ResourceDataTypes._ShipWeapons:
                shipStat = ship.ShipWeapons;
                break;
            case ResourceDataTypes._Security:
                shipStat = ship.Security;
                break;
            case ResourceDataTypes._CrewMorale:
                shipStat = MoraleManager.instance.CrewMorale;
                break;
            case ResourceDataTypes._Credits:
                shipStat = ship.Credits;
                break;
            case ResourceDataTypes._Payout:
                shipStat = ship.Payout;
                break;
            default:
                UnityEngine.Debug.LogError("Stat not specified in StatThresholdToggle");
                break;
        }

        return shipStat >= threshold;
    }
}
