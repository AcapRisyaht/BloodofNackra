using UnityEngine;

/// <summary>
/// PlayerStatus — Simpan status asas player.
/// Guard: Termasuk HP, stamina, stun, slow, dan duit.
/// </summary>
public class PlayerStatus : MonoBehaviour
{
    // Guard: Status asas
    public int hp = 100;
    public int stamina = 50;

    // Guard: Duit player
    public int duitPlayer = 100; // Duit permulaan

    // Guard: Status kawalan
    public bool isStunned = false;
    public bool isSlowed = false;

    private float slowFactor = 1f;

    // ============================
    // Kawalan Slow
    // ============================
    public void ApplySlow(float factor, float duration)
    {
        if (!isSlowed)
        {
            isSlowed = true;
            slowFactor = factor;
            Invoke(nameof(RemoveSlow), duration);
        }
    }

    public void RemoveSlow()
    {
        isSlowed = false;
        slowFactor = 1f;
    }

    // ============================
    // Kawalan Stun
    // ============================
    public void ApplyStun(float duration)
    {
        if (!isStunned)
        {
            isStunned = true;
            Invoke(nameof(RemoveStun), duration);
        }
    }

    public void RemoveStun()
    {
        isStunned = false;
    }

    // ============================
    // Kawalan Duit
    // ============================
    public void TambahDuit(int jumlah)
    {
        duitPlayer += jumlah;
        Debug.Log("Player dapat duit: " + jumlah);
    }

    public bool TolakDuit(int jumlah)
    {
        if (duitPlayer >= jumlah)
        {
            duitPlayer -= jumlah;
            Debug.Log("Player bayar: " + jumlah);
            return true;
        }
        else
        {
            Debug.Log("Duit tak cukup!");
            return false;
        }
    }

    // ============================
    // Getter untuk faktor kelajuan
    // ============================
    public float GetSpeedFactor()
    {
        return slowFactor;
    }
}