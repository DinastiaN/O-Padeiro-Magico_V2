using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellSystem : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private float rechargeRate = 10f;
    [SerializeField] private float spellCooldown = 1f;
    [SerializeField] private float spellCost = 20f;
    [SerializeField] private Transform castPoint;
    [SerializeField] private GameObject spellObject;
    [SerializeField] private float spellSpeed = 10f;
    [SerializeField] private float spellDuration = 5f;
    [SerializeField] private float delay = 1f;

    private bool canCast = true;

    private void Start()
    {
        playerStats = PlayerStats.Instance;
    }

    private void Update()
    {
        RechargeMana();
    }

    private void RechargeMana()
    {
        playerStats.currentMana += rechargeRate * Time.deltaTime;
        playerStats.currentMana = Mathf.Clamp(playerStats.currentMana, 0f, playerStats.maxMana);
    }

    public void CastSpell()
    {
        if (canCast && playerStats.currentMana >= spellCost)
        {
            StartCoroutine(SpellCooldown());
            StartCoroutine(SpellEffect());

            playerStats.setMana(playerStats.currentMana - spellCost);
        }
    }

    private IEnumerator SpellCooldown()
    {
        canCast = false;
        yield return new WaitForSeconds(spellCooldown);
        canCast = true;
    }

    private IEnumerator SpellEffect()
    {
        Debug.Log("Casting spell from " + castPoint.position);

        yield return new WaitForSeconds(delay);

        GameObject spell = Instantiate(spellObject, castPoint.position, castPoint.rotation);

        Rigidbody rb = spell.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(spell.transform.forward * spellSpeed, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(spellDuration);

        Destroy(spell);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
