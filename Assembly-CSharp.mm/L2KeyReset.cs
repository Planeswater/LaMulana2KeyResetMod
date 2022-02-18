using UnityEngine;
using UnityEngine.SceneManagement;
using LM2KeyMod.Patches;


namespace LM2KeyMod
{
    public class L2KeyReset : MonoBehaviour
    {
        private Font font = null;
        private GUIStyle style = null;
        private bool onTitle;

        private patched_L2System sys;
        private string system1data_name;

        public void init()
        {
            this.sys = GameObject.Find("GameSystem").GetComponent<patched_L2System>(); ;
            this.system1data_name = HeadMark.system1data_name;
        }

        //Called when selecting "New game" from the title screen
        public void reset()
        {
            this.sys.clearClothAndKeys();
            this.sys.writeSystem2Data(27, (byte)0); //Undoing "Previously beat the game"
            this.sys.emptyGlossary();
        }


        public void OnGUI()
        {
            if (font == null)
                font = Font.CreateDynamicFontFromOSFont("Consolas", 14);

            if (style == null)
            {
                style = new GUIStyle(GUI.skin.label);
                style.normal.textColor = Color.white;
                style.font = font;
                style.fontStyle = FontStyle.Bold;
            }

            if (onTitle)
            {
                GUIContent content = new GUIContent("La-Mulana 2 Garb Key Reset Mod v" + Version.version);
                style.fontSize = 14;
                Vector2 size = style.CalcSize(content);
                GUI.Label(new Rect(0, 0, size.x, size.y), content, style);
                style.fontSize = 10;
                GUI.Label(new Rect(0, size.y, 500, 50), "Starting a new game with this mod will clear your glossary and take away all garbs and keys. Backing up your LM2 system files is recommended.", style);
            }
        }

        public void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            onTitle = scene.name.Equals("title");
        }
    }
}