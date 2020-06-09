using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Untitled.Resource;

public class UI_Manager : MonoBehaviour
{
	
	public float UIRefreshRate = 0.05f;
	
	[Header("Resource Palel")]
	public string powerLabelName = "PowerLabel";
	public string moneyLabelName = "MoneyLabel";
	public string popLabelName = "PopLabel";
	public ResourceStorage storage;
	
	private Text powerLabel;
	private Text moneyLabel;
	private Text popLabel;

	
    // Start is called before the first frame update
    void Start()
    {
		powerLabel = GameObject.Find(powerLabelName).GetComponent<Text>();
		moneyLabel = GameObject.Find(moneyLabelName).GetComponent<Text>();
		popLabel = GameObject.Find(popLabelName).GetComponent<Text>();
		
        StartCoroutine("UpdateUI");
    }

    
    IEnumerator UpdateUI()
    {
		for(;;) 
		{
			powerLabel.text = ((int)storage.GetResourceCount(ResourceType.Power)).ToString();
			moneyLabel.text = ((int)storage.GetResourceCount(ResourceType.Money)).ToString();
			popLabel.text = ((int)storage.GetResourceCount(ResourceType.Population)).ToString();
			
			yield return new WaitForSeconds(UIRefreshRate);
		}
    }
}
