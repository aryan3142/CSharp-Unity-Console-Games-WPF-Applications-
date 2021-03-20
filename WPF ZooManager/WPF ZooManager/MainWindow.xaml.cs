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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WPF_ZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();
            //when  we add a data source we need to add our sql string
            //this is a  db connection string that allows our xaml.cs file to connect to the database
            string connectionString = ConfigurationManager.ConnectionStrings["WPF_ZooManager.Properties.Settings.MytestDBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            ShowZoos();
            ShowAnimals();
        }
        private void ShowZoos()
        {
            try
            {
                string query = "select * from Zoo";
                //the SqlDataAdapter will run the passed query on the passed sqlConnection which is connecting to the database
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                //we dont need to open a connection or close a conenction to the database because the sqladapter will
                //take care of that

                using (sqlDataAdapter)
                {
                    //allows us to store data from table in an object
                    DataTable zooTable = new DataTable();

                    //filling the object zooTable from the data acquired b the sqlDataAdapter
                    sqlDataAdapter.Fill(zooTable);

                    //which information of the table in datatable should be shown in our listbox
                    ListZoos.DisplayMemberPath = "Location";
                    //which value should be delivered when an item from our listbox is selected     
                    ListZoos.SelectedValuePath = "Id";
                    //the reference to the data the listbox should populate
                    ListZoos.ItemsSource = zooTable.DefaultView;

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void ShowAnimals()
        {
            try
            {
                string query = "select * from Animal";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    listAnimals.DisplayMemberPath = "Name";

                    listAnimals.SelectedValuePath = "Id";

                    listAnimals.ItemsSource = dataTable.DefaultView;
                }

            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /*
        private void AddAnimalToZoo()
        {
            try
            {
                string query = "Insert into Zoo (Location) values (@Name);";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@Name", ListZoos.SelectedValue);

                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        */

        
        private void ShowAssociatedAnimals()
        {
            try
            {
                string query = "select a.Name,a.Id from Animal a inner join ZooAnimal za on a.Id = za.AnimalId where za.ZooId = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooID", ListZoos.SelectedValue);

                    DataTable animalTable = new DataTable();

                    sqlDataAdapter.Fill(animalTable);


                  

                    listAssociatedAnimals.DisplayMemberPath = "Name";

                    listAssociatedAnimals.SelectedValuePath = "Id";

                    listAssociatedAnimals.ItemsSource = animalTable.DefaultView;

                }

            }
            catch (Exception e)
            {
               // MessageBox.Show(e.ToString());
            }

        }

        

        private void ListZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssociatedAnimals();
            ShowSelectedZooInDataTable();
        }

     /*   private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddAnimalToZoo();
        }*/

        //deleting a zoo
        private void Delete_Zoo(object sender,RoutedEventArgs e)
        {
            try
            {
                string query = "Delete from Zoo where id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", ListZoos.SelectedValue);
                sqlCommand.ExecuteScalar();
           
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }
        }
        //adding a zoo
        private void AddZoo_click(object sender,RoutedEventArgs e)
        {
            try
            {
                string query = "Insert into Zoo values(@Location)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", myTextBox1.Text);
                sqlCommand.ExecuteScalar();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }
        }
        private void DeleteAnimal_click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "Delete from Animal where id = @Id";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Id", listAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAnimals();
            }
        }
        private void AddAnimal_click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "Insert into Animal values(@Name)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", myTextBox1.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAnimals();
            }
        }

        private void UpdateAnimal_Click(object Sender,RoutedEventArgs e)
        {
            try
            {
                string query = "update Animal SET Name = @Name Where Id = @Id";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", myTextBox1.Text);
                sqlCommand.Parameters.AddWithValue("@Id", listAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAnimals();
            }

        }

        private void UpdateZoo_Click(object Sender,RoutedEventArgs e)
        {
            try
            {
                string query = "update Zoo SET Location = @Name Where Id = @Id";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", myTextBox1.Text);
                sqlCommand.Parameters.AddWithValue("@Id",ListZoos.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }
        }

        private void AddAnimalToZoo_Click(object sender,RoutedEventArgs e)
        {
            try
            {
                string query = "insert into ZooAnimal values (@AnimalId,@ZooId)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", ListZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();            
                ShowAssociatedAnimals();
            }
        }
        private void DeleteAnimalFromZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "Delete from ZooAnimal Where AnimalId = @AnimalId AND ZooId = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", ListZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAssociatedAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAssociatedAnimals();
            }
        }

        private void ShowSelectedZooInDataTable()
        {
            try
            {
                string query = "select Location from Zoo where Id = @Id";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@Id", ListZoos.SelectedValue);

                    DataTable zooTable = new DataTable();

                    sqlDataAdapter.Fill(zooTable);

                    myTextBox1.Text = zooTable.Rows[0]["Location"].ToString();
                }
            }
            catch(Exception e)
            {
                //
            }

        }
        private void ShowSelectedAnimalInDataTable()
        {
            try
            {
                string query = "select Name from Animal where Id = @Id";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@Id", listAnimals.SelectedValue);

                    DataTable zooTable = new DataTable();

                    sqlDataAdapter.Fill(zooTable);

                    myTextBox1.Text = zooTable.Rows[0]["Name"].ToString();
                }
            }
            catch (Exception e)
            {
                //
            }

        }

        private void listAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedAnimalInDataTable();
        }




    }
}
