using WF_FileToAzureBlob.Controller;

namespace WF_FileToAzureBlob
{
    public partial class Form1 : Form
    {
        MyBlobController blobController;

        public Form1()
        {
            blobController= new MyBlobController();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) => blobController.ShowAll();
       
        private async void button2_Click(object sender, EventArgs e) => blobController.Upload();

        private void buttonDel_Click(object sender, EventArgs e) => blobController.Remuve();

        private void listBox1_DoubleClick(object sender, EventArgs e) => blobController.Download();

        private void textBox1_TextChanged(object sender, EventArgs e) => blobController.Search();

        private void buttonRead_Click(object sender, EventArgs e) => blobController.ReadTXT();
    }
}