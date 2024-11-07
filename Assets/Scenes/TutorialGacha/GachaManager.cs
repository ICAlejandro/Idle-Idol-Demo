using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using TMPro;

public class GachaManager : MonoBehaviour
{
    //Gacha
    [SerializeField] private GachaRate[] gacha;
    public cardInfo[] _reward;
    [SerializeField] private Transform parent, pos;
    [SerializeField] private GameObject characterCardGO;
    GameObject characterCard;
    Cards card;

    //For Rate Up Gacha System Only
    [SerializeField] private cardInfo[] rateUpReward;
    [Range(1,10)] [SerializeField] private int rateUpRate;

    //For Soft Pity System Only
    [SerializeField] private int LegendSoftPity;
    private int[] NormalRate;

    //For Guaranteed System Only
    private int guaranteedPull = 10;
    [SerializeField] private TextMeshProUGUI pullLeft;

    private void Start() 
    {
        //soft pity
        //save the normal rate for each rarity
        NormalRate = new int[gacha.Length];
        for(int i=0; i<gacha.Length;i++)
        {
            NormalRate[i] = gacha[i].rate;
        }

        //guaranteed
        pullLeft.text = "Guaranteed in " + guaranteedPull + " Pulls";
    }

    private void Update() 
    {
        pullLeft.text = "Guaranteed in " + guaranteedPull + " Pulls";
    }

    public void Gacha()
    {
        GuaranteedGachaPull();
    }

    #region Simple Gacha
    void SimpleGacha()
    {
        if(characterCard == null)
        {
            characterCard = Instantiate(characterCardGO, pos.position, Quaternion.identity) as GameObject;
            characterCard.transform.SetParent(parent);
            characterCard.transform.localScale = new Vector3(1,1,1);
            card = characterCard.GetComponent<Cards>();
        }

        int rnd = UnityEngine.Random.Range(1,101);
        for(int i=0;i<gacha.Length;i++)
        {
            if(rnd <= gacha[i].rate)
            {
                card.card = Reward(gacha[i]._rarity);
                return;
            }
        }
    }
    #endregion

    #region Rate Up Gacha
    void RateUpGacha()
    {
        if(characterCard == null)
        {
            characterCard = Instantiate(characterCardGO, pos.position, Quaternion.identity) as GameObject;
            characterCard.transform.SetParent(parent);
            characterCard.transform.localScale = new Vector3(1,1,1);
            card = characterCard.GetComponent<Cards>();
        }

        int rnd = UnityEngine.Random.Range(1,101);
        for(int i=0;i<gacha.Length;i++)
        {
            if(rnd <= gacha[i].rate)
            {
                card.card = RatesUpReward(gacha[i]._rarity);
                return;
            }
        }
    }
    #endregion

    #region Gacha w/ Soft Pity
    void SoftPityGacha()
    {
        if(characterCard == null)
        {
            characterCard = Instantiate(characterCardGO, pos.position, Quaternion.identity) as GameObject;
            characterCard.transform.SetParent(parent);
            characterCard.transform.localScale = new Vector3(1,1,1);
            card = characterCard.GetComponent<Cards>();
        }

        int rnd = UnityEngine.Random.Range(1,101);
        for(int i=0;i<gacha.Length;i++)
        {
            if(rnd <= gacha[i].rate)
            {
                Debug.Log(gacha[i].rarity);
                if(gacha[i]._rarity != Rarity.Legend)
                {
                    AddRate();
                }
                else
                {
                    refreshGachaRate();
                }
                card.card = Reward(gacha[i]._rarity);
                return;
            }
        }
    }
    #endregion

    #region Guaranteed Pull
    void GuaranteedGachaPull()
    {
        if(characterCard == null)
        {
            characterCard = Instantiate(characterCardGO, pos.position, Quaternion.identity) as GameObject;
            characterCard.transform.SetParent(parent);
            characterCard.transform.localScale = new Vector3(1,1,1);
            card = characterCard.GetComponent<Cards>();
        }

        int rnd = UnityEngine.Random.Range(1,101);
        for(int i=0;i<gacha.Length;i++)
        {
            if(rnd <= gacha[i].rate)
            {
                //kalo ga dpt legend guarantee pulls dikurangi terus, kalo dah dapet balik ke awal
                if(gacha[i]._rarity != Rarity.Legend)
                    guaranteedPull--;
                else
                    guaranteedPull = 10;

                if(guaranteedPull == 0)
                {
                    Debug.Log("Legendary");
                    guaranteedPull = 10;
                    card.card = Reward(Rarity.Legend);
                    return;
                }
                Debug.Log(gacha[i].rarity);
                card.card = Reward(gacha[i]._rarity);
                return;
            }
        }
    }
    #endregion
    
    #region Multiple Pulls
    public void MultiplePullsGacha(int pulls)
    {
        for(int i=0;i<pulls;i++)
        {
            characterCard = Instantiate(characterCardGO, pos.position, Quaternion.identity) as GameObject;
            characterCard.transform.SetParent(parent);
            characterCard.transform.localScale = new Vector3(1,1,1);
            card = characterCard.GetComponent<Cards>();

            int rnd = UnityEngine.Random.Range(1,101);
            for(int j=0;i<gacha.Length;j++)
            {
                if(rnd <= gacha[j].rate)
                {
                    card.card = Reward(gacha[j]._rarity);
                    break;
                }
            }
        }
    }
    #endregion

    void AddRate()
    {
        for(int i=0;i<gacha.Length;i++)
        {
            if(gacha[i].rate != 100)
                gacha[i].rate += LegendSoftPity;
        }
    }

    //for refresh the rate
    void refreshGachaRate()
    {
        for(int i=0;i<gacha.Length;i++)
        {
            gacha[i].rate = NormalRate[i];
        }
    }

    public int Rates(Rarity rarity)
    {
        GachaRate gr = Array.Find(gacha, rt => rt._rarity == rarity);
        if(gr!=null)
        {
            return gr.rate;
        }
        else
        {
            return 0;
        }
    }

    cardInfo Reward(Rarity rarity)
    {
        cardInfo[] reward = Array.FindAll(_reward, r=>r.rarity == rarity);
        int rnd = UnityEngine.Random.Range(0,reward.Length);
        return reward[rnd];
    }

    cardInfo RatesUpReward(Rarity rarity)
    {
        int rnd;
        cardInfo[] reward = Array.FindAll(_reward, r=>r.rarity == rarity);
        //untuk cari semua kartu yang di rate up dengan rarity yang didapatkan
        cardInfo[] RateUpReward = Array.FindAll(rateUpReward, r=>r.rarity == rarity);

        //kalo ada rateup rewardnya di random lagi biar tau dapet yang rate up atau ndak
        if(RateUpReward.Length > 0)
        {
            rnd = UnityEngine.Random.Range(1,11);
            if(rnd <= rateUpRate)
            {
                Debug.Log("Rate up reward");
                rnd = UnityEngine.Random.Range(0,RateUpReward.Length);
                return RateUpReward[rnd];
            }
        }
        rnd =  UnityEngine.Random.Range(0,reward.Length);
        return reward[rnd];
    }
}

[CustomEditor(typeof(GachaManager))]
public class GachaEditor : Editor
{
    public int Common, Uncommon, Rare, Epic, Legend;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        GachaManager gm = (GachaManager)target;

        Common = EditorGUILayout.IntField("Common", (gm.Rates(Rarity.Common) - gm.Rates(Rarity.Uncommon)));
        Uncommon = EditorGUILayout.IntField("Uncommon", (gm.Rates(Rarity.Uncommon) - gm.Rates(Rarity.Rare)));
        Rare = EditorGUILayout.IntField("Rare", (gm.Rates(Rarity.Rare) - gm.Rates(Rarity.Epic)));
        Epic = EditorGUILayout.IntField("Epic", (gm.Rates(Rarity.Epic) - gm.Rates(Rarity.Legend)));
        Legend = EditorGUILayout.IntField("Legend", gm.Rates(Rarity.Legend));
    }
}