using L2IO;
using MonoMod;
using UnityEngine;
using LM2KeyMod.Patches;
using L2Flag;

#pragma warning disable 0649, 0414, 0108, 0626
namespace LM2KeyMod.Patches
{
    [MonoModPatch("L2IO.L2SaveAndLoad")]
    public class patched_L2SaveAndLoad : L2IO.L2SaveAndLoad
    {
        [MonoModIgnore]
        public patched_L2SaveAndLoad(int bufferSize, L2FlagSystem l2fs, patched_L2System l2sys) : base(bufferSize, l2fs, l2sys) { }

        [MonoModIgnore]
        private byte[] bookbit_data;

        public void clearSystemFile1()
        {
            int sys1_length = this.bookbit_data.Length;
            this.bookbit_data = new byte[sys1_length];
            for (int flag_no = 0; flag_no < sys1_length; flag_no++)
            {
                this.bookbit_data[flag_no] = (byte)0;
            }
            this.saveSystemFile1();
        }
    }
}
