using MonoMod;
using UnityEngine;

#pragma warning disable 0649, 0414, 0108, 0626
namespace LM2KeyMod.Patches
{
    [MonoModPatch("L2Base.L2System")]
    public class patched_L2System : L2Base.L2System
    {
        [MonoModIgnore]
        private patched_L2SaveAndLoad l2sal;

        private void orig_Start() { }
        private void Start()
        {
            orig_Start();
            GameObject obj = new GameObject();
            L2KeyReset component = obj.AddComponent<L2KeyReset>() as L2KeyReset;
            DontDestroyOnLoad(obj);
        }

        public void emptyGlossary()
        {
            this.l2sal.clearSystemFile1();
        }
    }
}
