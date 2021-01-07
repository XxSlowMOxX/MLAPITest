// The SteamManager is designed to work with Steamworks.NET
// This file is released into the public domain.
// Where that dedication is not recognized you are granted a perpetual,
// irrevocable license to copy and modify this file as you see fit.
//
// Version: 1.0.9

#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if !DISABLESTEAMWORKS

using Steamworks;

#endif

[DisallowMultipleComponent]
public class SteamMan : MonoBehaviour
{
#if !DISABLESTEAMWORKS
    protected static bool s_EverInitialized = false;

    protected static SteamMan s_instance;

    protected static SteamMan Instance
    {
        get
        {
            if (s_instance == null)
            {
                return new GameObject("SteamManager").AddComponent<SteamMan>();
            }
            else
            {
                return s_instance;
            }
        }
    }

    protected bool m_bInitialized = false;

    public static bool Initialized
    {
        get
        {
            return Instance.m_bInitialized;
        }
    }

    private Callback<LobbyMatchList_t> m_LobbyMatchList;
    private Callback<LobbyCreated_t> m_LobbyCreated;

    private ulong myLobbyID = 0;

    protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

    protected static void SteamAPIDebugTextHook(int nSeverity, System.Text.StringBuilder pchDebugText)
    {
        Debug.LogWarning(pchDebugText);
    }

    protected virtual void Awake()
    {
        // Only one instance of SteamManager at a time!
        if (s_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        s_instance = this;

        if (s_EverInitialized)
        {
            throw new System.Exception("Tried to Initialize the SteamAPI twice in one session!");
        }

        // We want our SteamManager Instance to persist across scenes.
        DontDestroyOnLoad(gameObject);

        if (!Packsize.Test())
        {
            Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
        }

        if (!DllCheck.Test())
        {
            Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
        }

        try
        {
            if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
            {
                Application.Quit();
                return;
            }
        }
        catch (System.DllNotFoundException e)
        { // We catch this exception here, as it will be the first occurrence of it.
            Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e, this);

            Application.Quit();
            return;
        }

        m_bInitialized = SteamAPI.Init();
        HostLobby(10);
        if (!m_bInitialized)
        {
            Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);

            return;
        }

        s_EverInitialized = true;
    }

    public void HostLobby(int playerCount)
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 100);
    }

    public void StartGame()
    {
        if (myLobbyID != 0)
        {
            //SteamMatchmaking.SetLobbyGameServer(myLobbyID, )
        }
    }

    private void OnMatchListUpdate(LobbyMatchList_t pCallback)
    {
        List<string> keylist = new List<string>();
        for (int i = 0; i < pCallback.m_nLobbiesMatching - 1; i++)
        {
            CSteamID thisID = SteamMatchmaking.GetLobbyByIndex(i);
            for (int j = 0; j < SteamMatchmaking.GetLobbyDataCount(thisID); j++)
            {
                string key, value;
                if(SteamMatchmaking.GetLobbyDataByIndex(thisID, j, out key, 255, out value, 8192))
                {
                    keylist.Add(key);
                }
            }
        }
        keylist = keylist.Distinct().ToList();
        foreach(string unique in keylist)
        {
            print(unique);
        }
    }

    /*private string getIP() {
		return
    }*/

    private void OnLobbyCreated(LobbyCreated_t pCallback)
    {
        if (pCallback.m_eResult == EResult.k_EResultOK)
        {
            myLobbyID = pCallback.m_ulSteamIDLobby;
        }
    }

    #region hide2

    // This should only ever get called on first load and after an Assembly reload, You should never Disable the Steamworks Manager yourself.
    protected virtual void OnEnable()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }

        if (!m_bInitialized)
        {
            return;
        }
        m_LobbyMatchList = Callback<LobbyMatchList_t>.Create(OnMatchListUpdate);
        m_LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);

        if (m_SteamAPIWarningMessageHook == null)
        {
            // Set up our callback to receive warning messages from Steam.
            // You must launch with "-debug_steamapi" in the launch args to receive warnings.
            m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
            SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
        }
    }

    protected virtual void OnDestroy()
    {
        if (s_instance != this)
        {
            return;
        }

        s_instance = null;

        if (!m_bInitialized)
        {
            return;
        }

        SteamAPI.Shutdown();
    }

    protected virtual void Update()
    {
        if (!m_bInitialized)
        {
            return;
        }

        // Run Steam client callbacks
        SteamAPI.RunCallbacks();
    }

    #endregion hide2

#else
	public static bool Initialized {
		get {
			return false;
		}
	}
#endif // !DISABLESTEAMWORKS
}