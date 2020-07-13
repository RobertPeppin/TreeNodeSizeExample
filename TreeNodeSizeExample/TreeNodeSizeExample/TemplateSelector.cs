using Syncfusion.TreeView.Engine;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TreeNodeSizeExample
{
    public class TemplateSelector : DataTemplateSelector
    {
        public DataTemplate DocumentTemplate { get; set; }

        public DataTemplate DocumentGroupHeaderTemplate { get; set; }

        public DataTemplate EmptyTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is TreeViewNode node)
            {
                if (node.Content is TreeGroup groupHeader)
                {
                    return DocumentGroupHeaderTemplate;
                }

                return DocumentTemplate;
            }
            return EmptyTemplate;
        }
    }
}
