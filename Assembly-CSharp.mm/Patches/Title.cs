using L2Base;
using L2Menu;
using System;
using TMPro;
using UnityEngine;
using MonoMod;

#pragma warning disable 0649, 0414, 0108, 0626
namespace LM2KeyMod.Patches
{
    [MonoModPatch("global::Title")]
    class patched_Title : Title
    {
		[MonoModIgnore]
		private bool move_opningdemo;
		
		[MonoModIgnore]
		private int fadecounter;
		
		[MonoModIgnore]
		private int wait_counter;
		
		[MonoModIgnore]
		private long title_time;

		[MonoModIgnore]
		private float canimespeed = 4.25f;

		[MonoModIgnore]
		private patched_Title.SELECT swi;

		[MonoModIgnore]
		private TextMeshProUGUI tx_start;

		[MonoModIgnore]
		private TextMeshProUGUI tx_cont;

		[MonoModIgnore]
		private TextMeshProUGUI tx_load;

		[MonoModIgnore]
		private TextMeshProUGUI tx_option;

		[MonoModIgnore]
		private TextMeshProUGUI tx_book;

		[MonoModIgnore]
		private TextMeshProUGUI tx_exit;

		[MonoModIgnore]
		private TextMeshProUGUI tx_system;

		[MonoModIgnore]
		private float color_counter;

		[MonoModIgnore]
		private bool color_ud_flag;

		[MonoModIgnore]
		private Animator titleanime;

		[MonoModIgnore]
		private bool start_flag;

		[MonoModIgnore]
		private int stoploadtimer;

		private L2KeyReset key_resetter;

		private void orig_Start() { }
		private void Start()
        {
			orig_Start();
			this.key_resetter = GameObject.FindObjectOfType<L2KeyReset>();
			key_resetter.init();
        }


		[MonoModReplace]
        public override bool Farst()
        {
			{
				if (this.move_opningdemo)
				{
					++this.fadecounter;
					if (this.fadecounter == 121)
						this.sys.getL2SystemCore().loadDemoSceane("Opening");
					return true;
				}
				if (this.wait_counter > -1)
				{
					++this.wait_counter;
					if ((DateTime.Now.Ticks - this.title_time) / 10000000L > 60L)
					{
						this.sys.getL2SystemCore().gameScreenFadeOut(120);
						this.fadecounter = 0;
						this.move_opningdemo = true;
						this.sys.getL2SystemCore().musicManager.masterMusicVolumeFade(0.0f, 100);
					}
				}
				if (this.start_flag)
				{
					if ((double)this.titleanime.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0)
					{
						if (this.sys.isAnyKey(KEYSTATE.DOWN))
						{
							this.titleanime.Play("TitleAnimation");
							this.choiceFontDraw(true);
							this.start_flag = false;
						}
						return true;
					}
					this.start_flag = false;
					this.choiceFontDraw(true);
				}
				switch (this.swi)
				{
					case patched_Title.SELECT.start:
						this.tx_start.color = new Color(1f, this.color_counter / (float)byte.MaxValue, this.color_counter / (float)byte.MaxValue);
						if (this.sys.getL2Keys(L2KEYS.ok, KEYSTATE.DOWN))
						{
							this.key_resetter.reset();
							this.sys.getL2SystemCore().seManager.playSE((GameObject)null, 0);
							this.swi = patched_Title.SELECT.game_start;
							this.sys.getL2SystemCore().gameScreenFadeOut(60);
							this.sys.getL2SystemCore().musicManager.setBaseBGMVolume(0.0f, 60);
							this.fadecounter = 0;
							this.sys.setSysFlag(SYSTEMFLAG.MENUOPEN);
							this.sys.delSysFlag(SYSTEMFLAG.MASKFULLON);
							this.sys.clearPlayTimes();
							this.wait_counter = -1;
							this.sys.gameStat();
							return true;
						}
						break;
					case patched_Title.SELECT.cont:
						if (this.sys.isMemSave())
						{
							this.tx_cont.color = new Color(1f, this.color_counter / (float)byte.MaxValue, this.color_counter / (float)byte.MaxValue);
							if (this.sys.getL2Keys(L2KEYS.ok, KEYSTATE.DOWN))
							{
								this.sys.getL2SystemCore().seManager.playSE((GameObject)null, 0);
								this.sys.memLoad();
								this.sys.setSysFlag(SYSTEMFLAG.MENUOPEN);
								this.sys.delSysFlag(SYSTEMFLAG.MASKFULLON);
								this.swi = patched_Title.SELECT.wait_load;
								this.sys.getL2SystemCore().gameScreenFadeOut(10);
								this.fadecounter = 0;
								this.sys.setContinuePlayTIme();
								this.wait_counter = -1;
								this.sys.runLoad();
								return true;
							}
							break;
						}
						this.tx_cont.color = new Color(0.5f, (float)((double)this.color_counter / (double)byte.MaxValue / 2.0), (float)((double)this.color_counter / (double)byte.MaxValue / 2.0));
						break;
					case patched_Title.SELECT.load:
						this.tx_load.color = new Color(1f, this.color_counter / (float)byte.MaxValue, this.color_counter / (float)byte.MaxValue);
						if (this.sys.getL2Keys(L2KEYS.ok, KEYSTATE.DOWN))
						{
							this.sys.openSaveAndLoadCanvas(SAVELOAD.load, new closeMenuCB(this.loadcallback), "title");
							this.swi = patched_Title.SELECT.wait;
							this.wait_counter = -1;
							this.title_time = DateTime.Now.Ticks;
							this.sys.setSysFlag(SYSTEMFLAG.MENUOPEN);
							this.sys.delSysFlag(SYSTEMFLAG.MASKFULLON);
							return true;
						}
						break;
					case patched_Title.SELECT.option:
						this.tx_option.color = new Color(1f, this.color_counter / (float)byte.MaxValue, this.color_counter / (float)byte.MaxValue);
						if (this.sys.getL2Keys(L2KEYS.ok, KEYSTATE.DOWN))
						{
							this.sys.getL2SystemCore().seManager.playSE((GameObject)null, 33);
							this.sys.setEndOfTitleMenusMet(new endTitleMet(this.optionCallBack));
							this.sys.openMenuNF(7);
							this.swi = patched_Title.SELECT.wait;
							this.wait_counter = -1;
							this.title_time = DateTime.Now.Ticks;
							return true;
						}
						break;
					case patched_Title.SELECT.exit:
						this.tx_exit.color = new Color(1f, this.color_counter / (float)byte.MaxValue, this.color_counter / (float)byte.MaxValue);
						if (this.sys.getL2Keys(L2KEYS.ok, KEYSTATE.DOWN))
						{
							Application.Quit();
							break;
						}
						break;
					case patched_Title.SELECT.wait_load:
						++this.fadecounter;
						if (this.fadecounter > 10)
						{
							this.sys.delSysFlag(SYSTEMFLAG.TITLENOW);
							this.sys.setSysFlag(SYSTEMFLAG.MENUOPEN);
							this.swi = patched_Title.SELECT.run_load;
							this.sys.runLoad();
							//this.sys.itemCompleteCheck();
							this.sys.softCompleteCheck();
							//this.sys.inportAllBookFlag();
							break;
						}
						break;
					case patched_Title.SELECT.run_load:
						if (this.sys.checkSysFlag(SYSTEMFLAG.MASKFULLON) != 0 && this.sys.getL2SystemCore().isAsyncLoadDone())
						{
							this.sys.delSysFlag(SYSTEMFLAG.MENUOPEN);
							this.sys.closeMenuNF();
							this.swi = patched_Title.SELECT.wait;
							this.sys.getL2SystemCore().setAsyncScene();
							break;
						}
						break;
					case patched_Title.SELECT.game_start:
						++this.fadecounter;
						if (this.fadecounter > 60)
						{
							this.sys.getL2SystemCore().setForceLoadAnimeMode(0);
							this.sys.getL2SystemCore().showLoadingAnime();
							this.sys.delSysFlag(SYSTEMFLAG.TITLENOW);
							this.swi = patched_Title.SELECT.run_start;
							break;
						}
						break;
					case patched_Title.SELECT.run_start:
						if (this.sys.checkSysFlag(SYSTEMFLAG.MASKFULLON) != 0 && this.sys.getL2SystemCore().isAsyncLoadDone())
						{
							this.sys.delSysFlag(SYSTEMFLAG.MENUOPEN);
							this.swi = patched_Title.SELECT.wait;
							this.sys.getL2SystemCore().setAsyncScene();
							break;
						}
						break;
					case patched_Title.SELECT.book:
						if (this.sys.isBooksData())
						{
							this.tx_book.color = new Color(1f, this.color_counter / (float)byte.MaxValue, this.color_counter / (float)byte.MaxValue);
							if (this.sys.getL2Keys(L2KEYS.ok, KEYSTATE.DOWN))
							{
								this.sys.getL2SystemCore().seManager.playSE((GameObject)null, 33);
								this.sys.exportBookFlags();
								this.sys.setEndOfTitleMenusMet(new endTitleMet(this.bookCallBack));
								this.sys.openMenuNF(5);
								this.swi = patched_Title.SELECT.wait;
								this.wait_counter = -1;
								this.title_time = DateTime.Now.Ticks;
								return true;
							}
							break;
						}
						this.tx_book.color = new Color(0.5f, (float)((double)this.color_counter / (double)byte.MaxValue / 2.0), (float)((double)this.color_counter / (double)byte.MaxValue / 2.0));
						break;
					case patched_Title.SELECT.system:
						this.tx_system.color = new Color(1f, this.color_counter / (float)byte.MaxValue, this.color_counter / (float)byte.MaxValue);
						if (this.sys.getL2Keys(L2KEYS.ok, KEYSTATE.DOWN))
						{
							this.sys.getL2SystemCore().seManager.playSE((GameObject)null, 33);
							this.sys.setEndOfTitleMenusMet(new endTitleMet(this.sysConfigCallBack));
							this.sys.openMenuNF(6);
							this.swi = patched_Title.SELECT.wait;
							this.wait_counter = -1;
							this.title_time = DateTime.Now.Ticks;
							return true;
						}
						break;
					case patched_Title.SELECT.load_stop:
						++this.stoploadtimer;
						if (this.stoploadtimer > 60 && (this.sys.getL2Keys(L2KEYS.ok, KEYSTATE.DOWN) || this.sys.getL2Keys(L2KEYS.cancel, KEYSTATE.DOWN)))
						{
							this.swi = patched_Title.SELECT.load;
							this.wait_counter = 0;
							this.title_time = DateTime.Now.Ticks;
							break;
						}
						break;
				}
				if (this.color_ud_flag)
				{
					this.color_counter += this.canimespeed;
					if ((double)this.color_counter >= (double)byte.MaxValue)
					{
						this.color_counter = (float)byte.MaxValue;
						this.color_ud_flag = false;
					}
				}
				else
				{
					this.color_counter -= this.canimespeed;
					if ((double)this.color_counter < 0.0)
					{
						this.color_counter = 0.0f;
						this.color_ud_flag = true;
					}
				}
				if (this.wait_counter > -1 && this.swi != patched_Title.SELECT.wait && this.swi != patched_Title.SELECT.wait_load && this.swi != patched_Title.SELECT.game_start)
					this.checkKey();
				this.drawClothKeys();
				return true;
			}
		}

		[MonoModIgnore]
		private void drawClothKeys() { }

		[MonoModIgnore]
		private void checkKey() { }

		[MonoModIgnore]
		private void choiceFontDraw(bool flag) { }

		[MonoModIgnore]
		private enum SELECT
		{
			start,
			cont,
			load,
			option,
			exit,
			wait,
			wait_load,
			run_load,
			game_start,
			run_start,
			book,
			system,
			load_stop,
		}
	}
}
