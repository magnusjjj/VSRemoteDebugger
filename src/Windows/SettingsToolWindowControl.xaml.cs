using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace VSRemoteDebugger.Windows
{
    /// <summary>
    /// Interaction logic for ToolWindow1Control.
    /// </summary>
    public partial class ToolWindow1Control : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindow1Control"/> class.
        /// </summary>
        public ToolWindow1Control()
        {
            this.InitializeComponent();
            this.txtHostname.IsEnabled = false;
            this.txtDotnetLocation.IsEnabled = false;
            this.txtGroupName.IsEnabled = false;
            this.txtOutputDirectory.IsEnabled = false;
            this.txtUsername.IsEnabled = false;
            this.txtVsdbgLocation.IsEnabled = false;
            this.chkDontDebug.IsEnabled = false;
            this.chkPublish.IsEnabled = false;
            this.chkUseCommandLineFromProject.IsEnabled = false;
            this.cmbProfile.IsEnabled = false;
            this.btnDelete.IsEnabled = false;
            this.btnRename.IsEnabled = false;

            cmbProfile.ItemsSource = ConfigFile.data;
        }

        public void Enable(bool activate)
        {
            this.txtHostname.IsEnabled = activate;
            this.txtDotnetLocation.IsEnabled = activate;
            this.txtGroupName.IsEnabled = activate;
            this.txtOutputDirectory.IsEnabled = activate;
            this.txtUsername.IsEnabled = activate;
            this.txtVsdbgLocation.IsEnabled = activate;
            this.chkDontDebug.IsEnabled = activate;
            this.chkPublish.IsEnabled = activate;
            this.chkUseCommandLineFromProject.IsEnabled = activate;
            this.cmbProfile.IsEnabled = activate;
            this.btnDelete.IsEnabled = activate;
            this.btnRename.IsEnabled = activate;
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                "ToolWindow1");
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("Name for the new profile"); // Forgive me, but could not arse to create a form and it's guaranteed to exist :')

            if (name.Length == 0)
            {
                return;
            }

            Enable(true);

            ConfigFile.data.Add(name, new ConfigFile.ConfigFileDataRow());
            ConfigFile.Save();
        }
    }
}