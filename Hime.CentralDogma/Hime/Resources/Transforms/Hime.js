﻿function toggle(btn, content)
{
    if (content.style.display == "none")
    {
        content.style.display = "";
        btn.src = "hime_data/button_minus.gif";
    }
    else
    {
        content.style.display = "none";
        btn.src = "hime_data/button_plus.gif";
    }
}