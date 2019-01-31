using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;

namespace Soon
{
    /// <summary>
    /// نمایش آیتم های پایگاه داده
    /// </summary>
    public partial class person : UserControl
    {
        #region Default Constructor

        /// <summary>
        /// کلاس سازنده ی پیش فرض
        /// </summary>
        public person()
        {
            InitializeComponent();
        }
        #endregion

        #region Custom Constructor

        /// <summary>
        /// کلاس سازنده ی شخصی
        /// </summary>
        /// <param name="_Id">آیدی سطر</param>
        /// <param name="_FullName">نام کامل</param>
        /// <param name="_Field">رشته</param>
        /// <param name="_NumberID">شناسه</param>
        public person(int _Id, string _FullName, string _Field, string _NumberID)
        {
            InitializeComponent();

            Id = _Id;
            FullName = _FullName;
            Field = _Field;
            NumberID = _NumberID;
        }
        #endregion

        #region Private Variable
        private string FullName, Field, NumberID;

        #endregion

        #region Public variable
        public int Id;

        #endregion

        #region Loaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtId.Text = Id.ToString();
            txtFullName.Text = FullName;
            txtField.Text = Field;
            txtNumberID.Text = NumberID;
        }
        #endregion

        #region Remove

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            // Query
            string Query = "delete from [person] where [Id] =" + Id ;
            SQLiteConnection con = new SQLiteConnection(MainWindow.win1.connectionString);
            try
            {
                // Data Base
                con.Open();
                SQLiteCommand cmd = new SQLiteCommand(Query, con);
                cmd.ExecuteNonQuery();
                // View
                MainWindow.win1.Holder.Children.Remove(this);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Edid
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // check Update
            MainWindow.Check_Update = true;
            // pointer
            MainWindow.pointer_Person = this;
            // View
            MainWindow.win1.txtFullName.Text = txtFullName.Text;
            MainWindow.win1.txtField.Text = txtField.Text;
            MainWindow.win1.txtNumberID.Text = txtNumberID.Text;

        }
        #endregion
    }
}
