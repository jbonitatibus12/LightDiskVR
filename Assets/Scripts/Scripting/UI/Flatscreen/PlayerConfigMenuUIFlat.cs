using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Button = UnityEngine.UI.Button;
using Text = UnityEngine.UI.Text;

public class PlayerConfigMenuUIFlat : MonoBehaviour
{
    public Button ReadyButton;
    public Text ReadyText;
    [SerializeField] Button WeaponLeft1, WeaponLeft2, WeaponRight1, WeaponRight2;
    [SerializeField] Text m_Weapon1, m_Weapon2;

    public GameObject[] AvailableWeapons;

    public static PlayerConfigMenuUIFlat Instance;

    public static Text Weapon1 => Instance.m_Weapon1;
    public static Text Weapon2 => Instance.m_Weapon1;

    BaseAccessor owner;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BaseAccessor accessor = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<BaseAccessor>();
        print(accessor);

        if (NetworkManager.Singleton.IsHost)
        {
            ReadyText.text = "Start Match";
        } else
        {
            ReadyText.text = "I'm Ready";
        }

        ReadyButton.onClick.RemoveAllListeners();
        ReadyButton.onClick.AddListener(delegate
        {
            if (accessor.IsOwner && NetworkManager.Singleton.IsHost)
            {
                accessor.PlayerConfigExit();
            } else
            {
                accessor.SetLockServerRpc(!accessor.Lock.Value);
            }
            accessor.PlayerConfig.InitialWeapon1 = AvailableWeapons[accessor.PlayerConfig.WeaponIndex1];
            accessor.PlayerConfig.InitialWeapon2 = AvailableWeapons[accessor.PlayerConfig.WeaponIndex2];

            print(accessor.PlayerConfig.InitialWeapon1);
            print(accessor.PlayerConfig.InitialWeapon2);
        });

        WeaponLeft1.onClick.RemoveAllListeners();
        WeaponLeft1.onClick.AddListener(delegate
        {
            accessor.SetWeapon1IndexServerRpc(MathUtils.Mod(accessor.PlayerConfig.WeaponIndex1 - 1, AvailableWeapons.Length));
        });

        WeaponRight1.onClick.RemoveAllListeners();
        WeaponRight1.onClick.AddListener(delegate
        {
            accessor.SetWeapon1IndexServerRpc(MathUtils.Mod(accessor.PlayerConfig.WeaponIndex1 + 1, AvailableWeapons.Length));
        });

        WeaponLeft2.onClick.RemoveAllListeners();
        WeaponLeft2.onClick.AddListener(delegate
        {
            accessor.SetWeapon2IndexServerRpc(MathUtils.Mod(accessor.PlayerConfig.WeaponIndex2 - 1, AvailableWeapons.Length));
        });

        WeaponRight2.onClick.RemoveAllListeners();
        WeaponRight2.onClick.AddListener(delegate
        {
            accessor.SetWeapon2IndexServerRpc(MathUtils.Mod(accessor.PlayerConfig.WeaponIndex2 + 1, AvailableWeapons.Length));
        });

        owner = accessor;
    }

    private void Update()
    {
        m_Weapon1.text = AvailableWeapons[owner.PlayerConfig.WeaponIndex1].GetComponent<Weapon>().weaponName;
        m_Weapon2.text = AvailableWeapons[owner.PlayerConfig.WeaponIndex2].GetComponent<Weapon>().weaponName;
    }
}
