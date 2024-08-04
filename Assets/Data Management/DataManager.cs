using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using System.IO;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Networking;


public class JSkillUIData
{
    public string[] skillName;
    public string[] skillDescription;
    public Sprite[] skillSprite;

    public JSkillUIData(string[] _skillName, string[] _skillDescription, Sprite[] _skillSprite)
    {
        this.skillName = _skillName;
        this.skillDescription = _skillDescription;
        this.skillSprite = _skillSprite;
    }
}

public class JSkillAbilityData
{
    //Ability By Level
    public float[] skillImpactValueOfLevel;
    public float[] attackIntervalOfLevel;
    public float[] speedOfLevel;
    public float[] attackRangOfLevel;
    public int[] attackCountOfOneTimeOfLevel;
    public float[] skillDurationOfLevel;

    public JSkillAbilityData(float[] _skillImpactValueOfLevel, float[] _attackIntervalOfLevel, 
        float[] _speedOfLevel, float[] _attackRangOfLevel, int[] _attackCountOfOneTimeOfLevel, float[] _skillDurationOfLevel)
    {
        this.skillImpactValueOfLevel = _skillImpactValueOfLevel;
        this.attackIntervalOfLevel = _attackIntervalOfLevel;
        this.speedOfLevel = _speedOfLevel;
        this.attackRangOfLevel = _attackRangOfLevel;
        this.attackCountOfOneTimeOfLevel = _attackCountOfOneTimeOfLevel;
        this.skillDurationOfLevel = _skillDurationOfLevel;
    }
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public event Action OnDataLoadComplete;

    [Header("Skill UI Value Json Data + Skill Sprite Addressables Data")]
    [SerializeField] private AssetReference[] assetReferenceSpriteLevel0;
    [SerializeField] private AssetReference[] assetReferenceSpriteLevel1;

    [Header("Skill UI Json Data")] //제이슨 파일 + 유니티 내 
    readonly Dictionary<int, JSkillUIData> skillUIDictionary = new Dictionary<int, JSkillUIData>();

    [Header("Skill Ability Value Json Data")]//제이슨 파일
    readonly Dictionary<string, JSkillAbilityData> skillAbilityDictionary = new Dictionary<string, JSkillAbilityData>();

    [Header("Link Skill Chart Json Data")] //제이슨 파일
    readonly List<bool>[] linkSkillCheckList = new List<bool>[28]; //(int)PlayerSkillCategory.WEAPON_SWORD + 1


    private void Awake()
    {
        Instance = this;
    }

    public IReadOnlyDictionary<int, JSkillUIData> SkillUIDataDictionary
    {
        get { return skillUIDictionary; }
    }

    public IReadOnlyDictionary<string, JSkillAbilityData> SkillAbilityDataDictionary
    {
        get { return skillAbilityDictionary; }
    }

    public IReadOnlyList<bool>[] LinkSkillCheckDataDictionary
    {
        get { return linkSkillCheckList; }
    }

    public void LoadData()
    {
        LoadAndParseJson("/SkillAbilityData.json", ParsingJsonAbilityData);

        LoadAndParseJson("/LinkSkillChartData.json", ParsingJsonLinkSkillChartData);

        LoadAndParseJson("/SkillUIData.json", ParsingJsonSkillUIData);

    }

    //불러오기
    void LoadAndParseJson(string _detailFilePath, Action<JsonData> parseMethod) 
    {
        JsonData jsonData = null;
        string JsonString;
        string filePath = Application.streamingAssetsPath + _detailFilePath;

        if (Application.platform == RuntimePlatform.Android)
        {
            UnityWebRequest reader = UnityWebRequest.Get(filePath);

            if (reader.result != UnityWebRequest.Result.Success)
                return;
            else
                JsonString = reader.downloadHandler.text;
        }
        else
            JsonString = File.ReadAllText(filePath);

        jsonData = JsonMapper.ToObject(JsonString);
        parseMethod?.Invoke(jsonData);
    }

    //데이터 파싱
    void ParsingJsonAbilityData(JsonData _data)
    {
        int[] startIndex = { 1, 2, 3, 4, 5 , 6};

        for (int i = 0; i < _data.Count; i++)
        {
            string skillAbilityID = _data[i][0].ToString();

            string[] skillImpactValue = new string[6];
            string[] attackInterval = new string[6];
            string[] speed = new string[6];
            string[] attackRang = new string[6];
            string[] attackCountOfOneTime = new string[6];
            string[] skillDuration = new string[6];

            for (int j = 0; j < 6; j++)
            {
                skillImpactValue[j] = _data[i][startIndex[0] + (j * 6)].ToString();
                attackInterval[j] = _data[i][startIndex[1] + (j * 6)].ToString();
                speed[j] = _data[i][startIndex[2] + (j * 6)].ToString();
                attackRang[j] = _data[i][startIndex[3] + (j * 6)].ToString();
                attackCountOfOneTime[j] = _data[i][startIndex[4] + (j * 6)].ToString();
                skillDuration[j] = _data[i][startIndex[5] + (j * 6)].ToString();
            }

            float[] tempSkillImpactValue = new float[6];
            float[] tempAttackInterval = new float[6];
            float[] tempSpeed = new float[6];
            float[] tempAttackRang = new float[6];
            int[] tempAttackCountOfOneTime = new int[6];
            float[] tempSkillDuration = new float[6];

            for (int j = 0; j < 6; j++)
            {
                tempSkillImpactValue[j] = float.Parse(skillImpactValue[j]);
                tempAttackInterval[j] = float.Parse(attackInterval[j]);
                tempSpeed[j] = float.Parse(speed[j]);
                tempAttackRang[j] = float.Parse(attackRang[j]);
                tempAttackCountOfOneTime[j] = int.Parse(attackCountOfOneTime[j]);
                tempSkillDuration[j] = float.Parse(skillDuration[j]);
            }

            JSkillAbilityData skillAbiltyData =
                new JSkillAbilityData(tempSkillImpactValue, tempAttackInterval, tempSpeed, tempAttackRang, tempAttackCountOfOneTime, tempSkillDuration);

            skillAbilityDictionary.Add(skillAbilityID, skillAbiltyData);
        }
    }

    void ParsingJsonLinkSkillChartData(JsonData _data)
    {
        for (int i = 0; i < _data.Count; i++)
        {
            linkSkillCheckList[i] = new List<bool>();
        }

        for (int i = 0; i < _data.Count; i++) //_data.Count: 28
        {
            string[] tempReqiurdSkillType = new string[_data.Count];

            for (int j = 0; j < _data.Count; j++)
            {
                tempReqiurdSkillType[j] = _data[i][j+1].ToString();
            }

            bool isReqiredThisSkill;
            for (int j = 0; j < _data.Count; j++)
            {
                int tempTransTo = int.Parse(tempReqiurdSkillType[j]);

                isReqiredThisSkill = (tempTransTo == 1)? true : false;

                linkSkillCheckList[i].Add(isReqiredThisSkill);
            }
        }
    }

    void ParsingJsonSkillUIData(JsonData _data)
    {
        for (int i = 0; i < _data.Count; i++)
        {
            string skillUIID = _data[i][0].ToString();

            string Lev0_skillDescription = _data[i][1].ToString();
            string Lev1_skillDescription = _data[i][2].ToString();
            string Lev2_skillDescription = _data[i][3].ToString();
            string Lev3_skillDescription = _data[i][4].ToString();
            string Lev4_skillDescription = _data[i][5].ToString();
            string Lev5_skillDescription = _data[i][6].ToString();

            string skillNameA = _data[i][7].ToString();
            string skillNameB = _data[i][8].ToString();

            int tempskillUIID = int.Parse(skillUIID);

            string[] skillDescription = new string[6];
            skillDescription[0] = Lev0_skillDescription;
            skillDescription[1] = Lev1_skillDescription;
            skillDescription[2] = Lev2_skillDescription;
            skillDescription[3] = Lev3_skillDescription;
            skillDescription[4] = Lev4_skillDescription;
            skillDescription[5] = Lev5_skillDescription;

            string[] skillName = new string[2];
            skillName[0] = skillNameA;
            skillName[1] = skillNameB;

            Sprite[] skillSprite = new Sprite[2];
            skillSprite[0] = null;
            skillSprite[1] = null;

            JSkillUIData skillUIData = new JSkillUIData(skillName , skillDescription, skillSprite);
            skillUIDictionary.Add(tempskillUIID, skillUIData);
        }

        //Sprite Data 넣기
        StartCoroutine(LoadAndParseSkillUIData());
    }

    IEnumerator LoadAndParseSkillUIData()
    {
        Dictionary<int, AsyncOperationHandle<Texture2D>[]> skillSpriteHandler = new Dictionary<int, AsyncOperationHandle<Texture2D>[]>();

        int index = 0;
        foreach (var asset in skillUIDictionary)
        {
            AsyncOperationHandle<Texture2D>[] temp = LoadTexture2DAsync(index, asset.Key);

            skillSpriteHandler.Add(asset.Key, temp);

            index++;
        }

        yield return WaitForAll(skillSpriteHandler);

        foreach (var asset in skillUIDictionary)
        {
            if (skillSpriteHandler.TryGetValue(asset.Key, out AsyncOperationHandle<Texture2D>[] handles))
            {
                for (int i = 0; i < 2; ++i)
                {
                    if (handles[i].IsValid() && handles[i].Status == AsyncOperationStatus.Succeeded)
                    {
                        float pixelsPerUnit = 200f; //플레이어 무기 제외하고 모두 UI만 사용- pixelsPerUnit 수정 필요X
                        if (asset.Key >= (int)PlayerSkillCategory.TOP_WEAPON_BAZOOKA && asset.Key <= (int)PlayerSkillCategory.WEAPON_SWORD)
                            skillUIDictionary[asset.Key].skillSprite[i] 
                                = Sprite.Create(handles[i].Result, new Rect(0, 0, handles[i].Result.width, handles[i].Result.height), Vector2.one * 0.5f, pixelsPerUnit);
                        else
                            skillUIDictionary[asset.Key].skillSprite[i] 
                                = Sprite.Create(handles[i].Result, new Rect(0, 0, handles[i].Result.width, handles[i].Result.height), Vector2.one * 0.5f);

                        skillUIDictionary[asset.Key].skillSprite[i].name = handles[i].Result.name;
                    }
                    else
                        continue;
                }
            }
        }

        UnloadAddressables(skillSpriteHandler);

        OnDataLoadComplete?.Invoke();
    }

    AsyncOperationHandle<Texture2D>[] LoadTexture2DAsync(int _index, int _assetKey)
    {
        AsyncOperationHandle<Texture2D>[] textureHandle = new AsyncOperationHandle<Texture2D>[2];

        bool isOnlyExistLevel0Type = _assetKey < (int)PlayerSkillCategory.TOP_ACTIVE_BOOMERANG || _assetKey >= (int)PlayerSkillCategory.TOP_BONUS_GOLD;

        textureHandle[0] = assetReferenceSpriteLevel0[_index].LoadAssetAsync<Texture2D>();

        if (!isOnlyExistLevel0Type)
            textureHandle[1] = assetReferenceSpriteLevel1[_index - 12].LoadAssetAsync<Texture2D>();
        else
            textureHandle[1] = default;

        return textureHandle;
    }

    IEnumerator WaitForAll(Dictionary<int, AsyncOperationHandle<Texture2D>[]> handles)
    {
        List<AsyncOperationHandle<Texture2D>> checkingHandle = new List<AsyncOperationHandle<Texture2D>>();

        foreach (var handleArray in handles.Values)
        {
            if (handleArray[0].IsValid())
                checkingHandle.Add(handleArray[0]); 

            if (handleArray.Length > 1 && handleArray[1].IsValid())
                checkingHandle.Add(handleArray[1]);
        }

        foreach (var handle in checkingHandle)
        {
            yield return handle;
        }
    }

    void UnloadAddressables(Dictionary<int, AsyncOperationHandle<Texture2D>[]> handles)
    {
        foreach (var handleArray in handles.Values)
        {
            if (handleArray[0].IsValid())
                Addressables.Release(handleArray[0]);

            if (handleArray.Length > 1 && handleArray[1].IsValid())
                Addressables.Release(handleArray[1]);
        }
    }
}