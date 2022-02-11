using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using TextBox = System.Windows.Controls.TextBox;
using UserControl = System.Windows.Controls.UserControl;

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

            //cmbProfile.ItemsSource = ConfigFile.Data.data.Keys;
            //cmbProfile.SelectedItem = ConfigFile.Data.CurrentlySelected;

            if(cmbProfile.Items.Count > 0)
            {
                Enable(true);
            } else
            {
                Enable(false);
            }

            this.txtHostname.Text = ConfigFile.Current.Hostname;
            loadCurrent();
            this.cmbProfile.SelectionChanged -= cmbProfile_SelectionChanged;
            this.cmbProfile.SelectionChanged += cmbProfile_SelectionChanged;
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
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ConfigFile.Current.DotnetLocation = txtDotnetLocation.Text;
            ConfigFile.Current.GroupName = txtGroupName.Text;
            ConfigFile.Current.Hostname = txtHostname.Text;
            ConfigFile.Current.OutputDirectory = txtOutputDirectory.Text;
            ConfigFile.Current.Username = txtUsername.Text;
            ConfigFile.Current.VsdbgLocation = txtVsdbgLocation.Text;
//            ConfigFile.Data.data[ConfigFile.Data.CurrentlySelected].DontDebug = chkDontDebug.IsChecked;
            ConfigFile.Current.Publish = (bool)chkPublish.IsChecked;
            ConfigFile.Current.DontDebug = (bool)chkDontDebug.IsChecked;
            ConfigFile.Current.UseCommandLineFromProject = (bool)chkUseCommandLineFromProject.IsChecked;
            ConfigFile.Save();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("Name for the new profile"); // Forgive me, but could not arse to create a form and it's guaranteed to exist :')

            if (name.Length == 0)
            {
                return;
            }

            Enable(true);


            ConfigFile.Data.data.Add(name, new ConfigFileDataRow());
            ConfigFile.Data.CurrentlySelected = name;
            ConfigFile.Save();


        }

        private void loadCurrent()
        {
            txtDotnetLocation.Text = ConfigFile.Current.DotnetLocation;
            txtGroupName.Text = ConfigFile.Current.GroupName;
            txtHostname.Text = ConfigFile.Current.Hostname;
            txtOutputDirectory.Text = ConfigFile.Current.OutputDirectory;
            txtUsername.Text = ConfigFile.Current.Username;
            txtVsdbgLocation.Text = ConfigFile.Current.VsdbgLocation;
            chkPublish.IsChecked = ConfigFile.Current.Publish;
            chkDontDebug.IsChecked = ConfigFile.Current.DontDebug;
            chkUseCommandLineFromProject.IsChecked = ConfigFile.Current.UseCommandLineFromProject;
        }

        private void cmbProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConfigFile.Data.CurrentlySelected = (string)e.AddedItems[0];

            loadCurrent();
        }


    }
}