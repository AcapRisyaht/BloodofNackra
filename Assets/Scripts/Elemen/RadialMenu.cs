using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    public Image[] slotIcons; // drag 7 ikon dari Inspector
    public int slotAktif = 0;
    public GameObject radialMenuUI;
    public bool isOpen => _isOpen;

    private PlayerControls playerControls;
    private GameProgress gp;
    private bool _isOpen = false;

    void Awake()
    {
        playerControls = new PlayerControls();
        gp = FindObjectOfType<GameProgress>();
    }

    public void UpdateGeligaSlots()
    {
        if (gp == null || slotIcons == null || slotIcons.Length == 0) return;

        // jumlah slot aktif = bilangan bos yang dah kalah
        int unlockedSlots = gp.bossDefeated;

        for (int i = 0; i < slotIcons.Length; i++)
        {
            bool unlocked = i < unlockedSlots;
            slotIcons[i].enabled = unlocked;
            slotIcons[i].color = unlocked ? Color.white : Color.gray;
        }

        if (unlockedSlots > 0)
        {
            SetActiveSlot(0);
        }
        else
        {
            slotAktif = 0;
        }
    }

    void OnEnable()
    {
        playerControls.Enable();
        playerControls.Combat.Enable();
        playerControls.Combat.ScrollWheel.performed += OnScrollWheel;
        playerControls.Combat.OpenWheel.performed += ctx => ToggleRadialMenu();
    }

    void OnDisable()
    {
        playerControls.Combat.ScrollWheel.performed -= OnScrollWheel;
        playerControls.Disable();
    }

    private void OnScrollWheel(InputAction.CallbackContext ctx)
    {

        float scroll = ctx.ReadValue<float>();
        Debug.Log("Scroll value (float): " + scroll);

        if (scroll > 0f)
            PreviousSlot();
        else if (scroll < 0f)
            NextSlot();
    }

    public void NextSlot()
    {
        int unlockedSlots = GetUnlockedSlotsSafe();
        if (unlockedSlots <= 0) return;

        slotAktif++;

        if (slotAktif >= unlockedSlots) 
            slotAktif = 0;
            
        SetActiveSlot(slotAktif);
    }

    public void PreviousSlot()
    {
        int unlockedSlots = GetUnlockedSlotsSafe();
        if (unlockedSlots <= 0) return;

        slotAktif--;

        if (slotAktif < 0) 
            slotAktif = unlockedSlots - 1;

        SetActiveSlot(slotAktif);
    }

    public void SetActiveSlot(int index)
    {
        // 1) Guard: elak index luar julat array ikon
        if (index < 0 || index >= slotIcons.Length)
        {
            Debug.Log($"SetActiveSlot: index {index} di luar julat (0..{slotIcons.Length - 1}).");
            return;
        }
        
        // 2) Guard: elak pilih slot yang belum unlock
        if (!slotIcons[index].enabled)
        {
            Debug.Log($"SetActiveSlot: slot {index} belum unlock, abaikan.");
            return;
        }

        // 3) Set slot aktif
        slotAktif = index;
        Debug.Log("Slot aktif: " + slotIcons[index].name);

        // 4) Warna (UI feedback) - konsisten dan jelas
        for (int i = 0; i < slotIcons.Length; i++)
        {
            bool isUnlocked = slotIcons[i].enabled;
            slotIcons[i].color = (i == slotAktif) ? Color.red : Color.white;
        }
    }

    public void ToggleRadialMenu()
    {
        _isOpen =  !_isOpen;
        radialMenuUI.SetActive(isOpen);

        Cursor.lockState = _isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _isOpen;
        
        if (_isOpen)
        {
            UpdateGeligaSlots();
        }
    }

    private int GetUnlockedSlotsSafe()
    {
        // Pastikan nilai sentisa valid dan tidak lebih panjang daripada slotIcons
        int unlockedSlots = (gp != null) ? gp.bossDefeated : 0;
        return Mathf.Clamp(unlockedSlots, 0, slotIcons.Length);
    }

    public bool IsSlotUnlocked(int index)
    {
        if (index < 0 || index >= slotIcons.Length) return false;
        return index >= 0 && index < unlockedSlots;
    }

    public int unlockedSlots => GetUnlockedSlotsSafe();
}