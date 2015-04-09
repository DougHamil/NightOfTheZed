using UnityEngine;
using System.Collections;

public class LevelHandler : MonoBehaviour {

	private int currentXp = 0;
	private int level = 0;

	public void AddXp(int xp)
	{
		int requiredXp = this.getXpForNextLevel();
		this.currentXp += xp;
		if(currentXp >= requiredXp)
		{
			level++;
			this.gameObject.SendMessage("OnLevelUp", level, SendMessageOptions.DontRequireReceiver);
		}
	}

	public int getCurrentLevel()
	{
		return level;
	}

	public int getCurrentXp()
	{
		return this.currentXp;
	}

	public int getXpForNextLevel()
	{
		return 500 + (int)(Mathf.Pow(2f, (float)this.level) * 500f);
	}
}
