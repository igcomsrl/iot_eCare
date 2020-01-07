//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Helpers;
using RazorEngine;
using RazorEngine.Templating;

namespace Meti.Infrastructure.Emails
{
    public class RazorCredentialTemplate
    {
        public string Result { get; set; }

        public RazorCredentialTemplate(string name, string surname, string username, string password, string appUrl)
        {
            #region Template

            string template = "<!DOCTYPE html>" +
                               "<html>" +
                               "<head>" +
                               "<meta http-equiv=\"Content-Type\" content = \"text/html; charset=utf-8\">" +
                               "<title> SAMPLE TITLE </title>" +
                               "</head> " +
                               "<body style = \"padding: 0px; margin: 0px;\">" +
                               "<div id=\"mailsub\" class=\"notification\" align=\"center\">" +
                               "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"min-width: 320px;\">" +
                               "<tr><td align = \"center\" bgcolor=\"#3b434c\">" +
                               "<table width = \"680\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
                               "<tr><td>" +
                               "<table border = \"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"table_width_100\" width=\"100%\" style=\"max-width: 680px; min-width: 300px;\">" +
                               "" +
                               "<tr><td align = \"center\" bgcolor=\"#3b434c\">" +
                               "<div style = \"height: 20px; line-height: 20px; font-size: 10px;\"> &nbsp;</div>" +
                               "<table width = \"96%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
                               "<tr><td align = \"left\" >" +
                               "<div class=\"mob_center_bl\" style=\"float: left; display: inline-block; width: 115px;\">" +
                               "<table class=\"mob_center\" width=\"115\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" style=\"border-collapse: collapse;\">" +
                               "<tr><td align = \"left\" valign=\"middle\">" +
                               "<div style = \"height: 20px; line-height: 20px; font-size: 10px;\">&nbsp;</div>" +
                               "<table width = \"115\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
                               "<tr><td align = \"left\" valign=\"top\" class=\"mob_center\">" +
                               "<a href = \"#\" target=\"_blank\" style=\"color: #596167; font-family: Arial, Helvetica, sans-serif; font-size: 13px;\">" +
                               "<font face = \"Arial, Helvetica, sans-seri; font-size: 13px;\" size=\"3\" color=\"#596167\">" +
                               "<img style=\" margin: -60px 0 -10px -200px;\" src=\"http://i.imgur.com/NGxKFC9.png\" > " +
                               // "<img src = \"http://artloglab.com/metromail2/images/logo.gif\" width=\"115\" height=\"19\" alt=\"Metronic\" border=\"0\" style=\"display: block;\"/>" +
                               "</font></a></td></tr></table></td></tr></table></div>" +
                               "</td><td align = \"right\" >" +
                               "<div class=\"mob_center_bl\" style=\"float: right; display: inline-block; width: 88px;\">" +
                               "<table width = \"88\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"right\" style=\"border-collapse: collapse;\">" +
                               "<tr><td align = \"right\" valign=\"middle\">" +
                               "<!-- padding -->" +
                               "<div style = \"height: 20px; line-height: 20px; font-size: 10px;\">&nbsp;</div>" +
                               "<table width = \"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
                               "<tr><td align =\"right\">" +
                               "<div class=\"mob_center_bl\" style=\"width: 88px;\">" +
                               "<table border = \"0\" cellspacing=\"0\" cellpadding=\"0\">" +
                               "<tr><td width = \"30\" align=\"center\" style=\"line-height: 19px;\">" +
                               //"<img style = \"zoom: 25%\" src=\"~/Content/themeforest/theme/templates/admin3_material_design/email_template2%20-%20Zeus/images/Logo%20IGcom%204%20col.jpg\">" +
                               "</td></tr></table></div><!--social END--></td></tr></table></td></tr></table></div><!-- Item END--></td></tr></table><!-- padding -->" +
                               "<div style = \"height: 30px; line-height: 30px; font-size: 10px;\" >&nbsp;</div></td></tr><!--header END--><!--content 1 -->" +
                               "<tr><td align = \"center\" bgcolor=\"#ffffff\">" +
                               "<table width = \"90%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
                               "<tr><td align = \"center\">" +
                               "<!--padding-->" +
                               "<div style=\"height: 100px; line-height: 100px; font-size: 10px;\">&nbsp;</div>" +
                               "<div style = \"line-height: 44px; \"><font face=\"Arial, Helvetica, sans-serif\" size=\"5\" color=\"#57697e\" style=\"font-size: 34px;\">" +
                               "<span style = \"font-family: Arial, Helvetica, sans-serif; font-size: 34px; color: #57697e;\">" +
                               "Registrazione sistema SEMPREVICINI" +
                               "</span></font></div><!-- padding -->" +
                               "<div style = \"height: 30px; line-height: 30px; font-size: 10px;\">&nbsp;</div></td></tr><tr>" +
                               "<td align = \"center\">" +
                               "<div style=\"line-height: 30px;\">" +
                               "<font face = \"Arial, Helvetica, sans-serif\" size=\"5\" color=\"#4db3a4\" style=\"font-size: 17px;\">" +
                               "<span style = \"font-family: Arial, Helvetica, sans-serif; font-size: 17px; color: #4db3a4;\" >" +
                               "Gentile @Model.Name @Model.Surname la sua utenza è stata registrata nei nostri sistemi informatici. Le sue credenziali di accesso sono: " +
                               "</span></font></div>" +
                               "<!-- padding --><div style = \"height: 35px; line-height: 35px; font-size: 10px;\">&nbsp;</div></td></tr>" +
                               "<tr><td align = \"center\">" +
                               "<table width=\"80%\" align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
                               "<tr><td align = \"center\"><div style=\"line-height: 24px;\">" +
                               "<font face = \"Arial, Helvetica, sans-serif\" size=\"4\" color=\"#57697e\" style=\"font-size: 16px;\">" +
                               "<span style = \"font-family: Arial, Helvetica, sans-serif; font-size: 15px; color: #57697e;\">" +
                               "<b>@Model.AppUrl</b><br/><b>@Model.Username<br/>@Model.Password</b></span ></font></div></td></tr></table>" +
                               "<!--padding-->" +
                               "<div style=\"height: 45px; line-height: 45px; font-size: 10px;\">&nbsp;</div></td></tr><tr>" +
                               "<td align = \"center\">" +
                               "<div style=\"line-height: 30px;\">" +
                               "<font face = \"Arial, Helvetica, sans-serif\" size=\"5\" color=\"#4db3a4\" style=\"font-size: 17px;\">" +
                               "<span style = \"font-family: Arial, Helvetica, sans-serif; font-size: 17px; color: #4db3a4;\">" +
                               "<br/>" +
                               "<font face = \"Arial, Helvetica, sans-serif\" size=\"5\" color=\"#4db3a4\" style=\"font-size: 12px;\">" +
                               "<span style = \"font-family: Arial, Helvetica, sans-serif; font-size: 12px; color: #4db3a4;\">" +
                               "<u><b> Per assistenza contattare: <a href=\"mailto:assistenza@igcom.it\" target=\"_top\">assistenza@igcom.it</a>.</b></u></span></font></div>" +
                               "<!-- padding -->" +
                               "<div style = \"height: 35px; line-height: 35px; font-size: 10px;\"> &nbsp;</div></td></tr></table></td></tr>" +
                               "<!--content 1 END-->" +
                               "<!--footer -->" +
                               "<tr><td class=\"iage_footer\" align=\"center\" bgcolor=\"#3b434c\" style=\"text-decoration: none; color: #929ca8;\">" +
                               "<!-- padding --><div style = \"height: 40px; line-height: 40px; font-size: 10px;\" > &nbsp;</div>" +
                               "<table width = \"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
                               "<tr><td align = \"center\">" +
                               "<font face=\"Arial, Helvetica, sans-serif\" size=\"3\" color=\"#96a5b5\" style=\"font-size: 13px;\">" +
                               "<span style = \"font-family: Arial, Helvetica, sans-serif; font-size: 13px; color: #96a5b5;\" >2019 & copy; SEMPREVICINI Rights Reserved.</span></font></td></tr></table>" +
                               "<!-- padding -->" +
                               "<div style = \"height: 50px; line-height: 50px; font-size: 10px;\">&nbsp;</div></td></tr><!--footer END-->" +
                               "</table><!--[if gte mso 10]></td></tr></table><![endif]--></td></tr></table></div></body></html>";

            #endregion Template

            Result = Engine.Razor.RunCompile(template, StringHelper.RandomString(3), null, new { Name = name, Password = password, Username = username, Surname = surname, AppUrl = appUrl });
        }
    }
}