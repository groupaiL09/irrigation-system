using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class viewFarmList : MonoBehaviour
{
    public GameObject canvasRef;
    public GameObject prefabButton;
    public static int noOfFarms = 3;
    
    public void create(){
    	noOfFarms++;
    	GameObject goButton = (GameObject)Instantiate(prefabButton);
    	goButton.GetComponentInChildren<Text>().text = "Farm " + noOfFarms;
    	goButton.transform.SetParent(canvasRef.transform);
    	if(noOfFarms == 1){
            goButton.transform.localPosition = new Vector2(-47f, -55f);
    	}
    	if(noOfFarms == 2){
            goButton.transform.localPosition = new Vector2(334f, -55f);
    	}
    	if(noOfFarms == 3){
            goButton.transform.localPosition = new Vector2(-47f, -375f);
    	}
    	if(noOfFarms == 4){
            goButton.transform.localPosition = new Vector2(334f, -375f);
    	}
    	Debug.Log(goButton);
    }
}
