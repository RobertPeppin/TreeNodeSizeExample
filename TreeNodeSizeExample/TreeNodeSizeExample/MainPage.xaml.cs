using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TreeNodeSizeExample
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        Dictionary<Guid, double> NodeSize = new Dictionary<Guid, double>();
        Dictionary<Guid, string> NodeContent = new Dictionary<Guid, string>();
        public MainPage()
        {
            InitializeComponent();

        }

        private void TreeView_QueryNodeSize(object sender, Syncfusion.XForms.TreeView.QueryNodeSizeEventArgs e)
        {
            var calculatedNodeSize = e.GetActualNodeHeight();
            var node = e.Node.Content as ITreeItem;
            Debug.WriteLine($"Node {node.Id} header {node.Header} with {node.Items.Count} children is {calculatedNodeSize}dp high");

            if (NodeSize.ContainsKey(node.Id))
            {
                if (NodeContent[node.Id] == node.Header)
                {
                    // should be the same
                    Debug.WriteLine($"Content same, previous size = {NodeSize[node.Id]} new size {calculatedNodeSize}");
                }
                else
                {
                    // the content is different, so the size should be different
                    Debug.WriteLine($"Content different, previous size = {NodeSize[node.Id]} new size {calculatedNodeSize}");

                    if (NodeSize[node.Id] == calculatedNodeSize)
                    {
                        Debug.WriteLine($"Content different, but size hasnt changed");
                    }

                    NodeContent[node.Id] = node.Header;
                    NodeSize[node.Id] = calculatedNodeSize;
                }
            }
            else
            {
                NodeSize.Add(node.Id, calculatedNodeSize);
                NodeContent.Add(node.Id, node.Header);
            }

            e.Height = e.GetActualNodeHeight();
            e.Handled = true;
        }
    }
}
