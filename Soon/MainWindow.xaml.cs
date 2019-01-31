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
    /// کلاس سازنده ی اصلی
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Default Constructor
        /// <summary>
        /// کلاس سازنده پیش فرض
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            win1 = this;

        }
        #endregion

        #region Public variable

        /// <summary>
        /// لینک اتصال
        /// </summary>
        public readonly string connectionString = "Data Source = DataBase.db; version = 3;New = False;Compress = True;";
        /// <summary>
        /// کنترل صفحات فریم
        /// </summary>
        public static MainWindow win1;
        /// <summary>
        /// برای ویرایش
        /// </summary>
        public static person pointer_Person;
        /// <summary>
        /// check Update Or New
        /// </summary>
        public static bool Check_Update = false;
        #endregion

        #region windows close
        private void AppWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                for (int intCounter = App.Current.Windows.Count - 1; intCounter >= 0; intCounter--)
                    App.Current.Windows[intCounter].Close();
            }
            catch { }

            win1.Close();
        }

        #endregion

        #region new record
        /// <summary>
        /// سطر جدید
        /// </summary>
        private void newRecord_Click(object sender, RoutedEventArgs e)
        {
            // update
            if (Check_Update) 
            {
                // check Empty
                if (txtFullName.Text.Length == 0) return;
                if (txtField.Text.Length == 0) return;

                // Query
                string Query = "update [person] set [FullName] = '" + txtFullName.Text + "', [Field] = '" + txtField.Text + "', [NumberID] = '" + txtNumberID.Text + "'" +
                    "where [Id] = "+pointer_Person.Id+";";

                SQLiteConnection con = new SQLiteConnection(connectionString);
                try
                {
                    con.Open();
                    SQLiteCommand cmd = new SQLiteCommand(Query, con);
                    cmd.ExecuteNonQuery();
                    // View
                    reload_Click(null, null);
                    // clear form
                    txtFullName.Text = "";
                    txtField.Text = "";
                    txtNumberID.Text = "";

                }catch(Exception ex) { MessageBox.Show(ex.Message); }
                finally { con.Close(); }

                Check_Update = false;
            }
            // new Record
            else
            {
                // Get Text From [TextBox] Then, add to DataBase
                string Query = "insert into [person] ( FullName,Field , NumberID) values  ('" + txtFullName.Text + "','" + txtField.Text + "', '" 
                    + txtNumberID.Text + "');";
                SQLiteConnection con = new SQLiteConnection(connectionString);
                try
                {
                    con.Open();
                    SQLiteCommand cmd = new SQLiteCommand(Query, con);
                    cmd.ExecuteNonQuery();
                    Holder.Children.Add(new person(0, txtFullName.Text, txtField.Text, txtNumberID.Text));

                    // Reload Holder
                    reload_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        #endregion

        #region search
        /// <summary>
        /// جستجو
        /// </summary>
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            // clear list
            Holder.Children.Clear();

            // Get Data
            string t1 = "♥";
            string t2 = "♥";
            string t3 = "♥";
            // check Empty string
            if (txtFullName.Text.Length != 0)
                t1 = txtFullName.Text;
            if (txtField.Text.Length != 0)
                t2 = txtField.Text;
            if (txtNumberID.Text.Length != 0)
                t3 = txtNumberID.Text;
            // Query
            string Query = "select * from [person] where [FullName] like '%" + t1 + "%' or [Field] like '%" + t2 + "%' or [NumberID] like '%" + t3 + "%'";
            SQLiteConnection con = new SQLiteConnection(connectionString);
            try
            {
                con.Open();
                SQLiteCommand cmd = new SQLiteCommand(Query, con);
                cmd.ExecuteNonQuery();
                SQLiteDataReader DB = cmd.ExecuteReader();
                if (DB.HasRows)
                {
                    // add item to list
                    while (DB.Read())
                    {
                        Holder.Children.Add(new person(DB.GetInt32(0), DB.GetString(1), DB.GetString(2), DB.GetString(3)));
                    }
                }
                else
                {
                    Holder.Children.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Reload

        /// <summary>
        /// خواندن تمام سطرهای دیتابیس
        /// </summary>
        private void reload_Click(object sender, RoutedEventArgs e)
        {
            // Select Query
            string Query = "Select * from [person]";

            // Link Connection To DataBase
            SQLiteConnection con = new SQLiteConnection(connectionString);

            try
            {
                // Open Link Connection
                con.Open();

                // Command To Execute Order
                SQLiteCommand cmd = new SQLiteCommand(Query, con);
                cmd.ExecuteNonQuery();

                // Read Data (If Has Data)
                SQLiteDataReader DB = cmd.ExecuteReader();

                if (DB.HasRows)
                {
                    // If hava Data, Should Be Clear [Holder] StackPanel 
                    Holder.Children.Clear();

                    while (DB.Read())
                    {
                        // Now , Read Data Then ==> Add To [Holder]
                        Holder.Children.Add(new person(DB.GetInt32(0), DB.GetString(1), DB.GetString(2), DB.GetString(3))); // OK
                    }
                }
            }
            catch { }
            finally
            {
                // Close Link Connection
                con.Close();
            }
            // Ctrl + F5 


            //      :)

        }
        #endregion

        #region Loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            reload_Click(null, null);
        }
        #endregion


        #region About ME
        private void details_Click(object sender, RoutedEventArgs e)
        {
            details.Visibility = Visibility.Visible;
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            details.Visibility = Visibility.Hidden;
        }
        #endregion
    }
}
