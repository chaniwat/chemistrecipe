using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chemistrecipe.localization
{
    public class ThLanguage : LocalLanguage
    {

        public ThLanguage() : base("th")
        {
            setMainMenu();
            setPlay();
        }

        private void setMainMenu()
        {
            setString("mainmenu.main.start", "เริ่ม");
            setString("mainmenu.main.connect", "เชื่อมต่อ");
            setString("mainmenu.main.setting", "ตั้งค่า");
            setString("mainmenu.main.exit", "ออก");

            setString("mainmenu.course.textcoursedescription", "เลือก Course จากเมนูด้านซ้าย");
            setString("mainmenu.course.tutorialbtn", "ดูวิธีเล่น");
            setString("mainmenu.course.getmarkerbtn", "ดาวน์โหลดมาร์คเกอร์");
            setString("mainmenu.course.playbtn", "เริ่มการทดลอง");
            setString("mainmenu.course.backbtn", "ย้อนกลับ");

            setString("mainmenu.setting.profile", "โปรไฟล์");
            setString("mainmenu.setting.profile.nametext", "ชื่อ");
            setString("mainmenu.setting.profile.alias", "ฉายา");
            setString("mainmenu.setting.profile.avatar", "รูป");
            setString("mainmenu.setting.sound", "ตั้งค่าเสียง");
            setString("mainmenu.setting.sound.volume", "ความดัง");
            setString("mainmenu.setting.language", "ภาษา");
            setString("mainmenu.setting.game", "ตั้งค่าโปรแกรม");
            setString("mainmenu.setting.game.player", "ไอดีผู้เล่น");
            setString("mainmenu.setting.confirm", "บันทึก");
        }

        private void setPlay()
        {
            setString("course.stir", "คนภาชนะ");
            setString("course.fail", "การทดลองล้มเหลว");
            setString("course.finishbtn", "จบการทดลอง");
            setString("course.restartbtn", "เริ่มใหม่");
            setString("course.resumebtn", "ย้อนกลับ");
            setString("course.instructionbtn", "วิธี/ขั้นตอน");
            setString("course.settingbtn", "ตั้งค่า");
            setString("course.mainmenubtn", "กลับหน้าหลัก");
            setString("course.closetutorialbtn", "ปิด");
        }

    }
}
