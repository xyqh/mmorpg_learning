using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICreatureInfo : MonoBehaviour {

    public Slider sliderHP;
    public Slider sliderMP;
    public Text textHP;
    public Text textMP;
    public Text userName;

    private Creature target;
    public Creature Target
    {
        get { return target; }
        set
        {
            this.target = value;
            this.UpdateUI();
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.UpdateUI();
	}

    void UpdateUI()
    {
        if (this.target == null) return;
        this.userName.text = string.Format("{0} Lv.{1}", target.Name, target.Info.Level);

        this.sliderHP.maxValue = this.target.Attributes.MaxHP;
        this.sliderHP.value = this.target.Attributes.HP;
        this.textHP.text = string.Format("{0}/{1}", this.target.Attributes.HP, this.target.Attributes.MaxHP);

        this.sliderMP.maxValue = this.target.Attributes.MaxMP;
        this.sliderMP.value = this.target.Attributes.MP;
        this.textMP.text = string.Format("{0}/{1}", this.target.Attributes.MP, this.target.Attributes.MaxMP);
    }
}
