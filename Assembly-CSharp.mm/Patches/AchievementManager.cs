using MonoMod;
using System;
using Steamworks;
using UnityEngine;

#pragma warning disable 0626, 0649, 0414, 0108
namespace LM2KeyMod.Patches
{
    [MonoModPatch("global::AchivementManager")]
    public class patched_AchivementManager : AchivementManager
    {
        public patched_AchivementManager(global::L2SystemCore l2syscore) : base(l2syscore)
        {
        }

        [MonoModIgnore]
        private CGameID m_GameID;

        [MonoModIgnore]
        protected AchivementManager.Achivements[] achList;

        /*
         * Always return true ("Yeah, totally just set this achievement for the first time")
         */
        [MonoModReplace]
        public bool setAchivement(int achId)
        {
            bool pbAchieved = true;
            if (SteamManager.Initialized)
            {
                SteamUserStats.GetAchievement(this.achList[achId].ach_id, out pbAchieved);
            }
            if (!pbAchieved)
            {
                SteamUserStats.SetAchievement(this.achList[achId].ach_id);
                SteamUserStats.StoreStats();
            }
            this.achList[achId].achieved = true;
            return true;
        }

        /*
         * Initialize all achievements' status to not achieved
         */
        [MonoModReplace]
        public void makeAchivementList()
        {
            string[] names = Enum.GetNames(typeof(AchivementManager.ACH_ID));
            this.achNums = names.Length;
            this.achList = new AchivementManager.Achivements[this.achNums];
            for (int no = 0; no < this.achNums; ++no)
            {
                this.achList[no] = new AchivementManager.Achivements();
                this.achList[no].ach_id = names[no];
                this.achList[no].achieved = false;
            }
        }

        /*
         * Don't let Steam tell you what you have and haven't done.
         */
        [MonoModReplace]
        private void OnUserStatsReceived(UserStatsReceived_t pCallback)
        {
            if (!SteamManager.Initialized || (long)(ulong)this.m_GameID != (long)pCallback.m_nGameID)
                return;
            if (pCallback.m_eResult == EResult.k_EResultOK)
            {
                Debug.Log((object)"Received stats and achievements from Steam\n");
                if (this.staList != null)
                {
                    for (int index = 0; index < this.staNums; ++index)
                    {
                        if (!SteamUserStats.GetStat(this.staList[index].sta_id, out this.staList[index].sta_value))
                            this.staList[index].sta_value = -1f;
                    }
                }
                this.sysCore.checkAllAchevements();
            }
            else
                Debug.Log((object)("RequestStats - failed, " + (object)pCallback.m_eResult));
        }
    }
}