using UnityEngine;

public class SkillInputHandler : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SkillManager.Instance.ExecuteSkill(0);
            Debug.Log("gd");
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            SkillManager.Instance.ExecuteSkill(1);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            SkillManager.Instance.ExecuteSkill(2);
        }
    }
}