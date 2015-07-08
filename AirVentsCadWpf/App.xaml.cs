using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using AirVentsCadWpf.Properties;

namespace AirVentsCadWpf
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        static App()
        {
            try
            {
                FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// Gets the SQL connection string.
        /// </summary>
        /// <value>
        /// The SQL connection string.
        /// </value>
        public static string SqlTestConnectionString
        {
            get
            {
                var builder = new SqlConnectionStringBuilder();
                builder["Data Source"] = "192.168.14.11";
                builder["Initial Catalog"] = Settings.Default.TestSqlConnection;//"SWPlusDB";
                builder["User ID"] = "sa";
                builder["Password"] = "PDMadmin";
                builder["Persist Security Info"] = true;
                return builder.ConnectionString;
            }
        }

        /// <summary>
        /// Gets the SQL connection string.
        /// </summary>
        /// <value>
        /// The SQL connection string.
        /// </value>
        public static string SqlConnectionString
        {//Data Source=192.168.14.11;Initial Catalog=SWPlusDB;User ID=sa;Password=PDMadmin
            get
            {
                var builder = new SqlConnectionStringBuilder();
                builder["Data Source"] = "192.168.14.11";
                builder["Initial Catalog"] = Settings.Default.WorkingSqlConnection;//"SWPlusDB";
                builder["User ID"] = "sa";
                builder["Password"] = "PDMadmin";
                builder["Persist Security Info"] = true;
                return builder.ConnectionString;
            }
        }
    }
}