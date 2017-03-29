using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chemistrecipe.localization
{
    public class EnLanguage : LocalLanguage
    {

        public EnLanguage() : base("en")
        {
            setMainMenu();
            setPlay();
        }

        private void setMainMenu()
        {
            setString("mainmenu.main.start", "Start");
            setString("mainmenu.main.connect", "Connect");
            setString("mainmenu.main.setting", "Setting");
            setString("mainmenu.main.exit", "exit");

            setString("mainmenu.course.textcoursedescription", "Select course name on the left");
            setString("mainmenu.course.tutorialbtn", "Tutorial");
            setString("mainmenu.course.getmarkerbtn", "Get Marker");
            setString("mainmenu.course.playbtn", "Play");
            setString("mainmenu.course.backbtn", "Back");

            setString("mainmenu.setting.profile", "Profile");
            setString("mainmenu.setting.profile.nametext", "Name");
            setString("mainmenu.setting.profile.alias", "Alias");
            setString("mainmenu.setting.profile.avatar", "Avatar");
            setString("mainmenu.setting.sound", "Sound setting");
            setString("mainmenu.setting.sound.volume", "Volume");
            setString("mainmenu.setting.language", "Language");
            setString("mainmenu.setting.game", "App setting");
            setString("mainmenu.setting.game.player", "Player UID");
            setString("mainmenu.setting.confirm", "Confirm");
        }

        private void setPlay()
        {
            setString("course.stir", "Stir");
            setString("course.fail", "Course fail");
            setString("course.finishbtn", "Finish");
            setString("course.restartbtn", "Restart");
            setString("course.resumebtn", "Resume");
            setString("course.instructionbtn", "Instruction");
            setString("course.settingbtn", "Setting");
            setString("course.mainmenubtn", "Main Menu");
            setString("course.closetutorialbtn", "Close");

            setString("course.equipment.detail.volume", "Volume");
            setString("course.equipment.detail.temperature", "Temp");
        }

    }
}
