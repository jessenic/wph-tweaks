using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace wphTweaks
{
    class AssociationUriMapper : UriMapperBase
    {
        private string tempUri;
        const string uribase = "/Protocol?encodedLaunchUri=";
        public override Uri MapUri(Uri uri)
        {
            tempUri = System.Net.HttpUtility.UrlDecode(uri.ToString());
            if (tempUri.ToLower().StartsWith(uribase.ToLower()))
            {
                tempUri = tempUri.Remove(0, uribase.Length);
                if (tempUri.ToLower().StartsWith("wphtweaks:"))
                {
                    return new Uri("/MainPage.xaml?source=urimap&fullUri=" + uri.ToString(), UriKind.Relative);
                }
            }

            // Otherwise perform normal launch.
            return uri;
        }
    }
}
