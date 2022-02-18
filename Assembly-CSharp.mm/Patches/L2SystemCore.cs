using UnityEngine;
using MonoMod;

#pragma warning disable 0649, 0414, 0108, 0626
namespace LM2KeyMod.Patches
{
    [MonoModPatch("global::L2SystemCore")]
    class patched_L2SystemCore : L2SystemCore
    {
        [MonoModIgnore]
        private byte checkAchivementData(AchivementManager.ACH_ID achId) { return 0; }

        /*
         * Also give the "Clear the game" key when the final boss is defeated
         */
        [MonoModReplace]
        private void getClothKey(AchivementManager.ACH_ID achId, bool floatMsg = false)
        {
            int key_no;
            switch (achId)
            {
                case AchivementManager.ACH_ID.ACH_C1_ITEMCOMP:
                    key_no = 0;
                    break;
                case AchivementManager.ACH_ID.ACH_C4_CATALOGUE:
                    key_no = 1;
                    break;
                case AchivementManager.ACH_ID.AHC_B10_LASTBOSS:
                case AchivementManager.ACH_ID.ACH_E16_CLEAR:
                    key_no = 2;
                    break;
                case AchivementManager.ACH_ID.ACH_J2_NEBLU2:
                    key_no = 3;
                    break;
                case AchivementManager.ACH_ID.ACH_DLC3_ENDING:
                    key_no = 4;
                    break;
                default:
                    return;
            }
            if (!this.sys.addClothesKey(key_no) || !floatMsg)
                return;
            this.sys.setFloatingDialog(3, key_no + 1);
            this.seManager.playSE((GameObject)null, 23);
        }

        /*
         * Do not clear garbs/keys during startup
         */
        [MonoModReplace]
        public void checkAllAchevements()
        {
            bool flag = false;
            int id = 0;
            AchivementManager.Achivements achivements = (AchivementManager.Achivements)null;
            for (; (achivements = this.achievementManager.getAchivementData(id)) != null; ++id)
            {
                switch (this.checkAchivementData((AchivementManager.ACH_ID)id))
                {
                    case 1:
                        flag = true;
                        break;
                }
            }
            if (this.achievementManager.getAchivementData(18).achieved)
                this.getClothKey(AchivementManager.ACH_ID.ACH_C1_ITEMCOMP);
            if (this.achievementManager.getAchivementData(21).achieved)
                this.getClothKey(AchivementManager.ACH_ID.ACH_C4_CATALOGUE);
            if (this.achievementManager.getAchivementData(37).achieved)
                this.getClothKey(AchivementManager.ACH_ID.ACH_E16_CLEAR);
            if (this.achievementManager.getAchivementData(43).achieved)
                this.getClothKey(AchivementManager.ACH_ID.ACH_J2_NEBLU2);
            if (this.achievementManager.getAchivementData(49).achieved)
                this.getClothKey(AchivementManager.ACH_ID.ACH_DLC3_ENDING);
            if (!flag)
                return;
            this.sys.saveZissekiFile();
        }
    }
}