using Web.UI.Controllers;
using System.Collections.Generic;

namespace Web.UI.ViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel(string content)
        {
                Content = content;
        }
        
        public string Content { get; set; } = "asdf";
    }
}
