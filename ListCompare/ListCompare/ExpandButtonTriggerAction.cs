using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ListCompare
{
    public class ExpandButtonTriggerAction : TriggerAction<Button>
    {
        protected async override void Invoke(Button button)
        {
            await button.ScaleTo(0.90, 100, Easing.CubicOut);
            await button.ScaleTo(1, 100, Easing.CubicIn);
        }
    }
}
