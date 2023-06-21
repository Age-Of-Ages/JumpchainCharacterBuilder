using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class AboutViewModel : ViewModelBase
    {
        #region Fields
        [ObservableProperty]
        private string _versionString = "";

        [ObservableProperty]
        private string _jcbLicense = "";

        [ObservableProperty]
        private Dictionary<string, string> _dependencyLicenses = new()
        {
            {"Community Toolkit", "" },
            {"Dependency Injection", "" }
        };

        [ObservableProperty]
        private string _dependencyLicenseSelection = "";

        [ObservableProperty]
        private string _dependencyLicenseString = "";

        #endregion

        #region Properties
        partial void OnDependencyLicenseSelectionChanged(string value)
        {
            if (value != null)
            {
                DependencyLicenseString = DependencyLicenses[value];
            }
        }

        #endregion

        #region Constructor
        public AboutViewModel()
        {
            Assembly? assembly = Assembly.GetEntryAssembly();

            if (assembly != null)
            {
                AssemblyName assemblyName = assembly.GetName();

                if (assemblyName.Version != null)
                {
                    VersionString = "v" + assemblyName.Version.ToString();
                }
            }

            List<string> tempLicenseList = TxtAccess.ReadText(Path.Combine(Environment.CurrentDirectory, "LICENSE.txt"));

            JcbLicense = FormatLicenseString(tempLicenseList);

            List<string> diLicenseList = TxtAccess.ReadText(Path.Combine(Environment.CurrentDirectory, "Licenses", "Microsoft.Extensions.DependencyInjection License.txt"));
            List<string> toolkitLicenseList = TxtAccess.ReadText(Path.Combine(Environment.CurrentDirectory, "Licenses", "CommunityToolkit.Mvvm License.txt"));

            DependencyLicenses["Dependency Injection"] = FormatLicenseString(diLicenseList);
            DependencyLicenses["Community Toolkit"] = FormatLicenseString(toolkitLicenseList);

            DependencyLicenseSelection = DependencyLicenses.First().Key;
        }

        #endregion

        #region Methods
        private static string FormatLicenseString(List<string> licenseStrings)
        {
            string output = "";

            foreach (string line in licenseStrings)
            {
                if (line == "")
                {
                    output += "\r\n\r\n";
                }
                else
                {
                    output += line + " ";
                }
            }

            return output;
        }

        #endregion
    }
}
