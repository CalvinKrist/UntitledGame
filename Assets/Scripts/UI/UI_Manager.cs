using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResourceTypes;

public class UI_Manager : MonoBehaviour
{
	
	public float UIRefreshRate = 0.05f;
	
	[Header("Resource Palel")]
	public string powerLabelName = "PowerLabel";
	public string moneyLabelName = "MoneyLabel";
	public ResourceStorage storage;
	
	private Text powerLabel;
	private Text moneyLabel;

	
    // Start is called before the first frame update
    void Start()
    {
		powerLabel = GameObject.Find(powerLabelName).GetComponent<Text>();
		moneyLabel = GameObject.Find(moneyLabelName).GetComponent<Text>();
		
        StartCoroutine("UpdateUI");
    }

    
    IEnumerator UpdateUI()
    {
		for(;;) 
		{
			powerLabel.text = ((int)storage.GetResourceCount(ResourceType.Power)).ToString();
			moneyLabel.text = ((int)storage.GetResourceCount(ResourceType.Money)).ToString();
			
			yield return new WaitForSeconds(UIRefreshRate);
		}
    }
}
