using System.Collections.Generic;
using UnityEngine;
using AillieoUtils;
using UnityEngine.UI;

public class MyTest : MonoBehaviour
{
    public struct RankItemData
    {
        // 名次
        public int rank;
        // 名字
        public string name;
    }

    List<RankItemData> testData = new List<RankItemData>();

    public ScrollView scrollView;

    private void Start()
    {
        // 构造测试数据
        InitData();


        scrollView.SetUpdateFunc((index, rectTransform) =>
        {
            // 更新item的UI元素
            RankItemData data = testData[index];
            rectTransform.gameObject.SetActive(true);
            rectTransform.Find("rankText").GetComponent<Text>().text = data.rank.ToString();
            rectTransform.Find("nameText").GetComponent<Text>().text = data.name;
            Button btn = rectTransform.Find("Button").GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(()=>{
                Debug.Log(data.name);
            });
            // RectTransform  bg = rectTransform.Find("bg").GetComponent<RectTransform>();
            // bg.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y - 4);
        });
        scrollView.SetItemSizeFunc((index) =>
        {
            // 返回item的尺寸
            RankItemData data = testData[index];
            if(data.rank <= 3)
            {
                return new Vector2(822, 180);
            }
            else
            {
                return new Vector2(822, 100);
            }
        });
        scrollView.SetItemCountFunc(() =>
        {
            // 返回数据列表item的总数
            return testData.Count;
        });

        scrollView.UpdateData(false);
        
    }

    private void InitData()
    {
        // 构建50000个排名数据
        for (int i = 1; i <= 1000; ++i)
        {
            RankItemData data = new RankItemData();
            data.rank = i;
            data.name = "Name_" + i;
            testData.Add(data);
        }
    }
}
