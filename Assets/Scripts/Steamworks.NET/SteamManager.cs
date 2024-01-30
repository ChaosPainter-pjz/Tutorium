// SteamManager被设计为与Steamworks.NET一起工作。
// 本文件已发布到公共领域。
// 在不承认这种奉献的情况下，你将被授予一个永久的。
// 授予你永久的、不可撤销的许可，你可以按你认为合适的方式复制和修改这个文件。
//
// Version: 1.0.12

#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using Plugins.Steamworks.NET;
using Plugins.Steamworks.NET.autogen;
using Plugins.Steamworks.NET.types.SteamClient;
using Plugins.Steamworks.NET.types.SteamTypes;
using UnityEngine;

//
// The SteamManager provides a base implementation of Steamworks.NET on which you can build upon.
// It handles the basics of starting up and shutting down the SteamAPI for use.
//
namespace Steamworks.NET
{
    [DisallowMultipleComponent]
    public class SteamManager : MonoBehaviour
    {
#if !DISABLESTEAMWORKS
        protected static bool s_EverInitialized = false;

        protected static SteamManager s_instance;

        protected static SteamManager Instance
        {
            get
            {
                if (s_instance == null)
                    return new GameObject("SteamManager").AddComponent<SteamManager>();
                else
                    return s_instance;
            }
        }

        protected bool m_bInitialized = false;
        public static bool Initialized => Instance.m_bInitialized;

        protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

        [AOT.MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
        protected static void SteamAPIDebugTextHook(int nSeverity, System.Text.StringBuilder pchDebugText)
        {
            Debug.LogWarning(pchDebugText);
        }

#if UNITY_2019_3_OR_NEWER
        // 在禁用域重载的情况下，在进入播放模式前重置静态成员。
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitOnPlayMode()
        {
            s_EverInitialized = false;
            s_instance = null;
        }
#endif

        protected virtual void Awake()
        {
            // 一次只能有一个SteamManager的实例!
            if (s_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            s_instance = this;

            if (s_EverInitialized)
                // This is almost always an error.
                // The most common case where this happens is when SteamManager gets destroyed because of Application.Quit(),
                // and then some Steamworks code in some other OnDestroy gets called afterwards, creating a new SteamManager.
                // You should never call Steamworks functions in OnDestroy, always prefer OnDisable if possible.
                throw new System.Exception("Tried to Initialize the SteamAPI twice in one session!");

            // We want our SteamManager Instance to persist across scenes.
            DontDestroyOnLoad(gameObject);

            if (!Packsize.Test())
                Debug.LogError(
                    "[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.",
                    this);

            if (!DllCheck.Test())
                Debug.LogError(
                    "[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.",
                    this);

            try
            {
                // 如果Steam没有运行或者游戏没有通过Steam启动，SteamAPI_RestartAppIfNecessary就会启动Steam客户端。
                // 如果用户拥有这个游戏，也会再次启动这个游戏。这可以作为一种初级形式的DRM。

                // 一旦你得到了Valve分配的Steam AppID，你就需要用它来替换AppId_t.Invalid，并且
                // 例如："(AppId_t)480 "或 "new AppId_t(480)"。
                // See the Valve documentation for more information: https://partner.steamgames.com/doc/sdk/api#initialization_and_shutdown
                if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
                {
                    Application.Quit();
                    return;
                }
            }
            catch (System.DllNotFoundException e)
            {
                // We catch this exception here, as it will be the first occurrence of it.
                Debug.LogError(
                    "[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" +
                    e, this);

                Application.Quit();
                return;
            }

            // 初始化Steamworks的API。
            // 如果返回false，则表明存在以下情况之一。
            // [*] Steam客户端没有运行。一个运行中的Steam客户端需要提供各种Steamworks接口的实现。
            // [*] Steam客户端无法确定游戏的应用ID。如果你直接从可执行文件或调试器中运行你的应用程序，那么你必须在可执行文件旁边的游戏目录中设置一个[code-inline]steam_appid.txt[/code-inline]，其中包含你的应用程序ID，而不是其他。Steam会在当前工作目录中寻找这个文件。如果你在不同的目录下运行你的可执行文件，你可能需要重新定位[code-inline]steam_appid.txt[/code-inline]文件。
            // [*] 您的应用程序没有在与Steam客户端相同的操作系统用户环境下运行，例如不同的用户或管理访问级别。
            // [*] 确保你在当前活跃的Steam账户上拥有App ID的许可证。您的游戏必须显示在您的Steam库中。
            // [*] 您的应用程序ID没有完全设置好，即处于发布状态。不可用，或者它缺少默认包。
            // Valve的相关文档位于这里。
            // https://partner.steamgames.com/doc/sdk/api#initialization_and_shutdown
            m_bInitialized = SteamAPI.Init();
            if (!m_bInitialized)
            {
                Debug.LogError(
                    "[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.",
                    this);

                return;
            }

            s_EverInitialized = true;
        }

        // This should only ever get called on first load and after an Assembly reload, You should never Disable the Steamworks Manager yourself.
        protected virtual void OnEnable()
        {
            if (s_instance == null) s_instance = this;

            if (!m_bInitialized) return;

            if (m_SteamAPIWarningMessageHook == null)
            {
                // Set up our callback to receive warning messages from Steam.
                // You must launch with "-debug_steamapi" in the launch args to receive warnings.
                m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
                SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
            }
        }

        // OnApplicationQuit被过早调用，无法关闭SteamAPI。
        // 因为SteamManager应该是持久的，而且永远不会被禁用或销毁，我们可以在这里关闭SteamAPI。
        // 因此，我们不建议在其他OnDestroy函数中执行任何Steamworks工作，因为在Shutdown时，执行的顺序是不明确的。最好是OnDisable()。
        protected virtual void OnDestroy()
        {
            if (s_instance != this) return;

            s_instance = null;

            if (!m_bInitialized) return;

            SteamAPI.Shutdown();
        }

        protected virtual void Update()
        {
            if (!m_bInitialized) return;

            // Run Steam client callbacks
            SteamAPI.RunCallbacks();
        }
#else
	public static bool Initialized {
		get {
			return false;
		}
	}
#endif // !DISABLESTEAMWORKS
    }
}