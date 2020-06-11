using System;
using WolfyJudgement;
using WolfyJudgement.macOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

[assembly: ExportRenderer(typeof(CustomMasterPage), typeof(CustomMasterPageRenderer))]
namespace WolfyJudgement.macOS
{
    public class CustomMasterPageRenderer : MasterDetailPageRenderer
    {
        protected override double MasterWidthPercentage => 0.2;
    }
}
