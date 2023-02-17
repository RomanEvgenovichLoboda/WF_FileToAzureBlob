using Azure.Storage.Blobs;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace WF_FileToAzureBlob
{
    public partial class Form1 : Form
    {
        string BlobStorageConnectionString;
        string BlobStorageContainerName;
        CloudBlobClient backupBlobClient ;
        CloudBlobContainer backupContainer ;
        BlobContainerClient container;
        public Form1()
        {
            BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=reml;AccountKey=1UVBOD+dzMKVm99zryAK2Q+QpCrCC7HyAootXMkoa9nR9NHwNQJeIKbwYlcOVealsGEddqmDjitr+AStk0zfEQ==;EndpointSuffix=core.windows.net";
            BlobStorageContainerName = "$web";
            backupBlobClient = CloudStorageAccount.Parse(BlobStorageConnectionString).CreateCloudBlobClient();
            backupContainer = backupBlobClient.GetContainerReference(BlobStorageContainerName);
            container = new BlobContainerClient(BlobStorageConnectionString, BlobStorageContainerName);

            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            var listName = backupContainer.ListBlobs().OfType<CloudBlockBlob>().Select(b => b.Name).ToList();
            listName.ForEach(b => { listBox1.Items.Add(b); });
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string name = openFileDialog.SafeFileName;
                var blob = container.GetBlobClient(name);
                var stream = File.OpenRead(filePath);
                await blob.UploadAsync(stream);
                button1_Click(sender, e);
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            container.GetBlobClient(listBox1.SelectedItem.ToString()).DeleteIfExists();
            button1_Click(sender, e);
        }


        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = listBox1.SelectedItem.ToString();

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    container.GetBlobClient(listBox1.SelectedItem.ToString()).DownloadTo(myStream);
                    myStream.Close();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            var listName = backupContainer.ListBlobs().OfType<CloudBlockBlob>().Select(b => b.Name).Where(x=>x.Contains(textBox1.Text)).ToList();
            listName.ForEach(x => listBox1.Items.Add(x));
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            var list = backupContainer.ListBlobs().OfType<CloudBlockBlob>().ToList();
            CloudBlockBlob blob = list.FirstOrDefault(item => item.Name == listBox1.SelectedItem.ToString());
            string str = blob.DownloadText();
            MessageBox.Show(str);
        }
    }
}