using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
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

            foreach (string license in tempLicenseList)
            {
                if (license == "")
                {
                    JcbLicense += "\r\n\r\n";
                }
                else
                {
                    JcbLicense += license + " ";
                }
            }

            List<string> diLicenseList = TxtAccess.ReadText(Path.Combine(Environment.CurrentDirectory, "Licenses", "Microsoft.Extensions.DependencyInjection License.txt"));
            List<string> toolkitLicenseList = TxtAccess.ReadText(Path.Combine(Environment.CurrentDirectory, "Licenses", "CommunityToolkit.Mvvm License.txt"));

            foreach (string license in diLicenseList)
            {
                if (license == "")
                {
                    DependencyLicenses["Dependency Injection"] += "\r\n\r\n";
                }
                else
                {
                    DependencyLicenses["Dependency Injection"] += license + " ";
                }
            }

            foreach (string license in toolkitLicenseList)
            {
                if (license == "")
                {
                    DependencyLicenses["Community Toolkit"] += "\r\n\r\n";
                }
                else
                {
                    DependencyLicenses["Community Toolkit"] += license + " ";
                }
            }
        }

        #endregion
    }
}
