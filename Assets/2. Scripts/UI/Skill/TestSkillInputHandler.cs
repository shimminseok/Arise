using UnityEngine;

public class TestSkillInputHandler : MonoBehaviour
{
    [SerializeField] private SkillCooldownIndicator skill1UI; // Alpha Skill
    [SerializeField] private SkillCooldownIndicator skill2UI; // Beta Skill

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            skill1UI.StartCooldown();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            skill2UI.StartCooldown();
        }
    }
}