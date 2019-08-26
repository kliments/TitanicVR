using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddXaxisText : MonoBehaviour {

    private string[] ageLegend = { "0", "10", "20", "30", "40", "50", "60", "70", "80" };
    private string[] pClassLegend = { "1", "2", "3" };
    private string[] sibSpLegend = { "0", "1", "2", "3", "4", "5" };
    private string[] parChLegend = { "0", "1", "2", "3", "4", "5", "6" };

    private TextMesh xAxis;

    public TextMesh age1;
    public TextMesh age2;
    public TextMesh age3;
    public TextMesh age4;
    public TextMesh age5;
    public TextMesh age6;
    public TextMesh age7;
    public TextMesh age8;
    public TextMesh age9;

    public TextMesh pclass1;
    public TextMesh pclass2;
    public TextMesh pclass3;

    public TextMesh sibSp1;
    public TextMesh sibSp2;
    public TextMesh sibSp3;
    public TextMesh sibSp4;
    public TextMesh sibSp5;
    public TextMesh sibSp6;

    public TextMesh parCh1;
    public TextMesh parCh2;
    public TextMesh parCh3;
    public TextMesh parCh4;
    public TextMesh parCh5;
    public TextMesh parCh6;
    public TextMesh parCh7;

    GameObject agePrefab;
    GameObject pClassPrefab;
    GameObject sibSpPrefab;
    GameObject parChPrefab;

    // Use this for initialization
    void Start()
    {
        if(transform.parent.parent.name == "PlottingData")
        {
            xAxis = GameObject.Find("PlottingData/Axis/xAxis/xAxisMenu/MenuItem/MenuText").GetComponent<TextMesh>();
        }
        else if(transform.parent.parent.name == "PlottingData(Clone)")
        {
            xAxis = GameObject.Find("PlottingData(Clone)/Axis/xAxis/xAxisMenu/MenuItem/MenuText").GetComponent<TextMesh>();
        }
    }

    // Update is called once per frame
    void Update()
    {/*
        agePrefab = GameObject.Find("xAxis/agePrefab");
        pClassPrefab = GameObject.Find("xAxis/pClassPrefab");
        sibSpPrefab = GameObject.Find("xAxis/sibSpPrefab");
        parChPrefab = GameObject.Find("xAxis/parChPrefab");
        
        agePrefab.transform.rotation = Quaternion.Euler(0,0,0);
        pClassPrefab.transform.rotation = Quaternion.Euler(0, 0, 0);
        sibSpPrefab.transform.rotation = Quaternion.Euler(0, 0, 0);
        parChPrefab.transform.rotation = Quaternion.Euler(0, 0, 0);
        
    */


        age1.text = "";
        age2.text = "";
        age3.text = "";
        age4.text = "";
        age5.text = "";
        age6.text = "";
        age7.text = "";
        age8.text = "";
        age9.text = "";

        pclass1.text = "";
        pclass2.text = "";
        pclass3.text = "";

        sibSp1.text = "";
        sibSp2.text = "";
        sibSp3.text = "";
        sibSp4.text = "";
        sibSp5.text = "";
        sibSp6.text = "";

        parCh1.text = "";
        parCh2.text = "";
        parCh3.text = "";
        parCh4.text = "";
        parCh5.text = "";
        parCh6.text = "";
        parCh7.text = "";



        if (xAxis.text == "Age_Of_Passenger")
        {
           // xAxis.text = "Age";

            for (int i = 0; i < ageLegend.Length; i++)
            {

                age1.transform.localPosition = new Vector3(0.05f, 0.8f, 0f);
                age1.text = ageLegend[0];

                age2.transform.localPosition = new Vector3(0.4f, 0.8f, 0f);
                age2.text = ageLegend[1];

                age3.transform.localPosition = new Vector3(0.75f,0.8f , 0f);
                age3.text = ageLegend[2];

                age4.transform.localPosition = new Vector3(1.1f, 0.8f, 0f);
                age4.text = ageLegend[3];

                age5.transform.localPosition = new Vector3(1.45f, 0.8f, 0f);
                age5.text = ageLegend[4];

                age6.transform.localPosition = new Vector3(1.8f, 0.8f, 0f);
                age6.text = ageLegend[5];

                age7.transform.localPosition = new Vector3(2.15f, 0.8f, 0f);
                age7.text = ageLegend[6];

                age8.transform.localPosition = new Vector3(2.5f, 0.8f, 0f);
                age8.text = ageLegend[7];

                age9.transform.localPosition = new Vector3(2.85f, 0.8f, 0f);
                age9.text = ageLegend[8];
            }
        }else if (xAxis.text == "Passenger_Class")
        {
           // xAxis.text = "Passenger Class";

            for (int i = 0; i < pClassLegend.Length; i++)
            {

                pclass1.transform.localPosition = new Vector3(0.9f, 0.8f, 0f);
                pclass1.text = pClassLegend[0];

                pclass2.transform.localPosition = new Vector3(1.8f, 0.8f, 0f);
                pclass2.text = pClassLegend[1];

                pclass3.transform.localPosition = new Vector3(2.7f, 0.8f, 0f);
                pclass3.text = pClassLegend[2];

            }
        }else if (xAxis.text == "Siblings_Spouses")
        {
          //  xAxis.text = "Siblings/Spouses";
            for (int i = 0; i < sibSpLegend.Length; i++)
            {

                sibSp1.transform.localPosition = new Vector3(0.05f, 0.8f, 0);
                sibSp1.text = sibSpLegend[0];

                sibSp2.transform.localPosition = new Vector3(0.61f, 0.8f, 0f);
                sibSp2.text = sibSpLegend[1];

                sibSp3.transform.localPosition = new Vector3(1.17f, 0.8f, 0f);
                sibSp3.text = sibSpLegend[2];

                sibSp4.transform.localPosition = new Vector3(1.73f, 0.8f, 0f);
                sibSp4.text = sibSpLegend[3];

                sibSp5.transform.localPosition = new Vector3(2.29f, 0.8f, 0f);
                sibSp5.text = sibSpLegend[4];

                sibSp6.transform.localPosition = new Vector3(2.85f, 0.8f, 0f);
                sibSp6.text = sibSpLegend[5];

            }
        }else if (xAxis.text == "Parents_Children")
        {
           // xAxis.text = "Parents/Children";
            for (int i = 0; i < parChLegend.Length; i++)
            {

                parCh1.transform.localPosition = new Vector3(0.05f, 0.8f, 0);
                parCh1.text = parChLegend[0];

                parCh2.transform.localPosition = new Vector3(0.51f, 0.8f, 0f);
                parCh2.text = parChLegend[1];

                parCh3.transform.localPosition = new Vector3(1.07f, 0.8f, 0f);
                parCh3.text = parChLegend[2];

                parCh4.transform.localPosition = new Vector3(1.53f, 0.8f, 0f);
                parCh4.text = parChLegend[3];

                parCh5.transform.localPosition = new Vector3(1.99f, 0.8f, 0f);
                parCh5.text = parChLegend[4];

                parCh6.transform.localPosition = new Vector3(2.45f, 0.8f, 0f);
                parCh6.text = parChLegend[5];

                parCh7.transform.localPosition = new Vector3(3f, 0.8f, 0f);
                parCh7.text = parChLegend[6];


            }
        }
        


    }
}
