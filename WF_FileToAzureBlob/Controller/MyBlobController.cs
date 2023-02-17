using Microsoft.Azure.Storage.Blob;
using WF_FileToAzureBlob.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WF_FileToAzureBlob.Controller
{
    internal class MyBlobController
    {
        MyBlobModel myBlobModel;
        public MyBlobController() 
        {
            myBlobModel = new MyBlobModel(); 
        }
        public void ShowAll()
        {

            Program.myForm.listBox1.Items.Clear();
            var listName = myBlobModel.backupContainer.ListBlobs().OfType<CloudBlockBlob>().Select(b => b.Name).ToList();
            listName.ForEach(b => { Program.myForm.listBox1.Items.Add(b); });
        }
        public async void Upload()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string name = openFileDialog.SafeFileName;
                var blob = myBlobModel.container.GetBlobClient(name);
                var stream = File.OpenRead(filePath);
                await blob.UploadAsync(stream);
                ShowAll();
            }
        }
        public void Remuve()
        {
            try
            {
                myBlobModel.container.GetBlobClient(Program.myForm.listBox1.SelectedItem.ToString()).DeleteIfExists();
                ShowAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        public void Download()
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = Program.myForm.listBox1.SelectedItem.ToString();

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myBlobModel.container.GetBlobClient(Program.myForm.listBox1.SelectedItem.ToString()).DownloadTo(myStream);
                    myStream.Close();
                }
            }
        }
        public void Search()
        {
            Program.myForm.listBox1.Items.Clear();
            var listName = myBlobModel.backupContainer.ListBlobs().OfType<CloudBlockBlob>().Select(b => b.Name).Where(x => x.Contains(Program.myForm.textBox1.Text)).ToList();
            listName.ForEach(x => Program.myForm.listBox1.Items.Add(x));
        }
        public void ReadTXT()
        {
            try
            {
                var list = myBlobModel.backupContainer.ListBlobs().OfType<CloudBlockBlob>().ToList();
                CloudBlockBlob blob = list.FirstOrDefault(item => item.Name == Program.myForm.listBox1.SelectedItem.ToString());
                string str = blob.DownloadText();
                MessageBox.Show(str,"Text", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            
        }
    }
}
