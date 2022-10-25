using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Text;
using System;
using System.IO;

using Common.Data;

using Newtonsoft.Json;

public class DataManager : Singleton<DataManager>
{
    public string DataPath;
    private Dictionary<int, MapDefine> Maps = null;
    private Dictionary<int, CharacterDefine> Characters = null;
    private Dictionary<int, TeleporterDefine> Teleporters = null;
    private Dictionary<int, NpcDefine> NPCs = null;
    private Dictionary<int, ItemDefine> Items = null;
    private Dictionary<int, Dictionary<int, SpawnPointDefine>> SpawnPoints = null;
    public Dictionary<int, ShopDefine> Shops = null;
    public Dictionary<int, Dictionary<int, ShopItemDefine>> ShopItems = null;
    public Dictionary<int, EquipDefine> Equips = null;
    public Dictionary<int, Dictionary<int, SkillDefine>> Skills = null;
    public Dictionary<int, BuffDefine> Buffs = null;
    public Dictionary<int, SkillDefine> SkillMap = null;

    public Dictionary<int, MapDefine> IMaps
    {
        get
        {
            return Maps;
        }
    }
    public Dictionary<int, CharacterDefine> ICharacters
    {
        get
        {
            return Characters;
        }
    }
    public Dictionary<int, TeleporterDefine> ITeleporters
    {
        get
        {
            return Teleporters;
        }
    }
    public Dictionary<int, NpcDefine> INPCs
    {
        get
        {
            return NPCs;
        }
    }
    public Dictionary<int, Dictionary<int, SpawnPointDefine>> ISpawnPoints
    {
        get
        {
            return SpawnPoints;
        }

        set
        {
            SpawnPoints = value;
        }
    }

    public Dictionary<int, ItemDefine> IItems
    {
        get
        {
            return Items;
        }
    }

    public Dictionary<int, ShopDefine> IShops
    {
        get
        {
            return Shops;
        }
    }

    public Dictionary<int, Dictionary<int, ShopItemDefine>> IShopItems
    {
        get
        {
            return ShopItems;
        }
    }

    public Dictionary<int, EquipDefine> IEquips
    {
        get
        {
            return Equips;
        }
    }

    public Dictionary<int, Dictionary<int, SkillDefine>> ISkills
    {
        get
        {
            return Skills;
        }
    }

    public Dictionary<int, BuffDefine> IBuffs
    {
        get
        {
            return Buffs;
        }
    }

    public Dictionary<int, SkillDefine> ISkillMap
    {
        get
        {
            return SkillMap;
        }
    }


    public DataManager()
    {
        this.DataPath = "Data/";
        Debug.LogFormat("DataManager > DataManager()");
    }

    public void Load()
    {
        string json = File.ReadAllText(this.DataPath + "MapDefine.txt");
        this.Maps = JsonConvert.DeserializeObject<Dictionary<int, MapDefine>>(json);

        json = File.ReadAllText(this.DataPath + "CharacterDefine.txt");
        this.Characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterDefine>>(json);

        json = File.ReadAllText(this.DataPath + "TeleporterDefine.txt");
        this.Teleporters = JsonConvert.DeserializeObject<Dictionary<int, TeleporterDefine>>(json);
        
        json = File.ReadAllText(this.DataPath + "NpcDefine.txt");
        this.NPCs = JsonConvert.DeserializeObject<Dictionary<int, NpcDefine>>(json);

        json = File.ReadAllText(this.DataPath + "ItemDefine.txt");
        this.Items = JsonConvert.DeserializeObject<Dictionary<int, ItemDefine>>(json);

        json = File.ReadAllText(this.DataPath + "SpawnPointDefine.txt");
        this.SpawnPoints = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, SpawnPointDefine>>> (json);

        json = File.ReadAllText(this.DataPath + "ShopDefine.txt");
        this.Shops = JsonConvert.DeserializeObject<Dictionary<int, ShopDefine>>(json);

        json = File.ReadAllText(this.DataPath + "ShopItemDefine.txt");
        this.ShopItems = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, ShopItemDefine>>>(json);

        json = File.ReadAllText(this.DataPath + "EquipDefine.txt");
        this.Equips = JsonConvert.DeserializeObject<Dictionary<int, EquipDefine>>(json);

        json = File.ReadAllText(this.DataPath + "SkillDefine.txt");
        this.Skills = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, SkillDefine>>>(json);

        json = File.ReadAllText(this.DataPath + "BuffDefine.txt");
        this.Buffs = JsonConvert.DeserializeObject<Dictionary<int, BuffDefine>>(json);

        this.initSkillMap();
    }


    public IEnumerator LoadData()
    {
        string json = File.ReadAllText(this.DataPath + "MapDefine.txt");
        this.Maps = JsonConvert.DeserializeObject<Dictionary<int, MapDefine>>(json);

        yield return null;

        json = File.ReadAllText(this.DataPath + "CharacterDefine.txt");
        this.Characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterDefine>>(json);

        yield return null;

        json = File.ReadAllText(this.DataPath + "TeleporterDefine.txt");
        this.Teleporters = JsonConvert.DeserializeObject<Dictionary<int, TeleporterDefine>>(json);

        yield return null;

        json = File.ReadAllText(this.DataPath + "NpcDefine.txt");
        this.NPCs = JsonConvert.DeserializeObject<Dictionary<int, NpcDefine>>(json);

        yield return null;
        
        json = File.ReadAllText(this.DataPath + "ItemDefine.txt");
        this.Items = JsonConvert.DeserializeObject<Dictionary<int, ItemDefine>>(json);

        yield return null;

        json = File.ReadAllText(this.DataPath + "SpawnPointDefine.txt");
        this.SpawnPoints = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, SpawnPointDefine>>>(json);

        yield return null;

        json = File.ReadAllText(this.DataPath + "ShopDefine.txt");
        this.Shops = JsonConvert.DeserializeObject<Dictionary<int, ShopDefine>>(json);

        yield return null;

        json = File.ReadAllText(this.DataPath + "ShopItemDefine.txt");
        this.ShopItems = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, ShopItemDefine>>>(json);

        yield return null;

        json = File.ReadAllText(this.DataPath + "EquipDefine.txt");
        this.Equips = JsonConvert.DeserializeObject<Dictionary<int, EquipDefine>>(json);

        yield return null;

        json = File.ReadAllText(this.DataPath + "SkillDefine.txt");
        this.Skills = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, SkillDefine>>>(json);
        yield return null;

        json = File.ReadAllText(this.DataPath + "BuffDefine.txt");
        this.Buffs = JsonConvert.DeserializeObject<Dictionary<int, BuffDefine>>(json);
        yield return null;

        this.initSkillMap();
    }

    void initSkillMap()
    {
        this.SkillMap = new Dictionary<int, SkillDefine>();
        foreach(var kv in this.Skills)
        {
            foreach(var _kv in kv.Value)
            {
                SkillMap[_kv.Key] = _kv.Value;
            }
        }
    }

#if UNITY_EDITOR
    public void SaveTeleporters()
    {
        string json = JsonConvert.SerializeObject(this.Teleporters, Formatting.Indented);
        File.WriteAllText(this.DataPath + "TeleporterDefine.txt", json);
    }

    public void SaveSpawnPoints()
    {
        string json = JsonConvert.SerializeObject(this.SpawnPoints, Formatting.Indented);
        File.WriteAllText(this.DataPath + "SpawnPointDefine.txt", json);
    }

#endif
}
