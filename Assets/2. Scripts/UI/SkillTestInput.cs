using UnityEngine;

public class SkillTestInput : MonoBehaviour
{
    [SerializeField] private SkillCooldownUI skill1UI; // Alpha Skill
    [SerializeField] private SkillCooldownUI skill2UI; // Beta Skill

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